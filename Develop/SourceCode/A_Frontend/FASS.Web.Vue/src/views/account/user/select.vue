<script setup lang="ts">
import { ref } from "vue";
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import { useHook } from "./utils/hook";
import tree from "./tree.vue";
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
  name: "Account-User"
});

const treeRef = ref();
const formRef = ref();
const tableRef = ref();

defineExpose({ tableRef });

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
  handleDetail
} = useHook(tableRef, treeRef);
</script>

<template>
  <div class="main">
    <el-form
      ref="formRef"
      :inline="true"
      :model="query"
      class="search-form bg-bg_color w-[99/100] pl-8 pt-[12px]"
    >
      <el-form-item :label="$t('table.username')" prop="username">
        <el-input
          v-model="query.username"
          :placeholder="$t('table.username')"
          clearable
          class="!w-[200px]"
        />
      </el-form-item>
      <el-form-item :label="$t('table.uname')" prop="name">
        <el-input
          v-model="query.name"
          :placeholder="$t('table.uname')"
          clearable
          class="!w-[200px]"
        />
      </el-form-item>
      <el-form-item :label="$t('table.phone')" prop="phone">
        <el-input
          v-model="query.phone"
          :placeholder="$t('table.phone')"
          clearable
          class="!w-[200px]"
        />
      </el-form-item>
      <el-form-item :label="$t('table.mail')" prop="mail">
        <el-input
          v-model="query.mail"
          :placeholder="$t('table.mail')"
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
