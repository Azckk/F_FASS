<script setup lang="ts">
import { ref, onMounted, watch, nextTick } from "vue";
import { router, resetRouter } from "@/router";
import { getTaskPage, GetUpdate } from "@/api/monitor/runtime";
import { getTasks } from "../utils/indexedDB";
const props = defineProps<{
  tableData: any;
  carStateList: any;
}>();
const dataList = ref(props.tableData);
let carState = ref({
  charging: 0,
  faulting: 0,
  idle: 0,
  offLine: 0,
  tasking: 0
});

watch(
  () => props.tableData,
  newVal => {
    dataList.value = newVal;
  },
  { immediate: true }
);
watch(
  () => props.carStateList,
  newVal => {
    carState.value = newVal;
  },
  { immediate: true }
);
function handleRouter(routerLink: string) {
  router.push(routerLink);
}
const emit = defineEmits(["row-double-click"]);

const handleRowDoubleClick = row => {
  emit("row-double-click", row);
};
</script>

<template>
  <!-- <div class="modal-header">
    <div>
      <el-button @click="handleRouter('/monitor/car/index')"
        >查看更多</el-button
      >
    </div>
  </div> -->
  <div class="contentList">
    <div class="content-header">
      <div>
        <div>{{ $t("table.exception") }}</div>
        <div style="text-align: center; color: red; font-weight: 800">
          {{ carState.faulting ? carState.faulting : 0 }}
        </div>
      </div>
      <div>
        <div>{{ $t("table.idle") }}</div>
        <div style="text-align: center; color: green; font-weight: 800">
          {{ carState.idle ? carState.idle : 0 }}
        </div>
      </div>
      <div>
        <div>{{ $t("table.charging") }}</div>
        <div style="text-align: center; color: orange; font-weight: 800">
          {{ carState.charging ? carState.charging : 0 }}
        </div>
      </div>
      <div>
        <div>{{ $t("table.inTask") }}</div>
        <div style="text-align: center; color: skyblue; font-weight: 800">
          {{ carState.tasking ? carState.tasking : 0 }}
        </div>
      </div>
      <div>
        <div>{{ $t("table.offline") }}</div>
        <div style="text-align: center; color: orange; font-weight: 800">
          {{ carState.offLine ? carState.offLine : 0 }}
        </div>
      </div>
    </div>
    <div class="content-table">
      <el-table
        :data="dataList"
        max-height="500"
        style="width: 100%"
        :default-sort="{ prop: 'name,charge', order: 'descending' }"
        @row-click="handleRowDoubleClick"
      >
        <el-table-column
          sortable
          prop="code"
          :label="$t('table.vehicleCoding')"
        />
        <el-table-column prop="name" :label="$t('table.carName')" />
        <el-table-column prop="state" :label="$t('table.carStatus')" />
        <el-table-column
          sortable
          prop="charge"
          :label="$t('table.electricity')"
        />
      </el-table>
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
  border-radius: 4px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  .modal-header {
    width: 100%;
    display: flex;
    justify-content: space-between;
  }
}
.contentList {
  // max-height: 500px;
  height: calc(100vh - 180px);
}
.content-header {
  padding: 15px;
  display: flex;
  justify-content: space-around;
  background-color: #eee;
}
</style>
