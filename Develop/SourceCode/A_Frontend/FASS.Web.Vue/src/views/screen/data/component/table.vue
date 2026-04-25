<script lang="ts" setup>
import { object } from "vue-types";

let tableData = [];
const props = defineProps<{
  title?: string;
  tableData: object;
}>();
tableData = props.tableData as any;
</script>
<template>
  <div class="title">{{ title }}</div>
  <el-table
    class="table"
    :data="tableData"
    height="250"
    border
    style="width: 100%"
  >
    <el-table-column prop="carno" label="编号" width="60" />
    <el-table-column prop="area" label="区域" width="65" />
    <el-table-column prop="alarm" label="报警" />
    <el-table-column prop="datetime" label="时间" width="160" />
  </el-table>
</template>

<style scoped lang="scss">
.title {
  color: #9da8d0;
  font-size: 18px;
  font-weight: 600;
}
.el-table {
  --el-table-border-color: #448ef7;
  // --el-table-border: none;
  --el-table-text-color: #a0d3fb;
  --el-table-header-text-color: #a0d3fb;
  --el-table-row-hover-bg-color: transparent;
  --el-table-current-row-bg-color: transparent;
  --el-table-header-bg-color: transparent;
  --el-table-bg-color: transparent;
  --el-table-tr-bg-color: transparent;
  --el-table-expanded-cell-bg-color: transparent;
}
</style>
