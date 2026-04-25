import { ref, onMounted } from "vue";
import type { FormInline } from "../utils/types";

import { GetListToSelect } from "@/api/warehouse/visualization";

export function useHook(formInline) {
  const tagData = ref([]);
  const selectedItemId = ref();
  async function getListToselectFn() {
    const { data } = await GetListToSelect();
    tagData.value = data;
    const matchedItem = tagData.value.find(item => item.id === formInline.id);
    if (matchedItem) {
      selectedItemId.value = matchedItem.id;
    }
  }
  const newFormInline = ref();
  async function addTagFn(e: FormInline, type: string) {
    newFormInline.value = e;
    newFormInline.value.keyValue = formInline.keyValue;
    newFormInline.value.type = type;
    selectedItemId.value = e.id;
  }
  async function TagDeleteFn(e: FormInline, type: string) {
    newFormInline.value = e;
    newFormInline.value.keyValue = formInline.keyValue;
    newFormInline.value.type = type;
    selectedItemId.value = "";
  }
  onMounted(async () => {
    getListToselectFn();
  });

  return {
    tagData,
    selectedItemId,
    newFormInline,
    addTagFn,
    TagDeleteFn
  };
}
