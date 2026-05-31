import { defineStore } from 'pinia';
import type { LoginResponse } from '@/types';

const STORAGE_KEY = 'vte.v2.auth';

interface PersistedAuth {
  token: string;
  expiresAt: string;
  userId: string;
  userName: string;
  fullName: string | null;
  companyId: number | null;
  companyName: string | null;
  roles: string[];
}

function loadFromStorage(): PersistedAuth | null {
  const raw = localStorage.getItem(STORAGE_KEY);
  if (!raw) return null;
  try {
    const parsed = JSON.parse(raw) as PersistedAuth;
    if (new Date(parsed.expiresAt).getTime() < Date.now()) {
      localStorage.removeItem(STORAGE_KEY);
      return null;
    }
    return parsed;
  } catch {
    return null;
  }
}

export const useAuthStore = defineStore('auth', {
  state: () => {
    const persisted = loadFromStorage();
    return {
      token: persisted?.token ?? null as string | null,
      expiresAt: persisted?.expiresAt ?? null as string | null,
      userId: persisted?.userId ?? null as string | null,
      userName: persisted?.userName ?? null as string | null,
      fullName: persisted?.fullName ?? null as string | null,
      companyId: persisted?.companyId ?? null as number | null,
      companyName: persisted?.companyName ?? null as string | null,
      roles: (persisted?.roles ?? []) as string[],
    };
  },
  getters: {
    isAuthenticated: (s) => !!s.token,
    isAdmin: (s) => s.roles.includes('Administrator'),
    isOperator: (s) => s.roles.includes('Operator'),
  },
  actions: {
    setSession(r: LoginResponse) {
      this.token = r.token;
      this.expiresAt = r.expiresAt;
      this.userId = r.userId;
      this.userName = r.userName;
      this.fullName = r.fullName;
      this.companyId = r.companyId;
      this.companyName = r.companyName;
      this.roles = r.roles;
      const persisted: PersistedAuth = {
        token: r.token,
        expiresAt: r.expiresAt,
        userId: r.userId,
        userName: r.userName,
        fullName: r.fullName,
        companyId: r.companyId,
        companyName: r.companyName,
        roles: r.roles,
      };
      localStorage.setItem(STORAGE_KEY, JSON.stringify(persisted));
    },
    /** Reflect a self-service profile update — keeps the topbar in sync after AccountSettings save. */
    updateFullName(name: string | null) {
      this.fullName = name;
      const raw = localStorage.getItem(STORAGE_KEY);
      if (raw) {
        try {
          const p = JSON.parse(raw) as PersistedAuth;
          p.fullName = name;
          localStorage.setItem(STORAGE_KEY, JSON.stringify(p));
        } catch { /* ignore */ }
      }
    },
    clear() {
      this.token = null;
      this.expiresAt = null;
      this.userId = null;
      this.userName = null;
      this.fullName = null;
      this.companyId = null;
      this.companyName = null;
      this.roles = [];
      localStorage.removeItem(STORAGE_KEY);
    },
  },
});
