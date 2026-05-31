<script setup lang="ts">
import { useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { computed } from 'vue'

const { t } = useI18n()
const route = useRoute()
const titleKey = computed(() => (route.meta?.titleKey as string) || '')
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ titleKey ? t(titleKey) : route.path }}</h1>
      <div class="subtitle">{{ t('common.comingSoon') }}</div>
    </div>
  </div>

  <div class="card">
    <div class="card-body" style="text-align: center; padding: 3rem">
      <i class="pi pi-wrench" style="font-size: 2.5rem; color: var(--color-text-muted); display: block; margin-bottom: 1rem" />
      <h2 style="margin-bottom: 0.5rem">{{ t('common.notReadyYet') }}</h2>
      <p style="color: var(--color-text-muted); max-width: 500px; margin: 0 auto">
        {{ t('common.notReadyHint') }}
      </p>
    </div>
  </div>
</template>
