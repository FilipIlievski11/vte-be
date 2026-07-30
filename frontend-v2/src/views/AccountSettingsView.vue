<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { api } from '@/api/client';
import { useAuthStore } from '@/stores/auth';
import type { MeResponse, ProfileUpdate, ChangePasswordRequest } from '@/types';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Password from 'primevue/password';
import { useToast } from 'primevue/usetoast';

const { t } = useI18n();
const toast = useToast();
const auth = useAuthStore();

const loading = ref(true);
const savingProfile = ref(false);
const savingPassword = ref(false);

// Read-only identity (not editable here)
const userName = ref('');
const roleText = ref('');
const companyName = ref<string | null>(null);

// Editable profile fields — kept as plain strings to play nicely with InputText
const fullName = ref('');
const email = ref('');

// Password fields — no current-password challenge (the JWT proves identity)
const newPassword = ref('');
const confirmNewPassword = ref('');

async function loadMe() {
  loading.value = true;
  try {
    const { data } = await api.get<MeResponse>('/auth/me');
    userName.value = data.userName;
    roleText.value = data.roles.join(', ');
    companyName.value = data.companyName;
    fullName.value = data.fullName ?? '';
    email.value = data.email ?? '';
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('account.loadFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  } finally {
    loading.value = false;
  }
}

async function saveProfile() {
  const body: ProfileUpdate = {
    fullName: fullName.value.trim() || null,
    email: email.value.trim() || null,
  };
  savingProfile.value = true;
  try {
    const { data } = await api.put<MeResponse>('/auth/profile', body);
    fullName.value = data.fullName ?? '';
    email.value = data.email ?? '';
    auth.updateFullName(data.fullName);            // keep topbar in sync
    toast.add({ severity: 'success', summary: t('account.profileSaved'), life: 1800 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
  } finally {
    savingProfile.value = false;
  }
}

async function changePassword() {
  if (!newPassword.value || newPassword.value.length < 6) {
    toast.add({ severity: 'warn', summary: t('account.password.tooShort'), life: 3000 });
    return;
  }
  if (newPassword.value !== confirmNewPassword.value) {
    toast.add({ severity: 'warn', summary: t('account.password.mismatch'), life: 3000 });
    return;
  }
  const body: ChangePasswordRequest = { newPassword: newPassword.value };
  savingPassword.value = true;
  try {
    await api.post('/auth/change-password', body);
    newPassword.value = '';
    confirmNewPassword.value = '';
    toast.add({ severity: 'success', summary: t('account.password.changed'), life: 1800 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
  } finally {
    savingPassword.value = false;
  }
}

onMounted(loadMe);
</script>

<template>
  <div class="account-page">
  <div class="page-header">
    <div>
      <h1>{{ t('account.title') }}</h1>
      <div class="subtitle">{{ t('account.subtitle') }}</div>
    </div>
  </div>

  <div v-if="loading" class="muted pad">{{ t('common.loading') }}…</div>

  <div v-else class="cards-grid">
    <div class="card">
      <div class="card-header">{{ t('account.profile.title') }}</div>
      <div class="card-body">
        <div class="grid two-col">
          <div class="field">
            <label>{{ t('account.profile.userName') }}</label>
            <div class="val readonly mono">{{ userName }}</div>
          </div>
          <div class="field">
            <label>{{ t('account.profile.role') }}</label>
            <div class="val readonly">{{ roleText || '—' }}</div>
          </div>
          <div v-if="companyName" class="field span-full">
            <label>{{ t('account.profile.company') }}</label>
            <div class="val readonly">{{ companyName }}</div>
          </div>
          <div class="field">
            <label>{{ t('account.profile.fullName') }}</label>
            <InputText v-model="fullName" :placeholder="t('account.profile.fullNamePh')" />
          </div>
          <div class="field">
            <label>{{ t('account.profile.email') }}</label>
            <InputText v-model="email" type="email" placeholder="name@example.com" />
          </div>
        </div>
        <div class="card-actions">
          <Button :label="t('common.save')" icon="pi pi-check" size="small" :loading="savingProfile" @click="saveProfile" />
        </div>
      </div>
    </div>

    <div class="card">
      <div class="card-header">{{ t('account.password.title') }}</div>
      <div class="card-body">
        <p class="muted small">{{ t('account.password.hint') }}</p>
        <div class="grid two-col">
          <div class="field">
            <label>{{ t('account.password.new') }} *</label>
            <Password
              v-model="newPassword"
              toggleMask
              autocomplete="new-password" input-class="w-full"
            />
          </div>
          <div class="field">
            <label>{{ t('account.password.confirm') }} *</label>
            <Password
              v-model="confirmNewPassword"
              :feedback="false" toggleMask
              autocomplete="new-password" input-class="w-full"
            />
          </div>
        </div>
        <div class="card-actions">
          <Button :label="t('account.password.change')" icon="pi pi-key" size="small" :loading="savingPassword" @click="changePassword" />
        </div>
      </div>
    </div>
  </div>
  </div>
</template>

<style scoped>
/* Same v1-style cards as the other forms (accent stripe, dense inputs). */
.account-page { max-width: 760px; margin: 0 auto; }
.account-page :deep(.card .card-header) {
  padding: 0.45rem 0.875rem; font-size: 0.8125rem; font-weight: 600;
  position: relative;
  background: linear-gradient(180deg, color-mix(in srgb, var(--color-surface) 92%, var(--color-brand-500)) 0%, var(--color-surface) 100%);
}
.account-page :deep(.card .card-header)::before {
  content: '';
  position: absolute; left: 0; top: 0; bottom: 0;
  width: 3px; background: var(--color-brand-500); border-radius: 0 2px 2px 0;
}
.account-page :deep(.card .card-body) { padding: 0.625rem 0.875rem; }
.account-page :deep(.p-inputtext) { width: 100%; padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.account-page :deep(.p-inputtext:focus) {
  outline: none;
  border-color: var(--color-brand-500);
  box-shadow: 0 0 0 2px color-mix(in srgb, var(--color-brand-500) 25%, transparent);
}

.cards-grid { display: grid; grid-template-columns: 1fr; gap: .5rem }
.card { width: 100% }
.grid.two-col { display: grid; grid-template-columns: 1fr 1fr; gap: .625rem }
.field { display: flex; flex-direction: column; gap: .15rem; margin-bottom: .45rem }
.field.span-full { grid-column: 1 / -1 }
.field label { font-weight: 500; font-size: .75rem; color: var(--color-text-secondary) }
.val.readonly { padding: .45rem .25rem; font-size: .9rem }
.mono { font-family: monospace }
.muted { color: var(--p-text-muted-color) }
.small { font-size: .8rem; margin: 0 0 .5rem }
.pad { padding: 1rem }
.subtitle { color: var(--p-text-muted-color); font-size: .85rem }
.card-actions { display: flex; justify-content: flex-end; margin-top: .25rem }
.field :deep(.p-password) { width: 100% }
.field :deep(.p-password input) { width: 100% }
</style>
