<script setup lang="ts">
import { ref, watch, onMounted, nextTick, onUnmounted } from "vue";
import { getTaskPage, GetUpdate } from "@/api/monitor/runtime";
import { getTasks } from "../utils/indexedDB";
import { throttle } from "lodash";
const props = defineProps<{
  // data: any;
  // msg: string;
  tableData: any;
}>();
const tableData = ref(props.tableData);
const updateDataList = throttle(newVal => {
  // console.log("截流函数被执行");
  tableData.value = newVal;
}, 1000);
watch(
  () => props.tableData,
  newVal => {
    // tableData.value = newVal;
    updateDataList(newVal);
  },
  { immediate: true }
);
</script>

<template>
  <div class="contentList">
    <el-table :data="tableData" max-height="500" style="width: 100%">
      <el-table-column prop="code" :label="$t('table.taskCoding')" />
      <el-table-column prop="carName" :label="$t('table.carName')" />
      <el-table-column prop="carCode" :label="$t('table.vehicleCoding')" />
      <el-table-column prop="state" :label="$t('table.taskStatus')" />
    </el-table>
  </div>
</template>

<style lang="scss" scoped>
.contentList {
  max-height: 500px;
}
</style>
