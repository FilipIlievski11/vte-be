<script setup lang="ts">
// Целосните клиент-полиња од легаси „Податоци за сопственикот / овластено лице"
// + трите документи (л.к. / пасош / возачка). Се вградува двапати во Полномошно
// формата; мутира го подадениот модел директно (записот се снима назад со PUT).
import { useI18n } from 'vue-i18n';
import type { Citizenship, City, Client, DocumentIssuer } from '@/types';
import InputText from 'primevue/inputtext';
import Checkbox from 'primevue/checkbox';
import Select from 'primevue/select';
import DatePicker from 'primevue/datepicker';

export interface DocForm { id: number; number: string; dateIssued: Date | null; expiresAt: Date | null; issuerName: string; }
export interface ClientDocs { idCard: DocForm; passport: DocForm; licence: DocForm; }

defineProps<{
  c: Partial<Client>;
  docs: ClientDocs;
  cities: City[];
  citizenships: Citizenship[];
  documentIssuers: DocumentIssuer[];
}>();

const { t } = useI18n();
const tp = (k: string) => t(`vehiclePermissions.form.${k}`);
</script>

<template>
  <!-- идентитет -->
  <div class="row checks">
    <label class="chk"><Checkbox v-model="c.business" :binary="true" /> {{ tp('clientBusiness') }}</label>
    <label class="chk"><Checkbox v-model="c.notificationsAllowed" :binary="true" /> {{ tp('clientNotifications') }}</label>
  </div>
  <div class="row">
    <div class="field"><label>{{ tp('clientMb') }}</label><InputText v-model="c.mb" maxlength="13" /></div>
    <div class="field"><label>{{ tp('clientEdb') }}</label><InputText v-model="c.taxNumber" maxlength="50" /></div>
    <div class="field"><label>{{ c.business ? tp('clientFoundedDate') : tp('clientDateOfBirth') }}</label>
      <DatePicker :modelValue="c.dateOfBirth ? new Date(c.dateOfBirth) : null"
                  @update:modelValue="(v: any) => (c.dateOfBirth = v ? v.toISOString() : null)"
                  dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
    </div>
    <div class="field"><label>{{ tp('clientBirthCity') }}</label>
      <Select v-model="c.birthCityId" :options="cities" optionLabel="name" optionValue="id" placeholder="—" showClear filter /></div>
  </div>
  <div class="row">
    <div class="field"><label>{{ c.business ? tp('clientFirmName') : tp('clientFirstName') }}</label><InputText v-model="c.firstName" maxlength="100" /></div>
    <div class="field" v-if="!c.business"><label>{{ tp('clientParentName') }}</label><InputText v-model="c.parentName" maxlength="100" /></div>
    <div class="field" v-if="!c.business"><label>{{ tp('clientLastName') }}</label><InputText v-model="c.lastName" maxlength="100" /></div>
  </div>
  <div class="row">
    <div class="field"><label>{{ tp('clientCitizenship') }}</label>
      <Select v-model="c.citizenshipId" :options="citizenships" optionLabel="name" optionValue="id" placeholder="—" showClear filter /></div>
    <div class="field"><label>{{ tp('clientCity') }}</label>
      <Select v-model="c.cityId" :options="cities" optionLabel="name" optionValue="id" placeholder="—" showClear filter /></div>
    <div class="field"><label>{{ tp('clientAddress') }}</label><InputText v-model="c.address" maxlength="100" /></div>
  </div>
  <div class="row">
    <div class="field"><label>{{ tp('clientProfession') }}</label><InputText v-model="c.profession" maxlength="150" /></div>
    <div class="field"><label>{{ tp('clientEmployer') }}</label><InputText v-model="c.employer" maxlength="200" /></div>
  </div>
  <!-- контакт -->
  <div class="row">
    <div class="field"><label>{{ tp('clientPhone') }}</label><InputText v-model="c.phoneNumber" maxlength="20" /></div>
    <div class="field"><label>{{ tp('clientFax') }}</label><InputText v-model="c.fax" maxlength="50" /></div>
    <div class="field"><label>{{ tp('clientEmail') }}</label><InputText v-model="c.email" maxlength="100" /></div>
  </div>

  <!-- документи -->
  <div class="docgrid">
    <div class="docblock">
      <h3>{{ tp('docIdCard') }}</h3>
      <div class="field"><label>{{ tp('docNumber') }}</label><InputText v-model="docs.idCard.number" maxlength="100" /></div>
      <div class="field"><label>{{ tp('docDateIssued') }}</label><DatePicker v-model="docs.idCard.dateIssued" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
      <div class="field"><label>{{ tp('docExpires') }}</label><DatePicker v-model="docs.idCard.expiresAt" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
      <div class="field"><label>{{ tp('docIssuer') }}</label>
        <Select v-model="docs.idCard.issuerName" :options="documentIssuers.map(i => i.name).filter(Boolean)" editable placeholder="—" /></div>
    </div>
    <div class="docblock">
      <h3>{{ tp('docPassport') }}</h3>
      <div class="field"><label>{{ tp('docNumber') }}</label><InputText v-model="docs.passport.number" maxlength="100" /></div>
      <div class="field"><label>{{ tp('docDateIssued') }}</label><DatePicker v-model="docs.passport.dateIssued" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
      <div class="field"><label>{{ tp('docExpires') }}</label><DatePicker v-model="docs.passport.expiresAt" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
      <div class="field"><label>{{ tp('docIssuer') }}</label>
        <Select v-model="docs.passport.issuerName" :options="documentIssuers.map(i => i.name).filter(Boolean)" editable placeholder="—" /></div>
    </div>
    <div class="docblock">
      <h3>{{ tp('docLicence') }}</h3>
      <div class="field"><label>{{ tp('docNumber') }}</label><InputText v-model="docs.licence.number" maxlength="100" /></div>
      <div class="field"><label>{{ tp('docDateIssued') }}</label><DatePicker v-model="docs.licence.dateIssued" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
      <div class="field"><label>{{ tp('docExpires') }}</label><DatePicker v-model="docs.licence.expiresAt" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
      <div class="field"><label>{{ tp('docIssuer') }}</label>
        <Select v-model="docs.licence.issuerName" :options="documentIssuers.map(i => i.name).filter(Boolean)" editable placeholder="—" /></div>
    </div>
  </div>
</template>

<style scoped>
.row.checks { display: flex; flex-wrap: wrap; gap: 0.4rem 1.5rem; margin-bottom: 0.45rem; }
.chk { display: flex; align-items: center; gap: 0.45rem; font-size: 0.8125rem; color: var(--color-text); cursor: pointer; }
.docgrid { display: grid; grid-template-columns: repeat(auto-fit, minmax(240px, 1fr)); gap: 0.5rem 0.875rem; margin-top: 0.35rem; }
.docblock h3 {
  font-size: 0.75rem; color: var(--color-text);
  margin: 0 0 0.4rem; padding-bottom: 0.25rem;
  border-bottom: 1px solid var(--color-border); font-weight: 600;
}
.docblock .field { margin-bottom: 0.4rem; }
</style>
