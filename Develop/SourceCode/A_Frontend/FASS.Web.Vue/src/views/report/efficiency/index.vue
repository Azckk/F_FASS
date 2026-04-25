<script setup lang="ts">
import LineChart from "./component/line.vue";
import { ref, onMounted, watch } from "vue";
import { useHook } from "./utils/hook";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";
import { useMyI18n } from "@/plugins/i18n";
import allImgs from "@/assets/report/imgs/all.png";
import overImgs from "@/assets/report/imgs/over.png";
import errImgs from "@/assets/report/imgs/error.png";
import completeImgs from "@/assets/report/imgs/complete.png";

const { t, locale } = useMyI18n();
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
// let dataList = ref();
let taskList = ref();
let overTask = ref();
let errTask = ref();
let bTask = ref();
watch(
  () => dataList,
  newVal => {
    if (newVal.value.taskTotal) {
      taskList.value = newVal.value.taskTotal.total;
      overTask.value = newVal.value.taskTotal.success;
      errTask.value = newVal.value.taskTotal.failure;
      if (taskList.value != 0) {
        bTask.value = Number(
          ((overTask.value / taskList.value) * 100).toFixed(1)
        );
      } else {
        bTask.value = 0;
      }
    }
  },
  {
    deep: true,
    immediate: true
  }
);
onMounted(() => {});
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
        <el-form-item>
          <!-- <el-button
            type="primary"
            :loading="loading"
            @click="handleSearch7Day"
          >
            近7天
          </el-button>
          <el-button
            type="primary"
            :loading="loading"
            @click="handleSearch30Day"
          >
            近30天
          </el-button> -->
        </el-form-item>
      </el-form>
      <div class="cardList">
        <el-row :gutter="20">
          <el-col :span="6">
            <div class="task item">
              <div>
                <img :src="allImgs" alt="" />
              </div>
              <div>
                <p>{{ taskList }}</p>
                <p>{{ $t("table.totalTask") }}</p>
              </div>
            </div>
          </el-col>
          <el-col :span="6">
            <div class="overTask item">
              <div>
                <img :src="completeImgs" alt="" />
              </div>
              <div>
                <p>{{ overTask }}</p>
                <p>{{ $t("table.completeTheTask") }}</p>
              </div>
            </div>
          </el-col>
          <el-col :span="6">
            <div class="errTask item">
              <div>
                <img :src="errImgs" alt="" />
              </div>
              <div>
                <p>{{ errTask }}</p>
                <p>{{ $t("table.exceptionTasks") }}</p>
              </div>
            </div>
          </el-col>
          <el-col :span="6">
            <div class="bTask item">
              <div>
                <img :src="overImgs" alt="" />
              </div>
              <div>
                <p>{{ bTask }}%</p>
                <p>{{ $t("table.taskAchievementRate") }}</p>
              </div>
            </div>
          </el-col>
        </el-row>
      </div>
      <div class="chart">
        <LineChart :dataList="dataList" :query="query" />
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
  height: 127px;
  width: 265px;
  border-radius: 4px;
  margin-top: 10px;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: space-evenly;
  font-family: conthrax-sb;
  p:nth-child(2) {
    font-size: 16px;
    color: #000;
  }
  p:nth-child(1) {
    font-size: 36px;
    font-weight: 600;
  }
  div > p {
    max-width: 200px;
    white-space: nowrap; /* 禁止换行 */
    overflow: hidden; /* 隐藏超出容器的内容 */
    text-overflow: ellipsis; /* 使用省略号表示溢出部分 */
    display: block;
  }
  img {
    height: 86px;
  }
}
.task {
  color: #af4be7;
  img {
    margin-top: 10px;
  }
  background: linear-gradient(to right, #e4d4ed, #ffffff);
}
.overTask {
  color: #1bd135;
  background: linear-gradient(to right, #d9ffde, #ffffff);
}
.errTask {
  color: #fc1b1b;
  background: linear-gradient(to right, #ffeeca, #ffffff);
}
.bTask {
  color: #1bd135;
  background: linear-gradient(to right, #d9ffde, #ffffff);
}
.chart {
  height: 50vh;
  width: 100%;
  background-color: #fff;
}
</style>
