<script setup lang="ts">
import { ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useI18n } from 'vue-i18n'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'

const { t } = useI18n()
const auth = useAuthStore()
const router = useRouter()
const route = useRoute()

const userName = ref('admin')
const password = ref('')
const error = ref<string | null>(null)
const loading = ref(false)

async function submit() {
  error.value = null; loading.value = true
  try {
    await auth.login(userName.value, password.value)
    const redirect = (route.query.redirect as string) || '/'
    router.push(redirect)
  } catch {
    error.value = t('login.error')
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="login-shell">
    <aside class="login-hero">
      <div class="brand">
        <div class="brand-mark">VTE</div>
        <strong style="font-size: 1.125rem">VTE</strong>
      </div>
      <div>
        <h2>{{ t('login.heroTitle') }}</h2>
        <p>{{ t('login.heroSubtitle') }}</p>
      </div>
      <footer>{{ t('login.heroFooter') }}</footer>
    </aside>

    <div class="login-form-pane">
      <form class="login-card" @submit.prevent="submit">
        <h1>{{ t('login.welcomeBack') }}</h1>
        <p class="subtitle">{{ t('login.subtitle') }}</p>

        <div class="field">
          <label>{{ t('login.userName') }}</label>
          <InputText v-model="userName" autocomplete="username" required />
        </div>
        <div class="field">
          <label>{{ t('login.password') }}</label>
          <Password v-model="password" :feedback="false" toggleMask autocomplete="current-password" required input-class="w-full" />
        </div>
        <Button type="submit" :loading="loading" :label="t('login.submit')" icon="pi pi-sign-in" iconPos="right" class="w-full" style="width: 100%" />
        <div v-if="error" class="error" style="margin-top: 0.875rem">{{ error }}</div>
      </form>
    </div>
  </div>
</template>
