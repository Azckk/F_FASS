<script setup lang="ts">
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

defineOptions({
  name: "Warehouse-Storagecontainerhistory"
});

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
  handleDeleteM3,
  handleDeleteM1,
  handleDeleteW1,
  handleDeleteD1,
  handleDeleteAll
} = useHook(tableRef);
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
        <el-form-item :label="$t('table.storageCode')" prop="storageCode">
          <el-input
            v-model="query.storageCode"
            :placeholder="$t('table.storageCode')"
            clearable
            class="!w-[200px]"
          />
        </el-form-item>
        <el-form-item :label="$t('table.storage')" prop="containerCode">
          <el-input
            v-model="query.storageName"
            :placeholder="$t('table.storage')"
            clearable
            class="!w-[200px]"
          />
        </el-form-item>
        <el-form-item :label="$t('table.containerCode')" prop="containerCode">
          <el-input
            v-model="query.containerCode"
            :placeholder="$t('table.containerCode')"
            clearable
            class="!w-[200px]"
          />
        </el-form-item>
        <el-form-item :label="$t('table.container')" prop="containerCode">
          <el-input
            v-model="query.containerName"
            :placeholder="$t('table.container')"
            clearable
            class="!w-[200px]"
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
      <PureTableBar
        :title="$t('table.list')"
        :columns="columns"
        @refresh="handleSearch"
      >
        <template #buttons>
          <!-- <el-button
            type="primary"
            :icon="useRenderIcon(View)"
            @click="handleDetail()"
          >
            {{ $t("buttons.view") }}
          </el-button> -->
          <el-button
            type="danger"
            :icon="useRenderIcon(Delete)"
            @click="handleDeleteM3()"
          >
            {{ $t("buttons.threeMonths") }}
          </el-button>
          <el-button
            type="danger"
            :icon="useRenderIcon(Delete)"
            @click="handleDeleteM1()"
          >
            {{ $t("buttons.oneMonths") }}
          </el-button>
          <el-button
            type="danger"
            :icon="useRenderIcon(Delete)"
            @click="handleDeleteW1()"
          >
            {{ $t("buttons.oneWeek") }}
          </el-button>
          <el-button
            type="danger"
            :icon="useRenderIcon(Delete)"
            @click="handleDeleteD1()"
          >
            {{ $t("buttons.oneDay") }}
          </el-button>
          <el-button
            type="danger"
            :icon="useRenderIcon(Delete)"
            @click="handleDeleteAll()"
          >
            {{ $t("buttons.clearAll") }}
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
</style>
