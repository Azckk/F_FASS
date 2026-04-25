import { ref, onMounted, onUnmounted } from "vue";
import { getData } from "@/api/screen/data";
import dayjs from "dayjs";
export function useHook() {
  const dataList = ref();
  async function BeforeData() {
    await getDataFn();
  }
  async function AfterData() {
    await getDataFn();
  }
  let Param = ref();
  function handleMsg() {
    return {
      Param: JSON.stringify({
        createAtStart: dayjs()
          .subtract(6, "day")
          .startOf("day")
          .format("YYYY-MM-DD HH:mm:ss"),
        createAtEnd: dayjs().endOf("day").format("YYYY-MM-DD HH:mm:ss")
      })
    };
  }
  async function getDataFn() {
    Param.value = handleMsg();
    let res = await getData(Param.value);
    dataList.value = res.data;
    return res.data;
  }
  function transformData(data) {
    const result = {
      name: [],
      value: []
    };
    // 遍历原始数据
    data.forEach(item => {
      // 将每个项目的 name 和 value 分别添加到 result 对应的数组中
      result.name.push(item.name);
      result.value.push(item.value);
    });
    return result;
  }
  const timer = ref(null);
  onMounted(async () => {
    getDataFn();
    timer.value = setInterval(() => {
      getDataFn();
    }, 5000);
  });
  onUnmounted(() => {
    // 清理操作
    clearInterval(timer.value);
    timer.value = null;
  });

  return {
    dataList,
    getDataFn,
    transformData,
    BeforeData,
    AfterData
  };
}
