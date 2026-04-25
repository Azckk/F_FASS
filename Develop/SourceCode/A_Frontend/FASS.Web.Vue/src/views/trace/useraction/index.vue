<script setup lang="ts">
import { ref } from "vue";
import { useHook } from "./utils/hook";
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";
import View from "@iconify-icons/ep/view";
import Delete from "@iconify-icons/ep/delete";

defineOptions({
  name: "Trace-UserAction"
});

const formRef = ref();
const tableRef = ref();
const {
  query,
  loading,
  columns,
  dataList,
  pagination,
  handleSelection,
  handlePageSize,
  handlePageCurrent,
  handleSearch,
  handleReset,
  handleDetail,
  handleDelete,
  handleDeleteM3,
  handleDeleteM1,
  handleDeleteW1,
  handleDeleteD1,
  handleDeleteAll
} = useHook(tableRef);
</script>

<template>
  <div class="main">
    <el-form
      ref="formRef"
      :inline="true"
      :model="query"
      class="search-form bg-bg_color w-[99/100] pl-8 pt-[12px]"
    >
      <el-form-item :label="$t('table.controller')" prop="controller">
        <el-input
          v-model="query.controller"
          :placeholder="$t('table.controller')"
          clearable
          class="!w-[200px]"
        />
      </el-form-item>
      <el-form-item :label="$t('table.operation')" prop="action">
        <el-input
          v-model="query.action"
          :placeholder="$t('table.operation')"
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
        <el-button :icon="useRenderIcon(Refresh)" @click="handleReset(formRef)">
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
        <el-button
          type="primary"
          :icon="useRenderIcon(View)"
          @click="handleDetail()"
        >
          {{ $t("buttons.view") }}
        </el-button>
        <el-button
          type="danger"
          :icon="useRenderIcon(Delete)"
          @click="handleDelete()"
        >
          {{ $t("buttons.delete") }}
        </el-button>
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
        >
          <template #operation="{ row }">
            <el-button
              link
              type="primary"
              :size="size"
              :icon="useRenderIcon(View)"
              @click="handleDetail([row])"
            >
              {{ $t("buttons.view") }}
            </el-button>
            <el-button
              link
              type="primary"
              :size="size"
              :icon="useRenderIcon(Delete)"
              @click="handleDelete([row])"
            >
              {{ $t("buttons.delete") }}
            </el-button>
          </template>
        </pure-table>
      </template>
    </PureTableBar>
  </div>
</template>

<style scoped lang="scss">
:deep(.el-dropdown-menu__item i) {
  margin: 0;
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
