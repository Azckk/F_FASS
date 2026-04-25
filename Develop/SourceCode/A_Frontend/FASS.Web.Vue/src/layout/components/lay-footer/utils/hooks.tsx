import { type Ref, onMounted, ref } from "vue";

import {
  connect,
  disconnect,
  setupSignalRHandlers,
  sendAlarm
} from "@/api/useMessageHubWebSocket";
// import { useMapStore } from "@/store/modules/map";

export function useHook(tableRef?: Ref | undefined) {
  const info = ref("");
  async function handleGlobalWarning() {
    await connect();
    await sendAlarm();
    await setupSignalRHandlers(data => {
      info.value = data;
    });
  }
  setInterval(async () => {
    await sendAlarm();
  }, 1000);

  onMounted(async () => {});

  return { info, handleGlobalWarning, disconnect };
}
