import { createI18n } from 'vue-i18n';
import mk from './mk';
import en from './en';

const STORAGE_KEY = 'vte.v2.locale';
const supported = ['mk', 'en'] as const;
export type Locale = (typeof supported)[number];

function detect(): Locale {
  const stored = localStorage.getItem(STORAGE_KEY) as Locale | null;
  if (stored && supported.includes(stored)) return stored;
  return 'mk';   // MK is the default per the original audit §3.1
}

export const i18n = createI18n({
  legacy: false,
  locale: detect(),
  fallbackLocale: 'en',
  messages: { mk, en },
});

export function setLocale(loc: Locale) {
  i18n.global.locale.value = loc;
  localStorage.setItem(STORAGE_KEY, loc);
}
