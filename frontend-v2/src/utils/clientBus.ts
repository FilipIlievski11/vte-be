// Крос-таб канал за „Нов" клиент: копчето до пикерот ја отвора клиент-формата во
// ново јазиче, а формата-повикувач слуша тука — штом клиентот се сними, пикерот во
// старото јазиче се пополнува автоматски. BroadcastChannel работи само меѓу јазичиња
// од ист origin; ако browser-от не го поддржува, тивко нема автопополнување и
// операторот го пребарува клиентот рачно (истиот тек како досега).
import type { Client } from '@/types';

const CHANNEL = 'vte.client-created';

export function announceClientCreated(client: Client): void {
  try {
    const bc = new BroadcastChannel(CHANNEL);
    bc.postMessage(client);
    bc.close();
  } catch { /* нема поддршка — рачно пребарување */ }
}

/** Слушај за новосоздадени клиенти. Врати ја функцијата за одјава (повикај ја во onUnmounted). */
export function onClientCreated(handler: (client: Client) => void): () => void {
  try {
    const bc = new BroadcastChannel(CHANNEL);
    bc.onmessage = (e: MessageEvent) => handler(e.data as Client);
    return () => bc.close();
  } catch {
    return () => {};
  }
}
