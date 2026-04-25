<script setup lang="ts">
import Chart from "./component/chart.vue";
import Columnar from "./component/Columnar.vue";
import { ref, watch } from "vue";
import { useHook } from "./utils/hook";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";
defineOptions({
  name: ""
});
const treeRef = ref();
const formRef = ref();
const tableRef = ref();

const {
  query,
  loading,
  shortcuts,
  dataList,
  deviceDetection,
  handleSearch,
  handleReset,
  handleSearch7Day,
  handleSearch30Day
} = useHook(tableRef);
let modeList = ref([
  {
    name: "总故障",
    value: 10023,
    code: "total"
  },
  {
    name: "安全触边报警",
    value: 10230,
    code: "touchEdgeFault"
  },
  {
    name: "急停报警",
    value: 10320,
    code: "stopFault"
  },
  {
    name: "驱动器报警",
    value: 1200,
    code: "qdqFault"
  },
  {
    name: "二维码丢失报警",
    value: 1030,
    code: "100"
  },
  {
    name: "定位报警",
    value: 2100,
    code: "101"
  }
]);
let data = ref([]);
watch(
  () => dataList,
  newVal => {
    data.value = newVal.value;
  },
  {
    deep: true
  }
);
</script>

<template>
  <div :class="['flex', 'justify-between', deviceDetection() && 'flex-wrap']">
    <div :class="[deviceDetection() ? ['w-full', 'mt-2'] : 'w-[calc(100%)]']">
      <el-form
        ref="formRef"
        :inline="true"
        :model="query"
        class="search-form bg-bg_color w-[99/100] pl-8 pt-[12px] overflow-auto"
      >
        <el-form-item :label="$t('table.time')" prop="time">
          <el-date-picker
            v-model="query.time"
            type="daterange"
            unlink-panels
            range-separator="To"
            start-placeholder="Start date"
            end-placeholder="End date"
            :shortcuts="shortcuts"
            @change="handleDateChange"
          />
        </el-form-item>
        <el-form-item>
          <el-button
            type="primary"
            :icon="useRenderIcon(Search)"
            :loading="loading"
            @click="handleSearch"
          >
            {{ $t("buttons.search") }}
          </el-button>
          <el-button
            :icon="useRenderIcon(Refresh)"
            @click="handleReset(formRef)"
          >
            {{ $t("buttons.reset") }}
          </el-button>
        </el-form-item>
      </el-form>

      <div class="cardList">
        <el-row :gutter="20">
          <el-col
            :span="4"
            v-for="(dataList, index) in dataList.totalData"
            :key="index"
          >
            <div class="item" :class="'bgStyle' + index">
              <!-- :class="dataList.code" -->
              <div>
                <p class="num">{{ dataList.value ? dataList.value : 0 }}</p>
                <p class="type">
                  {{ dataList.name ? dataList.name : dataList.code }}
                </p>
              </div>
            </div>
          </el-col>
        </el-row>
      </div>
      <div class="chart">
        <div class="chart-left">
          <Columnar
            title="报警类型TOP"
            :dataList="dataList.columnarChartData"
          />
        </div>
        <div class="chart-right">
          <Chart title="报警类型对比" :dataList="dataList.pieChartData" />
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
@font-face {
  font-family: "conthrax-sb";
  src: url("@/assets/report/fonts/conthrax-sb.ttf");
}
.cardList {
  background-color: #fff;
  padding: 10px;
  margin-top: 10px;
}
.item {
  height: 100px;
  border-radius: 4px;
  padding: 10px;
  margin-top: 10px;
  text-align: center;
  .num {
    font-family: conthrax-sb;
    font-size: 36px;
    overflow: hidden;
  }
  .type {
    font-size: 16px;
    color: #000;
  }
}
.bgStyletotal {
  background: linear-gradient(to top, #e9f4fd, #f3f9fd);
}

.bgStyle0 {
  color: #af4be7;
  background: linear-gradient(to top, #e0ccec, #f7effc);
}
.bgStyle1 {
  color: #fc1b1b;
  background: linear-gradient(180deg, #e9f4ff, #cae4ff);
}
.bgStyle2 {
  color: #1bd135;
  background: linear-gradient(180deg, #f1fff6, #d5f7e1);
}
.bgStyle3 {
  color: #fc1b1b;
  background: linear-gradient(180deg, #fdf9ed, #fbf0c8);
}
.bgStyle4 {
  color: #fc1b1b;
  background: linear-gradient(180deg, #fff2ed, #f9d1c4);
}
.bgStyle5 {
  color: #fc1b1b;
  background: linear-gradient(180deg, #f2f1ff, #d3cff7);
}
.bgStyle6 {
  background: linear-gradient(to top, #ebfdef, #f6fef2);
}
.chart {
  height: 50vh;
  width: 100%;
  background-color: #fff;
  display: flex;
  .chart-left {
    flex: 1;
    padding: 6px;
    margin: 6px;
    border: 1px solid #e6e6e6;
    border-radius: 6px;
  }
  .chart-right {
    flex: 2;
    padding: 6px;
    margin: 6px;
    margin-left: 0;
    border: 1px solid #e6e6e6;
    border-radius: 6px;
  }
}
</style>
