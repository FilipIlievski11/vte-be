import 'vue-router'
declare module 'vue-router' {
  interface RouteMeta {
    anonymous?: boolean
    roles?: string[]
  }
}
