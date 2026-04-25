import { GetIndexData } from "@/api/welcome/index";
import dayjs from "dayjs";
import { ref } from "vue";

let Param = ref();

let testData = {
  success: true,
  code: 200,
  data: {
    rates: {
      time: [
        "08-09",
        "08-10",
        "08-11",
        "08-12",
        "08-13",
        "08-14",
        "08-15",
        "08-16"
      ],
      data: ["0", "0", "0", "0", "0", "0", "0", "0"]
    },
    taskCount: [
      {
        success: 106,
        failure: 20,
        total: 111,
        rete: 0,
        totalAlarm: 0
      }
    ],
    alarm: {
      columnarChartData: {
        data: [],
        name: [],
        alarmLevel: []
      },
      totalAlarm: 12
    },
    carState: [
      {
        name: "空闲",
        value: 1
      },
      {
        name: "任务中",
        value: 3
      },
      {
        name: "充电中",
        value: 4
      },
      {
        name: "异常",
        value: 5
      },
      {
        name: "离线",
        value: 1
      }
    ],
    chargeState: [
      {
        name: "充电中",
        value: 1
      },
      {
        name: "空闲中",
        value: 2
      },
      {
        name: "离线",
        value: 3
      }
    ]
  }
};
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

/** 数据总览 统计 */
const dataOverview = ref();
dataOverview.value = testData.data.taskCount[0];
dataOverview.value.totalAlarm = testData.data.alarm.totalAlarm;
if (dataOverview.value.total != 0) {
  dataOverview.value.rete = Number(
    ((dataOverview.value.success / dataOverview.value.total) * 100).toFixed(1)
  );
} else {
  dataOverview.value.rete = 0;
}
/** 数据总览折线图 */
const chartData = ref();
/** 机器人工作状态 */
const picData1 = ref({
  data: [],
  color: ["#3E95D0", "#994FED", "#15DC7D", "#E36739", "#A5A5A5"]
});
/** 充电状态 */
const picData2 = ref({
  data: [],
  color: ["#15DC7D", "#3E95D0", "#A5A5A5"]
});

/** 今日报警类型Top10 */
const columnarData = ref({
  data: [],
  name: [],
  alarmLevel: []
});
async function GetTaskReportFn() {
  // picData1.value.data = testData.data.carState;
  Param.value = handleMsg();
  const { data } = await GetIndexData(Param.value);
  testData.data = data;
  /** 数据总览 统计 */
  dataOverview.value = testData.data.taskCount[0];
  dataOverview.value.totalAlarm = testData.data.alarm.totalAlarm;
  if (dataOverview.value.total != 0) {
    dataOverview.value.rete = Number(
      ((dataOverview.value.success / dataOverview.value.total) * 100).toFixed(1)
    );
  } else {
    dataOverview.value.rete = 0;
  }
  /** 数据总览折线图 */
  chartData.value = testData.data.rates;
  /** 机器人工作状态 */
  picData1.value.data = testData.data.carState;
  /** 充电状态 */
  picData2.value.data = testData.data.chargeState;
  /** 今日报警类型Top10 */
  columnarData.value = {
    data: testData.data.alarm.columnarChartData.data.reverse(),
    name: testData.data.alarm.columnarChartData.name.reverse(),
    alarmLevel: testData.data.alarm.columnarChartData.alarmLevel?.reverse()
  };
  // columnarData.value = {
  //   data: [3, 2, 2, 2],
  //   name: [
  //     "二维码导航脱轨",
  //     "导航雷达通讯超时报警",
  //     "激光导航定位丢失",
  //     "避障雷达通讯超时报警"
  //   ],
  //   // alarmLevel: testData.data.alarm.columnarChartData.alarmLevel?.reverse()
  //   alarmLevel: ["1", "2", "0", "2"]
  // };
}
export {
  dataOverview,
  chartData,
  picData1,
  picData2,
  columnarData,
  GetTaskReportFn
};
