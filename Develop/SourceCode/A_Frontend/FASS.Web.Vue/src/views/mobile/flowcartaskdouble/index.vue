<!-- 双点任务 -->

<script setup lang="ts">
import Header from "../home/components/header.vue";
import { ref } from "vue";
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import { useHook } from "./utils/hook";
import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";
import View from "@iconify-icons/ep/view";
import Plus from "@iconify-icons/ep/plus";
import Edit from "@iconify-icons/ep/edit";
import Delete from "@iconify-icons/ep/delete";
import Check from "@iconify-icons/ep/check";
import Close from "@iconify-icons/ep/close";
import User from "@iconify-icons/ep/user";
import ZoomIn from "@iconify-icons/ep/zoom-in";
import Expand from "@iconify-icons/ep/expand";
import Pointer from "@iconify-icons/ep/pointer";

defineOptions({
  name: "Base-Zone"
});
/**
 * @prame
 * @return
 */

const treeRef = ref();
const formRef = ref();
const tableRef = ref();

const {
  query,
  loading,
  columns,
  dataList,
  pagination,
  deviceDetection,
  handleSelection,
  handlePageSize,
  handlePageCurrent,
  handleSearch,
  handleReset,
  handleAdd,
  DictItemInLocalList
} = useHook(tableRef);
</script>

<template>
  <div class="menuItem">
    <div style="height: 5rem"><Header class="header" /></div>
    <div :class="['flex', 'justify-between', deviceDetection() && 'flex-wrap']">
      <div :class="[deviceDetection() ? ['w-full', 'mt-2'] : 'w-[calc(100%)]']">
        <el-form
          ref="formRef"
          :inline="true"
          :model="query"
          class="search-form bg-bg_color w-[99/100] pl-8 pt-[12px] overflow-auto"
        >
          <el-form-item :label="$t('table.code')" prop="code">
            <el-input
              v-model="query.code"
              :placeholder="$t('table.code')"
              clearable
              class="!w-[60px]"
            />
          </el-form-item>
          <el-form-item :label="$t('table.car')" prop="name">
            <el-input
              v-model="query.name"
              :placeholder="$t('table.car')"
              clearable
              class="!w-[60px]"
            />
          </el-form-item>

          <el-form-item :label="$t('table.taskStatus')" prop="state">
            <el-select
              v-model="query.state"
              filterable
              class="!w-[100px]"
              :placeholder="$t('table.pleaseSelect')"
            >
              <el-option :label="$t('table.doNotSpecify')" value="" />
              <el-option
                v-for="item in DictItemInLocalList"
                :key="item.code"
                :label="item.name"
                :value="item.code"
              />
            </el-select>
          </el-form-item>
          <el-form-item>
            <el-button
              type="primary"
              :icon="useRenderIcon(Search)"
              :loading="loading"
              @click="handleSearch"
            >
            </el-button>
            <el-button
              :icon="useRenderIcon(Refresh)"
              @click="handleReset(formRef)"
            >
            </el-button>
          </el-form-item>
        </el-form>
        <PureTableBar
          :title="$t('table.list')"
          :columns="columns"
          @refresh="handleSearch"
        >
          <template #buttons>
            <el-button
              type="primary"
              :icon="useRenderIcon(Plus)"
              @click="handleAdd()"
            >
              {{ $t("buttons.add") }}
            </el-button>
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
              @selection-change="handleSelection"
              @page-size-change="handlePageSize"
              @page-current-change="handlePageCurrent"
            />
          </template>
        </PureTableBar>
      </div>
    </div>
  </div>
</template>

<style scoped lang="scss">
:deep(.el-dropdown-menu__item i) {
  margin: 0;
}

:deep(.el-button:focus-visible) {
  outline: none;
}

.main-content {
  margin: 24px 24px 0 !important;
}

.search-form {
  :deep(.el-form-item) {
    margin-bottom: 12px;
  }
}
.menuItem {
  margin: 0 !important;
}
.header {
  height: 4rem;
  width: 100%;
  position: absolute;
  top: 0px;
  z-index: 999;
}
</style>
