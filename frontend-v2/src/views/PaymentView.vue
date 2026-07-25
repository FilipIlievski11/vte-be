<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import type { PaymentDetail } from '@/types';
import Button from 'primevue/button';
import Tag from 'primevue/tag';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputNumber from 'primevue/inputnumber';
import Dialog from 'primevue/dialog';
import Textarea from 'primevue/textarea';
import { useToast } from 'primevue/usetoast';
import { printFiscalForDocument } from '@/fiscal/fiscal';

const props = defineProps<{ id: string }>();
const { t } = useI18n();
const router = useRouter();
const toast = useToast();

const bill = ref<PaymentDetail | null>(null);
const loading = ref(true);

async function load() {
  loading.value = true;
  try {
    bill.value = (await api.get<PaymentDetail>(`/payment-documents/${props.id}`)).data;
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('payments.loadFailed'), detail: e?.response?.data ?? e?.message, life: 4000 });
  } finally {
    loading.value = false;
  }
}
onMounted(load);

const payingSeq = ref<number | null>(null);
async function payInstallment(seq: number) {
  payingSeq.value = seq;
  try {
    await api.post(`/payment-documents/${props.id}/installments/${seq}/pay`);
    // fiscal receipt for this rata (quiet — needs the configured folder)
    const fiscal = await printFiscalForDocument(props.id, false, seq);
    await load();
    toast.add({ severity: 'success', summary: t('payments.installmentPaid', { n: seq }),
      detail: fiscal.status === 'printed' ? t('fiscal.printedOk') : undefined, life: 3000 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('payments.installmentPayFailed'),
      detail: e?.response?.data?.error ?? e?.message, life: 4500 });
  } finally {
    payingSeq.value = null;
  }
}

// ---- Inline line-price editing (legacy bill grid let the operator change the price) ----
const editLineId = ref<number | null>(null);
const editPrice = ref<number | null>(null);
const savingPrice = ref(false);
function beginEditPrice(line: { id: number; unitPrice: number }) {
  if (bill.value?.stornoed) return;
  editLineId.value = line.id;
  editPrice.value = line.unitPrice;
}
function cancelEditPrice() { editLineId.value = null; editPrice.value = null; }
async function savePrice() {
  if (editLineId.value == null || editPrice.value == null || savingPrice.value) return;
  savingPrice.value = true;
  try {
    await api.put(`/payment-documents/${props.id}/lines/${editLineId.value}/price`, { unitPrice: editPrice.value });
    cancelEditPrice();
    await load();
    toast.add({ severity: 'success', summary: t('payments.priceSaved'), life: 2500 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('payments.priceSaveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4500 });
  } finally {
    savingPrice.value = false;
  }
}

// ---- СМЕТКОПОТВРДА print + платено/неплатено toggle ----
function openReceiptPrint() {
  const href = router.resolve({ name: 'payment-receipt-print', params: { id: props.id } }).href;
  window.open(href, '_blank');
}
const paidBusy = ref(false);
async function togglePaid() {
  if (!bill.value || paidBusy.value) return;
  paidBusy.value = true;
  try {
    await api.put(`/payment-documents/${props.id}/paid`, { paid: !bill.value.paid });
    await load();
    toast.add({ severity: 'success', summary: t('payments.statusSaved'), life: 2500 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('payments.statusSaveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4500 });
  } finally {
    paidBusy.value = false;
  }
}

// ---- Storno (legacy btnStorno: flag + storno fiscal receipt; v2 also re-opens the debts) ----
const stornoDialog = ref(false);
const stornoReason = ref('');
const stornoBusy = ref(false);
const canStorno = computed(() =>
  !!bill.value && bill.value.active && !bill.value.stornoed && bill.value.legacyId == null);
function openStorno() {
  stornoReason.value = '';
  stornoDialog.value = true;
}
async function confirmStorno() {
  if (!stornoReason.value.trim()) {
    toast.add({ severity: 'warn', summary: t('payments.stornoDialog.reasonRequired'), life: 3000 });
    return;
  }
  stornoBusy.value = true;
  try {
    const res = await api.post<{ stornoed: boolean; reopenedDebts: number }>(
      `/payment-documents/${props.id}/storno`, { reason: stornoReason.value.trim() });
    stornoDialog.value = false;
    await load();
    toast.add({ severity: 'success', summary: t('payments.stornoDialog.done'),
      detail: res.data.reopenedDebts > 0 ? t('payments.stornoDialog.debtsReopened', { n: res.data.reopenedDebts }) : undefined,
      life: 4500 });
    // Legacy reprinted the receipt as a fiscal STORNO one right after saving the flag.
    await printFiscal();
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('payments.stornoDialog.failed'),
      detail: e?.response?.data?.error ?? e?.message, life: 5000 });
  } finally {
    stornoBusy.value = false;
  }
}

const fiscalBusy = ref(false);
async function printFiscal() {
  fiscalBusy.value = true;
  try {
    const res = await printFiscalForDocument(props.id, true);
    switch (res.status) {
      case 'printed':
        toast.add({ severity: 'success', summary: t('fiscal.printedOk'), life: 3000 });
        await load();
        break;
      case 'skipped':
        toast.add({ severity: 'info', summary: t('fiscal.skipped'), detail: res.reason, life: 4000 });
        break;
      case 'no-folder':
        toast.add({ severity: 'warn', summary: t('fiscal.noFolder'), detail: t('fiscal.goConfigure'), life: 5000 });
        break;
      case 'unsupported':
        toast.add({ severity: 'warn', summary: t('fiscal.unsupportedShort'), life: 5000 });
        break;
      case 'error':
        toast.add({ severity: 'error', summary: t('fiscal.printFailed'), detail: res.message, life: 5000 });
        break;
    }
  } finally {
    fiscalBusy.value = false;
  }
}

function fmtDate(s: string | null): string {
  if (!s) return '—';
  const d = new Date(s); return isNaN(d.getTime()) ? '—' : d.toLocaleDateString();
}
function fmtDateTime(s: string | null): string {
  if (!s) return '—';
  const d = new Date(s); return isNaN(d.getTime()) ? '—' : d.toLocaleString();
}
function fmtMoney(v: number | null | undefined): string {
  if (v == null) return '—';
  return v.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}

const statusTag = computed<{ label: string; severity: 'success' | 'warn' | 'danger' }>(() => {
  const b = bill.value;
  if (!b) return { label: '—', severity: 'warn' };
  if (b.stornoed) return { label: t('payments.status.storno'), severity: 'danger' };
  if (b.paid)     return { label: t('payments.status.paid'),   severity: 'success' };
  return { label: t('payments.status.unpaid'), severity: 'warn' };
});

// Line total = unitPrice * quantity (discount/VAT shown separately, matches legacy display).
function lineSubtotal(l: { unitPrice: number; quantity: number }) {
  return l.unitPrice * l.quantity;
}

// Попуст колоната се појавува само ако навистина постои попуст на некоја ставка.
const hasAnyLineDiscount = computed(() => bill.value?.lines.some(l => l.discount > 0) ?? false);
</script>

<template>
  <div class="pay-detail">
    <div class="page-header">
      <div class="head-left">
        <Button icon="pi pi-arrow-left" text rounded @click="router.push('/payments')"
                v-tooltip.bottom="t('common.back')" />
        <div>
          <h1>
            {{ t('payments.detailTitle') }}
            <span v-if="bill" class="docnum"> · {{ bill.documentNumber }}</span>
          </h1>
          <div class="subtitle" v-if="bill">
            {{ t('payments.col.issued') }}: {{ fmtDate(bill.issueDate) }}
            <span class="muted"> · {{ t('payments.col.due') }}: {{ fmtDate(bill.dueDate) }}</span>
          </div>
        </div>
      </div>
      <div class="head-right" v-if="bill">
        <Tag v-if="bill.fiscalPrintedAt" :value="t('fiscal.printedTag')" severity="info" class="big-tag"
          v-tooltip.bottom="fmtDateTime(bill.fiscalPrintedAt)" />
        <Button :label="t('payments.printReceipt')" icon="pi pi-file" size="small" outlined
          @click="openReceiptPrint" />
        <Button :label="t('fiscal.printReceipt')" icon="pi pi-print" size="small"
          :loading="fiscalBusy" @click="printFiscal" />
        <Button v-if="!bill.stornoed" size="small" outlined
          :severity="bill.paid ? 'warn' : 'success'"
          :icon="bill.paid ? 'pi pi-times-circle' : 'pi pi-check-circle'"
          :label="bill.paid ? t('payments.markUnpaid') : t('payments.markPaid')"
          :loading="paidBusy" @click="togglePaid" />
        <Button v-if="canStorno" :label="t('payments.stornoDialog.button')" icon="pi pi-ban"
          size="small" outlined severity="danger" @click="openStorno" />
        <Tag :value="statusTag.label" :severity="statusTag.severity" class="big-tag" />
      </div>
    </div>

    <Dialog v-model:visible="stornoDialog" modal :header="t('payments.stornoDialog.title')"
            :style="{ width: '30rem' }">
      <p class="storno-warn">
        {{ t('payments.stornoDialog.warning', { doc: bill?.documentNumber ?? '' }) }}
      </p>
      <label class="storno-label" for="storno-reason">{{ t('payments.stornoDialog.reasonLabel') }}</label>
      <Textarea id="storno-reason" v-model="stornoReason" rows="3" autoResize class="storno-reason"
                :placeholder="t('payments.stornoDialog.reasonPlaceholder')" />
      <template #footer>
        <Button :label="t('common.cancel')" text severity="secondary"
                :disabled="stornoBusy" @click="stornoDialog = false" />
        <Button :label="t('payments.stornoDialog.confirm')" icon="pi pi-ban" severity="danger"
                :loading="stornoBusy" :disabled="!stornoReason.trim()" @click="confirmStorno" />
      </template>
    </Dialog>

    <div v-if="loading" class="muted pad">{{ t('common.loading') }}…</div>

    <template v-else-if="bill">
      <!-- Header / parties -->
      <section class="card">
        <h2>{{ t('payments.detailTitle') }}</h2>
        <div class="grid">
          <div class="field span2">
            <label>{{ t('payments.owner') }}</label>
            <div class="val">
              {{ bill.clientName || '—' }}
              <span v-if="bill.clientMB" class="muted small"> · {{ bill.clientMB }}</span>
            </div>
          </div>
          <div class="field span2">
            <label>{{ t('payments.vehicle') }}</label>
            <div class="val">
              <span v-if="bill.vehiclePlate" class="plate">{{ bill.vehiclePlate }}</span>
              <span v-if="bill.vehicleVin"   class="muted">{{ bill.vehicleVin }}</span>
              <span v-if="bill.vehicleMakerModel"> · {{ bill.vehicleMakerModel }}</span>
              <span v-if="!bill.vehiclePlate && !bill.vehicleVin">—</span>
            </div>
          </div>
          <div class="field">
            <label>{{ t('payments.col.type') }}</label>
            <div class="val">{{ bill.paymentTypeName || '—' }}</div>
          </div>
          <div class="field">
            <label>{{ t('payments.col.docNumber') }}</label>
            <div class="val mono">{{ bill.documentNumber }}</div>
          </div>
          <div class="field">
            <label>{{ t('payments.col.issued') }}</label>
            <div class="val">{{ fmtDate(bill.issueDate) }}</div>
          </div>
          <div class="field">
            <label>{{ t('payments.col.due') }}</label>
            <div class="val">{{ fmtDate(bill.dueDate) }}</div>
          </div>
          <!-- Попуст: скриен визуелно (станицата не го користи) — bill.discount и
               пресметките (фискална, сметкопотврда) остануваат недопрени. -->
          <div class="field" v-if="bill.note">
            <label>{{ t('common.note') }}</label>
            <div class="val">{{ bill.note }}</div>
          </div>
          <div class="field" v-if="bill.stornoReason">
            <label>{{ t('payments.stornoReason') }}</label>
            <div class="val">{{ bill.stornoReason }}</div>
          </div>
        </div>
      </section>

      <!-- Lines -->
      <section class="card">
        <h2>{{ t('payments.lines') }} <span class="count">({{ bill.lines.length }})</span></h2>
        <DataTable v-if="bill.lines.length" :value="bill.lines" stripedRows size="small" class="tight-table" dataKey="id">
          <Column :header="t('payments.col.item')">
            <template #body="{ data }">
              <span v-if="data.priceCatalogName" class="item-name">{{ data.priceCatalogName }}</span>
              <span v-if="data.note" class="muted small"> · {{ data.note }}</span>
            </template>
          </Column>
          <Column :header="t('payments.col.qty')" style="width:60px; text-align:right">
            <template #body="{ data }">{{ data.quantity }}</template>
          </Column>
          <Column :header="t('payments.col.unitPrice')" style="width:130px; text-align:right">
            <template #body="{ data }">
              <span v-if="editLineId === data.id" class="price-edit-wrap">
                <InputNumber v-model="editPrice" :minFractionDigits="2" :maxFractionDigits="2" :min="0"
                             inputClass="price-edit-input" autofocus
                             @keydown.enter="savePrice" @keydown.esc="cancelEditPrice" />
                <Button icon="pi pi-check" size="small" text severity="success" :loading="savingPrice" @click="savePrice" />
                <Button icon="pi pi-times" size="small" text severity="secondary" :disabled="savingPrice" @click="cancelEditPrice" />
              </span>
              <span v-else class="mono price-cell" :class="{ editable: !bill?.stornoed && data.active }"
                    :title="!bill?.stornoed && data.active ? t('payments.editPriceHint') : ''"
                    @click="!bill?.stornoed && data.active && beginEditPrice(data)">{{ fmtMoney(data.unitPrice) }}</span>
            </template>
          </Column>
          <Column :header="t('payments.col.vat')" style="width:75px; text-align:right">
            <template #body="{ data }">{{ data.vatPercent }} %</template>
          </Column>
          <!-- Колоната „Попуст" е скриена визуелно — вредноста и натаму учествува
               во пресметките; прикажи ја само ако некоја ставка навистина има попуст. -->
          <Column v-if="hasAnyLineDiscount" :header="t('payments.col.lineDiscount')" style="width:75px; text-align:right">
            <template #body="{ data }">{{ data.discount }} %</template>
          </Column>
          <Column :header="t('payments.col.subtotal')" style="width:110px; text-align:right">
            <template #body="{ data }"><span class="mono">{{ fmtMoney(lineSubtotal(data)) }}</span></template>
          </Column>
        </DataTable>
        <p v-else class="muted">{{ t('payments.noLines') }}</p>
        <div class="totals-row">
          <span class="totals-label">{{ t('payments.total') }}</span>
          <span class="totals-amount mono">{{ fmtMoney(bill.linesTotal) }}</span>
        </div>
      </section>

      <!-- Installment agreement + schedule -->
      <section v-if="bill.agreement || bill.installments.length" class="card">
        <h2>
          {{ t('payments.installmentsTitle') }}
          <span class="count" v-if="bill.installments.length">({{ bill.installments.length }})</span>
        </h2>

        <div v-if="bill.agreement" class="grid mb">
          <div class="field">
            <label>{{ t('payments.agreement.number') }}</label>
            <div class="val mono">{{ bill.agreement.number }}</div>
          </div>
          <div class="field">
            <label>{{ t('payments.agreement.date') }}</label>
            <div class="val">{{ fmtDate(bill.agreement.date) }}</div>
          </div>
          <div class="field">
            <label>{{ t('payments.agreement.totalInstallments') }}</label>
            <div class="val">{{ bill.agreement.totalInstallments }}</div>
          </div>
          <div class="field span2" v-if="bill.agreement.guarantorName">
            <label>{{ t('payments.agreement.guarantor') }}</label>
            <div class="val">
              {{ bill.agreement.guarantorName }}
              <span v-if="bill.agreement.guarantorEmbg" class="muted small"> · {{ bill.agreement.guarantorEmbg }}</span>
            </div>
          </div>
          <div class="field span2" v-if="bill.agreement.guarantorAddress">
            <label>{{ t('payments.agreement.guarantorAddress') }}</label>
            <div class="val">{{ bill.agreement.guarantorAddress }}</div>
          </div>
        </div>

        <DataTable v-if="bill.installments.length" :value="bill.installments" stripedRows size="small" class="tight-table" dataKey="id">
          <Column :header="t('payments.col.installmentNo')" style="width:60px; text-align:center">
            <template #body="{ data }">{{ data.sequenceNo }}</template>
          </Column>
          <Column :header="t('payments.col.amount')" style="width:120px; text-align:right">
            <template #body="{ data }"><span class="mono">{{ fmtMoney(data.amount) }}</span></template>
          </Column>
          <Column :header="t('payments.col.due')" style="width:110px">
            <template #body="{ data }">{{ fmtDate(data.dueDate) }}</template>
          </Column>
          <Column :header="t('payments.col.paidAt')" style="width:140px">
            <template #body="{ data }">{{ fmtDateTime(data.paidAt) }}</template>
          </Column>
          <Column :header="t('payments.col.paidAmount')" style="width:120px; text-align:right">
            <template #body="{ data }"><span v-if="data.paidAmount != null" class="mono">{{ fmtMoney(data.paidAmount) }}</span><span v-else class="muted">—</span></template>
          </Column>
          <Column :header="t('payments.col.status')" style="width:90px">
            <template #body="{ data }">
              <Tag
                :value="data.paid ? t('payments.status.paid') : t('payments.status.unpaid')"
                :severity="data.paid ? 'success' : 'warn'"
              />
            </template>
          </Column>
          <Column :header="''" style="width:120px">
            <template #body="{ data }">
              <Button v-if="!data.paid && !bill.stornoed"
                :label="t('payments.payInstallment')" icon="pi pi-check" size="small"
                :loading="payingSeq === data.sequenceNo" @click="payInstallment(data.sequenceNo)" />
            </template>
          </Column>
        </DataTable>
      </section>

      <!-- Audit -->
      <section class="card">
        <h2>{{ t('payments.audit') }}</h2>
        <div class="grid">
          <div class="field">
            <label>{{ t('payments.col.created') }}</label>
            <div class="val">{{ fmtDateTime(bill.createdAt) }}</div>
          </div>
          <div class="field" v-if="bill.modifiedAt">
            <label>{{ t('payments.col.modified') }}</label>
            <div class="val">{{ fmtDateTime(bill.modifiedAt) }}</div>
          </div>
          <div class="field" v-if="bill.fiscalPrintedAt">
            <label>{{ t('payments.col.fiscalPrintedAt') }}</label>
            <div class="val">{{ fmtDateTime(bill.fiscalPrintedAt) }}</div>
          </div>
          <div class="field" v-if="bill.legacyId">
            <label>{{ t('payments.col.legacyId') }}</label>
            <div class="val mono">{{ bill.legacyId }}</div>
          </div>
          <div class="field" v-if="bill.operatorName || bill.operatorLegacyId">
            <label>{{ t('payments.col.operator') }}</label>
            <div class="val">
              <template v-if="bill.operatorName">{{ bill.operatorName }}
                <span v-if="bill.operatorLegacyId" class="muted small mono">#{{ bill.operatorLegacyId }}</span>
              </template>
              <span v-else class="mono">#{{ bill.operatorLegacyId }}</span>
            </div>
          </div>
        </div>
      </section>
    </template>
  </div>
</template>

<style scoped>
.pay-detail { max-width: 1100px; margin: 0 auto; display: flex; flex-direction: column; gap: 1rem; }
.page-header { display: flex; align-items: center; justify-content: space-between; }
.head-left { display: flex; align-items: center; gap: .5rem; }
.head-right { display: flex; align-items: center; gap: .75rem; }
.page-header h1 { margin: 0; font-size: 1.3rem; }
.page-header .docnum { color: var(--color-text-muted); font-weight: 500; font-family: monospace; }
.subtitle { color: var(--color-text-muted); font-size: .85rem; }
.big-tag :deep(.p-tag), .big-tag { font-size: 1rem; padding: .4rem .8rem; }
.pad { padding: 1rem; }
.muted { color: var(--color-text-muted); }
.small { font-size: .75rem; }
.mono { font-family: monospace; }
.mb { margin-bottom: 1rem; }

.card {
  background: var(--surface-card, #fff);
  border: 1px solid var(--surface-border, #e5e7eb);
  border-radius: 10px;
  padding: 1rem 1.1rem;
}
.card h2 { margin: 0 0 .85rem; font-size: 1rem; }
.card .count { color: var(--color-text-muted); font-weight: 400; }

.grid { display: grid; grid-template-columns: repeat(4, 1fr); gap: .75rem 1rem; }
.field { display: flex; flex-direction: column; gap: .15rem; min-width: 0; }
.field.span2 { grid-column: span 2; }
.field label { font-size: .72rem; text-transform: uppercase; letter-spacing: .02em; color: var(--color-text-muted); }
.field .val { font-size: .9rem; word-break: break-word; }
.plate { display: inline-block; font-family: monospace; font-weight: 600; padding-right: .5rem; }
.item-name { font-weight: 500; }

.storno-warn { margin: 0 0 .75rem; font-size: .9rem; }
.storno-label { display: block; font-size: .72rem; text-transform: uppercase; letter-spacing: .02em; color: var(--color-text-muted); margin-bottom: .25rem; }
.storno-reason { width: 100%; }

.price-cell.editable { cursor: pointer; border-bottom: 1px dashed var(--p-surface-400); }
.price-cell.editable:hover { color: var(--p-primary-600); border-bottom-color: var(--p-primary-400); }
.price-edit-wrap { display: inline-flex; align-items: center; gap: 2px; }
.price-edit-wrap :deep(.price-edit-input) { width: 84px; padding: 2px 6px; text-align: right; font-family: monospace; }
.price-edit-wrap :deep(.p-button) { width: 26px; height: 26px; padding: 0; }

.totals-row {
  display: flex; justify-content: flex-end; align-items: baseline; gap: .75rem;
  margin-top: .65rem; padding-top: .65rem;
  border-top: 1px solid var(--surface-border, #e5e7eb);
}
.totals-label { text-transform: uppercase; font-size: .8rem; color: var(--color-text-muted); }
.totals-amount { font-size: 1.05rem; font-weight: 600; }

.tight-table :deep(.p-datatable-tbody td),
.tight-table :deep(.p-datatable-thead th) { padding: .25rem .5rem; font-size: .8125rem; }
</style>
