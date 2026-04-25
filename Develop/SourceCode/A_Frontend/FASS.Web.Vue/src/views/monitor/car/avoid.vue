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

// defineOptions({
//   name: "Base-Map"
// });

const treeRef = ref();
const formRef = ref();
const tableRef = ref();

// defineExpose({ tableRef });

const {
  query,
  loading,
  columns,
  carTypeList,
  dataList,
  pagination,
  deviceDetection,
  handleSelection,
  handlePageSize,
  handlePageCurrent,
  handleSearch,
  handleReset,
  handleDetail,
  handleAdd,
  handleUpdate,
  handleDelete,
  handleEnable,
  handleDisable
  // handleResetPassword,
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
        <el-form-item :label="$t('table.carType')" prop="carTypeId">
          <el-select
            v-model="query.carTypeId"
            filterable
            class="!w-[200px]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option label="--全部--" value="" />
            <el-option
              v-for="item in carTypeList"
              :label="item.name"
              :value="item.id"
              :key="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.code')" prop="code">
          <el-input
            v-model="query.code"
            :placeholder="$t('table.code')"
            clearable
            class="!w-[200px]"
          />
        </el-form-item>
        <el-form-item :label="$t('table.name')" prop="name">
          <el-input
            v-model="query.name"
            :placeholder="$t('table.name')"
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
          <el-button
            type="primary"
            :icon="useRenderIcon(View)"
            @click="handleDetail()"
          >
            {{ $t("buttons.view") }}
          </el-button>
          <!--<el-button
            type="primary"
            :icon="useRenderIcon(Plus)"
            @click="handleAdd()"
          >
            新增
          </el-button>
          <el-button
            type="primary"
            :icon="useRenderIcon(Edit)"
            @click="handleUpdate()"
          >
            修改
          </el-button>
          <el-button
            type="primary"
            :icon="useRenderIcon(Delete)"
            @click="handleDelete()"
          >
            删除
          </el-button>
          <el-button
            type="primary"
            :icon="useRenderIcon(Check)"
            @click="handleEnable()"
          >
            启用
          </el-button>
          <el-button
            type="primary"
            :icon="useRenderIcon(Close)"
            @click="handleDisable()"
          >
            禁用
          </el-button>
           <el-button
            type="primary"
            :icon="useRenderIcon(User)"
            @click="handleResetPassword()"
          >
            重置
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
                查看
              </el-button>
              <!-- <el-button
                link
                type="primary"
                :size="size"
                :icon="useRenderIcon(Edit)"
                @click="handleUpdate([row])"
              >
                修改
              </el-button>
              <el-button
                link
                type="primary"
                :size="size"
                :icon="useRenderIcon(Delete)"
                @click="handleDelete([row])"
              >
                删除
              </el-button> -->
            </template>
          </pure-table>
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
