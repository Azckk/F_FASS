import { type Ref, h, ref, reactive, onMounted } from "vue";
import { message } from "@/utils/message";
import { getData, setData } from "@/api/setting/configdata";
import { type FormItemProps } from "../utils/types";

export function useHook() {
  const form = reactive({
    userLogDayLimit: "",
    auditLogDayLimit: "",
    diaryDayLimit: "",
    alarmDayLimit: "",
    carInstantActionDayLimit: "",
    taskInstanceDayLimit: "",
    workDayLimit: "",
    carMinBattery: "",
    carMaxBattery: "",
    carIdleSecond: ""
  });
  const formRef = ref();
  const loading = ref(true);

  async function handleGetData() {
    loading.value = true;
    const { data } = await getData();
    form.userLogDayLimit = data?.userLogDayLimit ?? null;
    form.auditLogDayLimit = data?.auditLogDayLimit ?? null;
    form.diaryDayLimit = data?.diaryDayLimit ?? null;
    form.alarmDayLimit = data?.alarmDayLimit ?? null;
    form.carInstantActionDayLimit = data?.carInstantActionDayLimit ?? null;
    form.taskInstanceDayLimit = data?.taskInstanceDayLimit ?? null;
    form.workDayLimit = data?.workDayLimit ?? null;
    form.carMinBattery = data?.carMinBattery ?? null;
    form.carMaxBattery = data?.carMaxBattery ?? null;
    form.carIdleSecond = data?.carIdleSecond ?? null;
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
