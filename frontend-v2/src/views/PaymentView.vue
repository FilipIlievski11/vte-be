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
import { useToast } from 'primevue/usetoast';

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
        <Tag :value="statusTag.label" :severity="statusTag.severity" class="big-tag" />
      </div>
    </div>

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
          <div class="field" v-if="bill.discount != null">
            <label>{{ t('payments.discount') }}</label>
            <div class="val">{{ bill.discount }} %</div>
          </div>
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
          <Column :header="t('payments.col.unitPrice')" style="width:110px; text-align:right">
            <template #body="{ data }"><span class="mono">{{ fmtMoney(data.unitPrice) }}</span></template>
          </Column>
          <Column :header="t('payments.col.vat')" style="width:75px; text-align:right">
            <template #body="{ data }">{{ data.vatPercent }} %</template>
          </Column>
          <Column :header="t('payments.col.lineDiscount')" style="width:75px; text-align:right">
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
          <div class="field" v-if="bill.operatorLegacyId">
            <label>{{ t('payments.col.operator') }}</label>
            <div class="val mono">#{{ bill.operatorLegacyId }}</div>
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
