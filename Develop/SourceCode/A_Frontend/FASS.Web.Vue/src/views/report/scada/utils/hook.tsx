import { ref, reactive, onMounted } from "vue";
import { useRouter } from "vue-router";
import { message } from "@/utils/message";
import { deviceDetection } from "@pureadmin/utils";
// import { useMyI18n } from "@/plugins/i18n";
import * as echarts from "echarts";
import {
  formatDate
  // formatShortDate
} from "@/utils/common/format";
import { GetChargeConsumeReport } from "@/api/report/scada/index";
import dayjs from "dayjs";

// interface EnergyData {
//   created: string;
//   energy: number;
// }
// 定义 query 对象的类型
interface Query {
  time: [Date, Date]; // 确保 time 是一个包含两个 Date 对象的元组
}
interface TaskData {
  created: string;
  count: number;
}

interface ChargerData {
  // Define the properties for charger data here
}

interface AmrData {
  // Define the properties for amr data here
}

export function useHook() {
  const router = useRouter();
  // const { t } = useMyI18n();
  const query: Query = reactive({
    time: [dayjs().subtract(1, "week").toDate(), new Date()] // 最近一周的默认值
  });
  const formattedQuery = reactive({
    createAtStart: "",
    createAtEnd: ""
  });
  const loading = ref(true);
  const dataList = ref([]);
  const pageParam = ref({});
  const fromDate = ref<Date>(
    new Date(new Date().getFullYear(), new Date().getMonth(), 1)
  );
  function handlePageParam() {
    return {
      Param: JSON.stringify({
        // CarCode: "12",
        createAtStart: formattedQuery.createAtStart,
        createAtEnd: formattedQuery.createAtEnd
      })
    };
    // return JSON.stringify({
    //   Param: {
    //     // CarCode: "12",
    //     createAtStart: formattedQuery.createAtStart,
    //     createAtEnd: formattedQuery.createAtEnd
    //   }
    // });
  }

  function handleReset(form) {
    if (!form) {
      return;
    }
    form.resetFields();
    query.time = [dayjs().subtract(1, "week").toDate(), new Date()];
    handleDateChange(query.time);
    handleSearch();
  }

  // const toDate = ref<Date>(new Date());
  // const dailyEnergyChart = ref<echarts.ECharts | null>(null);
  const dailyEnergyProduced = ref([]);
  const dailyEnergyConsumed = ref([]);
  // const dailyTaskChart = ref<echarts.ECharts | null>(null);
  // const dailyTaskCounts = ref<TaskData[]>([]);
  const chargers = ref<ChargerData[]>([]);
  const amrs = ref<AmrData[]>([]);

  const init = () => {
    fromDate.value = new Date(
      fromDate.value.getFullYear(),
      fromDate.value.getMonth(),
      1
    );
  };

  const shortcuts = [
    {
      text: "Last week",
      value: () => {
        const end = new Date();
        const start = new Date();
        start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
        return [start, end];
      }
    },
    {
      text: "Last month",
      value: () => {
        const end = new Date();
        const start = new Date();
        start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
        return [start, end];
      }
    },
    {
      text: "Last 3 months",
      value: () => {
        const end = new Date();
        const start = new Date();
        start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
        return [start, end];
      }
    }
  ];

  // const initCharts = () => {
  //   dailyEnergyChart.value = echarts.init(
  //     document.getElementById("daily-energy-chart") as HTMLElement
  //   );
  //   dailyEnergyChart.value.setOption(dailyEnergyChartOption());
  // };

  // const dailyEnergyChartOption = () => {
  //   return {
  //     title: {
  //       text: "电能趋势日报",
  //       subtext: `${formatDate(fromDate.value)} ~ ${formatDate(toDate.value)}`,
  //       left: "50%"
  //     },
  //     tooltip: {
  //       trigger: "axis"
  //     },
  //     legend: {
  //       data: ["充入电能", "放出电能"],
  //       right: "10%"
  //     },
  //     xAxis: {
  //       name: "日期",
  //       type: "category",
  //       data: viewTime.value
  //     },
  //     yAxis: {
  //       name: "千瓦时",
  //       type: "value"
  //     },
  //     series: [
  //       {
  //         name: "充入电能",
  //         data: dailyEnergyProduced.value,
  //         type: "line"
  //       },
  //       {
  //         name: "放出电能",
  //         data: dailyEnergyConsumed.value,
  //         type: "line"
  //       }
  //     ]
  //   };
  // };

  const handleDateChange = (value: [Date, Date]) => {
    formattedQuery.createAtStart = dayjs(value[0]).format("YYYY-MM-DD");
    formattedQuery.createAtEnd = dayjs(value[1]).format("YYYY-MM-DD");
    // console.log(formattedQuery); // 这里你可以处理转换后的值
  };
  let viewTime = ref([]);
  handleDateChange(query.time); // 初始化时转换默认值
  const getChartData = async () => {
    let Param = handlePageParam();
    const { data } = await GetChargeConsumeReport(Param);
    try {
      data.chargeList.forEach(item => {
        dailyEnergyProduced.value.push(item.dn);
        viewTime.value.push(item.chargeTime);
      });
      data.disChargeList.forEach(item => {
        dailyEnergyConsumed.value.push(item.dn);
      });
    } catch (err: any) {
      message(`获取能耗看板数据失败：${err.message}`, { type: "error" });
    }
  };

  const getChargers = async () => {
    try {
      const { data } = await GetChargeConsumeReport(Param.value);
      chargers.value = data.chargers;
    } catch (err: any) {
      message(`获取充电站列表失败：${err.message}`, { type: "error" });
    }
  };

  const getAmrs = async () => {
    try {
      const { data } = await GetChargeConsumeReport(Param.value);
      amrs.value = data.amrs;
    } catch (err: any) {
      message(`获取AMR数据失败:${err.message}`, { type: "error" });
    }
  };

  async function handleSearch() {
    loading.value = true;
    pageParam.value = handlePageParam();
    const { data } = await GetChargeConsumeReport(pageParam.value);
    dataList.value = data;
    // console.log("dataList", dataList.value);
    // getChartData();
    // initCharts();
    loading.value = false;
  }

  onMounted(async () => {
    handleSearch();
    init();
    // initCharts();
    // getChartData();
    // getChargers();
    // getAmrs();
    setInterval(() => {
      // console.log("router is", router);
      if (router.currentRoute.path === "/report/scada/index") {
        // getChartData();
        // getChargers();
        // getAmrs();
      }
    }, 10000);
  });

  return {
    query,
    amrs,
    shortcuts,
    chargers,
    loading,
    dataList,
    handleReset,
    deviceDetection,
    handleSearch,
    handleDateChange
  };
}
