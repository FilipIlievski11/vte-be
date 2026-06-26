<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { api } from '@/api/client';
import type { Company, Paged, UserListItem } from '@/types';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Password from 'primevue/password';
import Select from 'primevue/select';
import Tag from 'primevue/tag';
import Skeleton from 'primevue/skeleton';
import Dialog from 'primevue/dialog';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm';
import PagedTableEmpty from '@/components/PagedTableEmpty.vue';

const { t } = useI18n();
const toast = useToast();
const confirm = useConfirm();

const items = ref<UserListItem[]>([]);
const total = ref(0);
const loading = ref(true);

const companies = ref<Company[]>([]);

// Filters
const q = ref('');
const companyFilter = ref<number | null>(null);
const roleFilter = ref<string | null>(null);
const page = ref(1);
const pageSize = ref(50);
let searchTimer: number | undefined;

const companyOptions = computed(() => [
  { id: null as number | null, name: t('operators.allCompanies') },
  ...companies.value,
]);

const roleOptions = computed(() => [
  { value: null as string | null, label: t('operators.allRoles') },
  { value: 'Administrator', label: t('operators.role.Administrator') },
  { value: 'Operator',      label: t('operators.role.Operator') },
]);

async function loadCompanies() {
  try { companies.value = (await api.get<Company[]>('/companies')).data; }
  catch { /* ignore */ }
}

async function load() {
  loading.value = true;
  try {
    const { data } = await api.get<Paged<UserListItem>>('/users', {
      params: {
        q: q.value || undefined,
        companyId: companyFilter.value ?? undefined,
        role: roleFilter.value ?? undefined,
        page: page.value,
        pageSize: pageSize.value,
      },
    });
    items.value = data.items;
    total.value = data.total;
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('clients.loadFailed'), detail: e?.message, life: 4000 });
  } finally {
    loading.value = false;
  }
}

onMounted(async () => {
  await loadCompanies();
  await load();
});

watch(q, () => {
  window.clearTimeout(searchTimer);
  searchTimer = window.setTimeout(() => { page.value = 1; load(); }, 300);
});
watch([companyFilter, roleFilter], () => { page.value = 1; load(); });

function onPage(ev: { page: number; rows: number }) {
  page.value = ev.page + 1;
  pageSize.value = ev.rows;
  load();
}

// ---------- Create / edit dialog ----------
const showFormDialog = ref(false);
const editingId = ref<string | null>(null);
const formBusy = ref(false);
const fForm = ref({
  userName: '',
  fullName: '' as string | null | string,
  email: '',
  password: '',
  companyId: null as number | null,
  role: 'Operator',
});

function openNew() {
  editingId.value = null;
  fForm.value = {
    userName: '',
    fullName: '',
    email: '',
    password: '',
    companyId: companies.value[0]?.id ?? null,
    role: 'Operator',
  };
  showFormDialog.value = true;
}

function openEdit(row: UserListItem) {
  editingId.value = row.id;
  fForm.value = {
    userName: row.userName,
    fullName: row.fullName ?? '',
    email: row.email ?? '',
    password: '',                 // not used in edit mode
    companyId: row.companyId,
    role: row.role,
  };
  showFormDialog.value = true;
}

async function saveForm() {
  formBusy.value = true;
  try {
    if (editingId.value == null) {
      // Create
      await api.post('/users', {
        userName: fForm.value.userName.trim(),
        email: fForm.value.email.trim(),
        password: fForm.value.password,
        fullName: (fForm.value.fullName as string)?.trim() || null,
        companyId: fForm.value.companyId,
        role: fForm.value.role,
      });
      toast.add({ severity: 'success', summary: t('operators.created'), life: 2000 });
    } else {
      // Update (note: userName + role + password are NOT updatable here by design)
      await api.put(`/users/${editingId.value}`, {
        fullName: (fForm.value.fullName as string)?.trim() || null,
        email: fForm.value.email.trim() || null,
        companyId: fForm.value.companyId,
      });
      toast.add({ severity: 'success', summary: t('operators.saved'), life: 2000 });
    }
    showFormDialog.value = false;
    await load();
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
  } finally {
    formBusy.value = false;
  }
}

// ---------- Reset password dialog ----------
const showPwDialog = ref(false);
const pwTargetUser = ref<UserListItem | null>(null);
const pwNew = ref('');
const pwBusy = ref(false);

function openResetPassword(row: UserListItem) {
  pwTargetUser.value = row;
  pwNew.value = '';
  showPwDialog.value = true;
}

async function resetPassword() {
  if (!pwTargetUser.value) return;
  pwBusy.value = true;
  try {
    await api.post(`/users/${pwTargetUser.value.id}/reset-password`, { newPassword: pwNew.value });
    toast.add({ severity: 'success', summary: t('operators.resetPassword.done'), life: 2000 });
    showPwDialog.value = false;
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
  } finally {
    pwBusy.value = false;
  }
}

// ---------- Soft-delete (deactivate) ----------
function deactivate(row: UserListItem) {
  confirm.require({
    message: t('operators.deleteConfirm', { name: row.fullName ?? row.userName }),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`/users/${row.id}`);
        toast.add({ severity: 'success', summary: t('operators.deleted'), life: 2000 });
        load();
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
      }
    },
  });
}

function roleLabel(r: string) {
  return r === 'Administrator' ? t('operators.role.Administrator')
       : r === 'Operator'      ? t('operators.role.Operator')
       : r;
}

const skeletonRows = Array.from({ length: 8 });
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('operators.title') }}</h1>
      <div class="subtitle">{{ t('operators.records', { count: total }) }}</div>
    </div>
    <div class="actions">
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText v-model="q" :placeholder="t('common.search')" size="small" />
      </span>
      <Select
        v-model="companyFilter"
        :options="companyOptions"
        optionLabel="name" optionValue="id"
        :placeholder="t('operators.allCompanies')"
        size="small" class="filter" showClear
      />
      <Select
        v-model="roleFilter"
        :options="roleOptions"
        optionLabel="label" optionValue="value"
        :placeholder="t('operators.allRoles')"
        size="small" class="filter" showClear
      />
      <Button :label="t('operators.new')" icon="pi pi-plus" size="small" severity="success" @click="openNew" />
    </div>
  </div>

  <DataTable
    v-if="loading && items.length === 0"
    :value="skeletonRows" stripedRows size="small"
    class="tight-table"
  >
    <Column :header="t('operators.col.userName')"><template #body><Skeleton /></template></Column>
    <Column :header="t('operators.col.fullName')"><template #body><Skeleton /></template></Column>
    <Column :header="t('operators.col.email')"><template #body><Skeleton /></template></Column>
    <Column :header="t('operators.col.company')"><template #body><Skeleton /></template></Column>
    <Column :header="t('operators.col.role')" style="width:130px"><template #body><Skeleton /></template></Column>
    <Column :header="t('operators.col.actions')" style="width:130px"><template #body><Skeleton /></template></Column>
  </DataTable>

  <DataTable
    v-else
    class="tight-table"
    :value="items"
    :loading="loading"
    stripedRows size="small"
    lazy paginator
    :first="(page - 1) * pageSize"
    :rows="pageSize"
    :totalRecords="total"
    :rowsPerPageOptions="[25, 50, 100, 200]"
    @page="onPage"
    rowHover
    dataKey="id"
  >
    <template #empty>
      <PagedTableEmpty
        icon="pi-users"
        :title="t('operators.title')"
        :hint="q ? t('empty.noResultsFor', { q }) : t('empty.noRows')"
        :ctaLabel="t('operators.new')"
        @cta="openNew"
      />
    </template>

    <Column field="userName" :header="t('operators.col.userName')" />
    <Column field="fullName" :header="t('operators.col.fullName')">
      <template #body="{ data }">{{ data.fullName || '—' }}</template>
    </Column>
    <Column field="email" :header="t('operators.col.email')">
      <template #body="{ data }">{{ data.email || '—' }}</template>
    </Column>
    <Column :header="t('operators.col.company')">
      <template #body="{ data }">
        <span v-if="data.companyName">{{ data.companyName }}</span>
        <span v-else class="muted">—</span>
      </template>
    </Column>
    <Column :header="t('operators.col.role')" style="width: 130px">
      <template #body="{ data }">
        <Tag
          :value="roleLabel(data.role)"
          :severity="data.role === 'Administrator' ? 'warn' : 'info'"
        />
      </template>
    </Column>
    <Column :header="t('operators.col.actions')" style="width: 130px">
      <template #body="{ data }">
        <Button icon="pi pi-pencil"        text rounded severity="secondary" @click="openEdit(data)" v-tooltip.left="t('common.edit')" />
        <Button icon="pi pi-key"           text rounded severity="warn"      @click="openResetPassword(data)" v-tooltip.left="t('operators.resetPassword.title')" />
        <Button icon="pi pi-user-minus"    text rounded severity="danger"    @click="deactivate(data)" v-tooltip.left="t('common.delete')" />
      </template>
    </Column>
  </DataTable>

  <!-- ===== Create / Edit dialog ===== -->
  <Dialog
    v-model:visible="showFormDialog"
    modal
    :header="editingId ? t('operators.form.editTitle') : t('operators.form.newTitle')"
    style="width: 480px"
  >
    <div class="form-grid">
      <div class="field full">
        <label>
          {{ t('operators.form.userName') }} *
          <i v-if="editingId" class="pi pi-lock" style="font-size: 0.7rem; margin-left: 0.25rem; opacity: 0.6" />
        </label>
        <InputText v-model="fForm.userName" :disabled="!!editingId" maxlength="100" />
        <span v-if="editingId" class="muted" style="font-size: 0.7rem">{{ t('operators.form.userNameLocked') }}</span>
      </div>
      <div class="field full">
        <label>{{ t('operators.form.fullName') }}</label>
        <InputText v-model="fForm.fullName" maxlength="200" />
      </div>
      <div class="field full">
        <label>{{ t('operators.form.email') }} *</label>
        <InputText v-model="fForm.email" maxlength="200" />
      </div>
      <div v-if="!editingId" class="field full">
        <label>{{ t('operators.form.password') }} *</label>
        <Password v-model="fForm.password" :feedback="false" toggleMask input-class="w-full" />
        <span class="muted" style="font-size: 0.7rem">{{ t('operators.form.passwordHint') }}</span>
      </div>
      <div class="field full">
        <label>{{ t('operators.form.company') }}</label>
        <Select
          v-model="fForm.companyId"
          :options="companies"
          optionLabel="name" optionValue="id"
          placeholder="—" filter showClear
        />
      </div>
      <div v-if="!editingId" class="field full">
        <label>{{ t('operators.form.role') }}</label>
        <Select
          v-model="fForm.role"
          :options="[{ value: 'Operator', label: t('operators.role.Operator') }, { value: 'Administrator', label: t('operators.role.Administrator') }]"
          optionLabel="label" optionValue="value"
        />
      </div>
    </div>
    <template #footer>
      <Button :label="t('common.cancel')" text severity="secondary" @click="showFormDialog = false" />
      <Button :label="t('common.save')" icon="pi pi-check" :loading="formBusy" @click="saveForm" />
    </template>
  </Dialog>

  <!-- ===== Reset password dialog ===== -->
  <Dialog
    v-model:visible="showPwDialog"
    modal
    :header="t('operators.resetPassword.title')"
    style="width: 380px"
  >
    <div class="form-grid">
      <div class="field full">
        <span class="muted" style="font-size: 0.8rem">
          {{ pwTargetUser?.fullName ?? pwTargetUser?.userName }}
        </span>
      </div>
      <div class="field full">
        <label>{{ t('operators.resetPassword.newPassword') }}</label>
        <Password v-model="pwNew" :feedback="false" toggleMask input-class="w-full" />
        <span class="muted" style="font-size: 0.7rem">{{ t('operators.form.passwordHint') }}</span>
      </div>
    </div>
    <template #footer>
      <Button :label="t('common.cancel')" text severity="secondary" @click="showPwDialog = false" />
      <Button :label="t('common.save')" icon="pi pi-key" severity="warn" :loading="pwBusy" @click="resetPassword" />
    </template>
  </Dialog>
</template>

<style scoped>
.search-wrap { position: relative; display: inline-flex; align-items: center; }
.search-wrap i {
  position: absolute; left: 0.5rem;
  color: var(--color-text-muted); pointer-events: none; font-size: 0.75rem;
}
.search-wrap :deep(input) { padding-left: 1.625rem; min-width: 240px; font-size: 0.8125rem; }
.filter { min-width: 150px; }
.filter :deep(.p-select-label) { font-size: 0.8125rem; }

.tight-table :deep(.p-datatable-tbody td),
.tight-table :deep(.p-datatable-thead th) {
  padding: 0.2rem 0.5rem;
  font-size: 0.8125rem;
  line-height: 1.2;
}
.tight-table :deep(.p-datatable-tbody td) { height: 28px; }
.tight-table :deep(.p-button.p-button-icon-only) { width: 1.75rem; height: 1.75rem; }
:deep(.p-password) { width: 100%; }
:deep(.p-password-input) { width: 100%; }
</style>
