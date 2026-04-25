// src/store/formState.ts
import { defineStore } from "pinia";

export const useFormStateStore = defineStore("formState", {
  state: () => ({
    callId: "",
    srcStorageId: "",
    destStorageId: "",
    siteStatus: 0
  }),
  actions: {
    setCallId(id: string) {
      this.callId = id;
    },
    setSrcStorageId(id: string) {
      this.srcStorageId = id;
    },
    setDestStorageId(id: string) {
      this.destStorageId = id;
    },
    setSiteStatus(sys: number) {
      this.siteStatus = sys;
    }
  }
});
