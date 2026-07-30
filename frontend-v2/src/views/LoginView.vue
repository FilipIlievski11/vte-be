<script setup lang="ts">
import { ref } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { useI18n } from 'vue-i18n';
import { useAuthStore } from '@/stores/auth';
import { api } from '@/api/client';
import type { LoginResponse } from '@/types';
import InputText from 'primevue/inputtext';
import Password from 'primevue/password';
import Button from 'primevue/button';
import Checkbox from 'primevue/checkbox';

const { t } = useI18n();
const router = useRouter();
const route = useRoute();
const auth = useAuthStore();

// Секој оператор има своја сметка — полето го памети последниот најавен
// корисник на овој компјутер наместо да пред-пополнува „admin".
const LAST_USER_KEY = 'vte.v2.lastUser';
const userName = ref(localStorage.getItem(LAST_USER_KEY) ?? '');
const password = ref('');
const useCookie = ref(false);
const loading = ref(false);
const error = ref<string | null>(null);

async function submit() {
  error.value = null;
  loading.value = true;
  try {
    const { data } = await api.post<LoginResponse>('/auth/login', {
      userName: userName.value,
      password: password.value,
      useCookie: useCookie.value,
    });
    localStorage.setItem(LAST_USER_KEY, userName.value.trim());
    auth.setSession(data);
    const redirect = (route.query.redirect as string) || '/dashboard';
    router.push(redirect);
  } catch (e: any) {
    error.value = e?.response?.data?.error || t('login.failed');
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <div class="login-shell">
    <aside class="login-hero">
      <div class="brand">
        <div class="brand-mark"><i class="pi pi-car" /></div>
      </div>
      <div>
        <h2>{{ t('login.heroTitle') }}</h2>
        <p>{{ t('login.heroSubtitle') }}</p>
      </div>
      <footer>© {{ new Date().getFullYear() }} {{ t('login.heroFooter') }}</footer>
    </aside>

    <div class="login-form-pane">
      <form class="login-card" @submit.prevent="submit">
        <h1>{{ t('login.welcomeBack') }}</h1>
        <p class="subtitle">{{ t('login.subtitle') }}</p>

        <div class="field">
          <label>{{ t('login.userName') }}</label>
          <InputText v-model="userName" autocomplete="username" required :disabled="loading" />
        </div>
        <div class="field">
          <label>{{ t('login.password') }}</label>
          <Password
            v-model="password"
            :feedback="false"
            toggleMask
            autocomplete="current-password"
            required
            input-class="w-full"
            :disabled="loading"
          />
        </div>

        <div class="field" style="margin-bottom: 1rem; flex-direction: row; align-items: center; gap: 0.5rem">
          <Checkbox v-model="useCookie" :binary="true" inputId="cookie" :disabled="loading" />
          <label for="cookie" style="margin: 0; cursor: pointer; user-select: none">{{ t('login.useCookie') }}</label>
        </div>

        <Button
          type="submit"
          :loading="loading"
          :label="t('login.signIn')"
          icon="pi pi-sign-in"
          iconPos="right"
          style="width: 100%"
        />
        <div v-if="error" class="error" style="margin-top: 0.875rem">{{ error }}</div>
      </form>
    </div>
  </div>
</template>
