<script setup lang="ts">
import { ref, watch } from "vue";
import { router, resetRouter } from "@/router";
import dayjs from "dayjs";
import { throttle } from "lodash";
const props = defineProps<{
  tableData: any;
}>();
const dataList = ref(props.tableData);
const updateDataList = throttle(newVal => {
  // console.log("截流函数被执行");
  dataList.value = newVal;
  if (Array.isArray(dataList.value) && dataList.value.length > 0) {
    dataList.value.forEach(item => {
      if (item.startTime) {
        let ltime = item.startTime.split("T")[1];
        if (ltime) {
          item.startTime = ltime.split(".")[0];
        }
      }
    });
  }
}, 1000); // 根据需求调整延迟时间

watch(
  () => props.tableData,
  (newVal, oldVal) => {
    // console.log("监听函数");
    updateDataList(newVal);
  },
  { immediate: true }
);

// dataList.value = [
//   {
//     level: null,
//     type: null,
//     code: "300",
//     message: "丢失二维码报警",
//     data: null,
//     state: "100",
//     startTime: "14:00:00",
//     endTime: "" // 可能为空字符串
//   },
//   {
//     level: null,
//     type: null,
//     code: "301",
//     message:
//       "1#障碍物检测减速停止区报警报警报警报警s障碍物检测减速停止区报警报警报警报警s障碍物检测减速停止区报警报警报警报警s",
//     data: null,
//     state: "100",
//     startTime: "14:00:00",
//     endTime: "14:00:00"
//   },
//   {
//     level: null,
//     type: null,
//     code: "302",
//     message: "再次报警",
//     data: null,
//     state: "100",
//     startTime: "14:00:00"
//     // 没有 endTime
//   }
// ];

function handleRouter(routerLink: string) {
  router.push(routerLink);
}
const isEndTimeInvalid = endTime => {
  return !endTime;
};
</script>

<template>
  <div class="contentList">
    <div v-if="dataList.length == 0" class="error">
      {{ $t("table.noData") }}
    </div>
    <div class="content" v-for="item in dataList" :key="item.id">
      <div class="contentStyle">
        <div
          class="startTime"
          :class="{ start: isEndTimeInvalid(item.endTime) }"
        >
          <div>{{ $t("table.startAlarm") }}：{{ item.startTime }}</div>
          <!-- <span class="endTime" v-if="!isEndTimeInvalid(item.endTime)"
            >{{ $t("table.endAlarm") }}：{{ item.endTime }}</span
          > -->
        </div>
        <div class="carInfo">
          <div class="carId">
            <img class="h-16" src="@/assets/statistics/amr2.png" alt="" />
            <span
              class="code"
              :class="{ start: isEndTimeInvalid(item.endTime) }"
              >{{ item.name }}</span
            >
          </div>
          <div id="carText">
            <!-- <h4>{{ item.name }}</h4> -->
            <h4>{{ item.message }}</h4>
            <h5>{{ $t("table.errorCode") }}：{{ item.state }}</h5>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.modal {
  position: absolute;
  // cursor: move;
  // top: 50%;
  // right: 0;
  transform: translate(-50%, -50%);
  background-color: #fff;
  padding: 20px;
  border-radius: 4px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  .modal-header {
    width: 100%;
    display: flex;
    justify-content: space-between;
  }
}
.contentList {
  max-height: 500px;
  background-color: #fff;
}
.content {
  min-height: 120px;
  padding: 10px;
  padding-top: 0;
  background-color: #fff;
}

.content:last-child {
  // margin-bottom: 2px;
  // padding-bottom: 10px;
}
.contentStyle {
  border: #c4e9fa 2px solid;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  box-sizing: border-box;
  border-radius: 6px;
  background-color: #f0f0f0;
  padding: 10px;
}
.carInfo {
  display: flex;
  justify-content: flex-start;
}
.startTime {
  font-weight: 500;
  font-size: 14px;
  margin-bottom: 5px;
  display: flex;
  justify-content: space-between;
}
.carId {
  font-size: 28px;
  margin-right: 60px;
  text-align: center;
  display: flex;
  align-items: center;
  .code {
    font-size: 14px;
    font-weight: 500;
    margin-left: 15px;
  }
}
#carText {
  color: #000;
  text-align: center;
}
.error {
  text-align: center;
  color: #999;
  font-size: 14px;
  padding: 20px;
  background-color: #fff;
}
.start {
  color: red;
}
.endTime {
  // margin-left: 60px;
  text-align: right;
}
</style>
