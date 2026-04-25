import { defineStore } from "pinia";
import { store } from "../utils";

export const useMapStore = defineStore({
  id: "pure-user",
  state: () => ({
    count: 1001,
    isEnable: true
  }),
  actions: {
    SET_ISENSBLE(isEnable: boolean) {
      this.isEnable = isEnable;
      this.count++;
    }
  },
  getters: {
    GET_ISENSBLE() {
      return this.isEnable;
    },
    GET_COUNT() {
      return this.count;
    }
  }
});

export function useMapStoreHook() {
  return useMapStore(store);
}
