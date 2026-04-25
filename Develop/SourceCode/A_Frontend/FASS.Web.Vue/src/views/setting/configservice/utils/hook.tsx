import { type Ref, h, ref, reactive, onMounted } from "vue";
import { message } from "@/utils/message";
import { getData, setData } from "@/api/setting/configservice";
import { type FormItemProps } from "../utils/types";

export function useHook() {
  const form = reactive({
    item1: "",
    item2: "",
    item3: ""
  });
  const formRef = ref();
  const loading = ref(true);

  async function handleGetData() {
    loading.value = true;
    const { data } = await getData();
    form.item1 = data?.item1 ?? null;
    form.item2 = data?.item2 ?? null;
    form.item3 = data?.item3 ?? null;
    loading.value = false;
  }

  function handleSetData() {
    const formData = form as FormItemProps;
    formRef.value.validate(async valid => {
      if (valid) {
        await setData(formData);
        message("操作成功！", { type: "success" });
      }
    });
  }

  onMounted(() => {
    handleGetData();
  });

  return {
    form,
    formRef,
    loading,
    handleGetData,
    handleSetData
  };
}
