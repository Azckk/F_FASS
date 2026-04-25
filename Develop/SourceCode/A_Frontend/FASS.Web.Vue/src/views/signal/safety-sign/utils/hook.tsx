import { ref, onMounted, onUnmounted } from "vue";
import { deviceDetection } from "@pureadmin/utils";
import { useMyI18n } from "@/plugins/i18n";
import { getData, StorageGetPage } from "@/api/signal/safetysign";
import { GetDictItemInLocal } from "@/utils/auth";
import type { TabsPaneContext } from "element-plus";

export function useHook() {
  const { t } = useMyI18n();
  const loading = ref(true);
  const dataList = ref({});
  const SignalDict = ref([]);
  const selection = ref([]);
  const activeName = ref("One");
  const activeCode = ref("1");
  const tagList = ref([]);
  let timer = null;
  let timerInitialized = false;
  const handleClick = async (tab?: TabsPaneContext, event?: Event) => {
    if (!timerInitialized) {
      loading.value = true;
    }
    const { data } = await getData({
      storageName: tab?.props.name || activeName.value,
      storageCode: tab?.props.label || activeCode.value
    });
    activeCode.value = tab?.props.label || activeCode.value;
    dataList.value = generateSignalData(JSON.parse(data), SignalDict.value);
    if (!timerInitialized) {
      loading.value = false;
      timerInitialized = true;
    }
  };

  function generateSignalData(data: Record<string, boolean>, dictData: any[]) {
    const signalData: Record<
      string,
      { data: { code: string; name: string; value: boolean }[] }
    > = {};

    dictData.forEach(dictItem => {
      const [prefix, key] = dictItem.code.split("_");
      const lowerCaseKey = key.toLowerCase();
      const lowerCaseDataKeys = Object.fromEntries(
        Object.entries(data).map(([k, v]) => [k.toLowerCase(), v])
      );
      const value = lowerCaseDataKeys[lowerCaseKey];
      if (!signalData[prefix]) {
        signalData[prefix] = { data: [] };
      }
      if (value !== undefined) {
        signalData[prefix].data.push({
          code: dictItem.code,
          name: dictItem.name,
          value: value
        });
      }
    });

    return signalData;
  }

  async function GetSignalDictList() {
    try {
      const data = await GetDictItemInLocal("Signal");
      SignalDict.value = [...data];
      // console.log("SignalDict", SignalDict);
      handleClick({
        props: {
          name: activeName.value,
          label: activeCode.value
        }
      });
    } catch (error) {
      console.error("Error:", error);
    }
  }

  const StorageGetPageFun = async () => {
    const { data } = await StorageGetPage("LX");
    tagList.value = data;
    if (tagList.value.length) {
      activeName.value = tagList.value[0].name;
      activeCode.value = tagList.value[0].code;
      GetSignalDictList();
    }
    // console.log("activeCode is", activeCode.value);
  };

  onMounted(async () => {
    StorageGetPageFun();
    timer = setInterval(() => {
      handleClick({
        props: {
          name: activeName.value,
          label: activeCode.value
        }
      });
    }, 3 * 1000); // 每秒轮询一次
  });

  onUnmounted(() => {
    clearInterval(timer);
  });

  return {
    loading,
    dataList,
    activeName,
    handleClick,
    selection,
    tagList,
    deviceDetection
  };
}
