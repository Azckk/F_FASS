import { ref, onMounted, onUnmounted } from "vue";
import { deviceDetection } from "@pureadmin/utils";
import { getData } from "@/api/signal/generalsafetysignal";
import type { TabsPaneContext } from "element-plus";

let testData = {
  success: true,
  code: 200,
  message: "",
  data: [
    {
      DeviceName: "内饰机构1",
      SignalInfos: [
        {
          TypeName: "内饰机构信号",
          TypeCode: "QNSFetchLeftStatus",
          Signals: [
            {
              Key: "请求进",
              Value: "true"
            },
            {
              Key: "允许进入",
              Value: "false"
            },
            {
              Key: "其他",
              Value: "12312312312312"
            }
          ]
        },
        {
          TypeName: "内饰机构信号",
          TypeCode: "QNSFetchRightStatus",
          Signals: [
            {
              Key: "请求进",
              Value: "true"
            },
            {
              Key: "允许进入",
              Value: "false"
            }
          ]
        }
      ]
    },
    {
      DeviceName: "内饰机构2",
      SignalInfos: [
        {
          TypeName: "内饰机构信号",
          TypeCode: "QNSFetchLeftStatus",
          Signals: [
            {
              Key: "请求进",
              Value: "true"
            },
            {
              Key: "允许进入",
              Value: "false"
            }
          ]
        },
        {
          TypeName: "内饰机构信号",
          TypeCode: "QNSFetchRightStatus",
          Signals: [
            {
              Key: "请求进",
              Value: "true"
            },
            {
              Key: "允许进入",
              Value: "false"
            }
          ]
        }
      ]
    }
  ]
};

export function useHook() {
  const loading = ref(true);
  const tabList = ref();
  const dataList = ref();
  const selection = ref([]);
  const activeName = ref(0);
  const activeCode = ref("1");
  const tagList = ref([]);
  let timer = null;
  let activeLabel = ref();
  const handleClick = async (tab?: TabsPaneContext, event?: Event) => {
    activeLabel.value = tab.props.label;
    dataList.value = await getContentList(activeLabel.value);
    StorageGetPageFun();
    loading.value = false;
  };
  function getContentList(table = tabList.value[0].DeviceName) {
    let data;
    try {
      tabList.value.forEach(item => {
        if (item.DeviceName === table && item.SignalInfos) {
          data = item.SignalInfos;
          return item.SignalInfos;
        }
      });
    } catch {
      console.log("cuowu########");
    }
    return data;
  }

  const StorageGetPageFun = async () => {
    loading.value = true;
    const res = await getData();
    if (res.code == 200) {
      tabList.value = res.data;
      dataList.value = getContentList(
        activeLabel.value ? activeLabel.value : tabList.value[0].DeviceName
      );
    } else {
      tabList.value = [];
    }

    loading.value = false;
    // activeCode.value = tagList.value[0].DeviceName;

    // console.log("activeCode is", activeCode.value);
  };

  onMounted(async () => {
    await StorageGetPageFun();
    dataList.value = await getContentList(tabList.value[0].DeviceName);
    timer = setInterval(() => {
      StorageGetPageFun();
    }, 1 * 1000);
  });

  onUnmounted(() => {
    clearInterval(timer);
  });

  return {
    loading,
    tabList,
    dataList,
    activeName,
    handleClick,
    selection,
    tagList,
    deviceDetection,
    StorageGetPageFun
  };
}
