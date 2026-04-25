<script setup lang="ts">
import { ref } from "vue";
import { useRouter, useRoute } from "vue-router";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
const router = useRouter();
import { PureTableBar } from "@/components/RePureTableBar";
import { useHook } from "./utils/hook";
import Header from "../home/components/header.vue";
const {
  query,
  loading,
  columns,
  dataList,
  pagination,
  deviceDetection,
  handlePageSize,
  handlePageCurrent,
  handleSearch,
  handleRouter
} = useHook();
</script>

<template>
  <div class="menuItem">
    <div style="height: 5rem"><Header class="header" /></div>
    <div :class="['flex', 'justify-between', deviceDetection() && 'flex-wrap']">
      <div :class="[deviceDetection() ? ['w-full', 'mt-2'] : 'w-[calc(100%)]']">
        <!-- <div>缺料呼叫</div> -->
        <el-card
          shadow="hover"
          class="flex justify-center items-center"
          @click="handleRouter('/mobile/storagecall/index')"
        >
          <div class="justify-center items-center leading-none">
            <img
              class="h-6 align-top mt-1"
              src="../../../assets/mobile/menu/call.png"
              alt=""
            />
            <span class="text-2xl">缺料呼叫</span>
          </div>
        </el-card>
        <PureTableBar
          :title="$t('table.list')"
          :columns="columns"
          @refresh="handleSearch"
        >
          <template #buttons>
            <!-- <el-button
            type="primary"
            :icon="useRenderIcon(Plus)"
            @click="handleAdd()"
          >
            {{ $t("buttons.add") }}
          </el-button> -->
          </template>
          <template v-slot="{ size, dynamicColumns }">
            <pure-table
              ref="tableRef"
              row-key="id"
              align-whole="center"
              adaptive
              :adaptiveConfig="{ offsetBottom: 100 }"
              :loading="loading"
              :data="dataList"
              :pagination="pagination"
              :size="size"
              :columns="dynamicColumns"
              :paginationSmall="size === 'small' ? true : false"
              :header-cell-style="{
                background: 'var(--el-fill-color-light)',
                color: 'var(--el-text-color-primary)'
              }"
              @page-size-change="handlePageSize"
              @page-current-change="handlePageCurrent"
            />
          </template>
        </PureTableBar>
      </div>
    </div>
  </div>
</template>

<style scoped>
.menuItem {
  margin: 0 !important;
}
</style>
