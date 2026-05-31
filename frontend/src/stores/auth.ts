import { defineStore } from 'pinia'
import axios from 'axios'

interface LoginResponse {
  token: string
  userName: string
  roles: string[]
  stationId: number | null
}

const STORAGE_KEY = 'vte.auth'

interface PersistedAuth {
  token: string
  userName: string
  roles: string[]
  stationId: number | null
}

function loadPersisted(): PersistedAuth | null {
  try {
    const raw = localStorage.getItem(STORAGE_KEY)
    return raw ? (JSON.parse(raw) as PersistedAuth) : null
  } catch {
    return null
  }
}

export const useAuthStore = defineStore('auth', {
  state: () => {
    const p = loadPersisted()
    return {
      token: p?.token ?? null as string | null,
      userName: p?.userName ?? '' as string,
      roles: p?.roles ?? [] as string[],
      stationId: p?.stationId ?? null as number | null,
    }
  },

  getters: {
    isAuthenticated: (s) => !!s.token,
    isAdministrator: (s) => s.roles.includes('Administrator'),
    isOperator: (s) => s.roles.includes('Operator'),
  },

  actions: {
    async login(userName: string, password: string) {
      // Direct axios call (not the interceptor-wired client) to avoid circular deps during boot
      const { data } = await axios.post<LoginResponse>('/api/auth/login', { userName, password })
      this.token = data.token
      this.userName = data.userName
      this.roles = data.roles
      this.stationId = data.stationId
      localStorage.setItem(
        STORAGE_KEY,
        JSON.stringify({ token: this.token, userName: this.userName, roles: this.roles, stationId: this.stationId }),
      )
    },

    logout() {
      this.token = null
      this.userName = ''
      this.roles = []
      this.stationId = null
      localStorage.removeItem(STORAGE_KEY)
    },
  },
})
