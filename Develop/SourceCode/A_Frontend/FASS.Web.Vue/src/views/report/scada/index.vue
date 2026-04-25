<script lang="ts" setup>
import Charger from "./component/charger.vue";
import Line from "./component/line.vue";
import Amr from "./component/amr.vue";
import { useHook } from "./utils/hook";
import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import { ref } from "vue";
const formRef = ref();
const {
  query,
  amrs,
  chargers,
  shortcuts,
  loading,
  dataList,
  handleReset,
  deviceDetection,
  handleSearch,
  handleDateChange
} = useHook();
</script>
<template>
  <div :class="['w-[calc(100%)]']">
    <div style="width: 99%">
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
        <!-- <el-form-item :label="$t('table.endTime')" prop="createAtEnd">
          <el-date-picker
            v-model="query.createAtEnd"
            type="datetime"
            :placeholder="$t('table.endTime')"
            format="YYYY-MM-DD"
            value-format="YYYY-MM-DD"
          />
        </el-form-item> -->
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
      <el-row style="background-color: #fff">
        <el-col :span="14">
          <!-- <div id="daily-energy-chart" class="cnt"></div> -->
          <Line class="line" :dataList="dataList" :query="query" />
        </el-col>
        <el-col :span="10">
          <div class="charger">
            <Charger class="cnt" :chargers="chargers" />
          </div>
          <div class="amr-box">
            <Amr class="cnt" :amrs="amrs" />
          </div>
        </el-col>
      </el-row>
      <!-- <el-row :gutter="8">
        <el-col :span="14">
          <div id="daily-task-chart" class="cnt"></div>
        </el-col>
        <el-col :span="10">
          <div class="amr-box">
              <Amr class="cnt" :amrs="amrs" />
          </div>
        </el-col>
      </el-row> -->
      <!-- 确保目标元素存在 -->
      <div class="app-main el-scrollbar__wrap">
        <!-- 内容 -->
      </div>
    </div>
  </div>
</template>
<style scoped>
#daily-energy-chart {
  width: 100%;
  height: 800px;
}
#daily-task-chart {
  width: 100%;
  height: 400px;
}
.charger {
  height: 400px;
}
.amr-box {
  height: 400px;
}
.line {
  width: 100%;
  height: 70vh;
}
</style>
