<script setup lang="ts">
import { ref } from "vue";
import { useHook } from "./utils/hook";
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";
import View from "@iconify-icons/ep/view";
import Plus from "@iconify-icons/ep/plus";
import Edit from "@iconify-icons/ep/edit";
import Delete from "@iconify-icons/ep/delete";

defineOptions({
  name: "Account-Permission"
});

const formRef = ref();
const tableRef = ref();
const {
  query,
  loading,
  columns,
  dataList,
  handleSelection,
  handleSearch,
  handleReset,
  handleDetail,
  handleAdd,
  handleUpdate,
  handleDelete
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
      <el-form-item :label="$t('table.code')" prop="code">
        <el-input
          v-model="query.code"
          :placeholder="$t('table.code')"
          clearable
          class="!w-[200px]"
        />
      </el-form-item>
      <el-form-item :label="$t('table.method')" prop="method">
        <el-input
          v-model="query.method"
          :placeholder="$t('table.method')"
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
      row-key="id"
      :columns="columns"
      :tableRef="tableRef?.getTableRef()"
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
          type="primary"
          :icon="useRenderIcon(Plus)"
          @click="handleAdd()"
        >
          {{ $t("buttons.add") }}
        </el-button>
        <el-button
          type="primary"
          :icon="useRenderIcon(Edit)"
          @click="handleUpdate()"
        >
          {{ $t("buttons.modification") }}
        </el-button>
        <el-button
          type="primary"
          :icon="useRenderIcon(Delete)"
          @click="handleDelete()"
        >
          {{ $t("buttons.delete") }}
        </el-button>
      </template>
      <template v-slot="{ size, dynamicColumns }">
        <pure-table
          ref="tableRef"
          row-key="id"
          align-whole="center"
          showOverflowTooltip
          default-expand-all
          adaptive
          :adaptiveConfig="{ offsetBottom: 100 }"
          :loading="loading"
          :data="dataList"
          :size="size"
          :columns="dynamicColumns"
          :paginationSmall="size === 'small' ? true : false"
          :header-cell-style="{
            background: 'var(--el-fill-color-light)',
            color: 'var(--el-text-color-primary)'
          }"
          @selection-change="handleSelection"
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
              :icon="useRenderIcon(Edit)"
              @click="handleUpdate([row])"
            >
              {{ $t("buttons.modification") }}
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

<style lang="scss" scoped>
:deep(.el-table__inner-wrapper::before) {
  height: 0;
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
