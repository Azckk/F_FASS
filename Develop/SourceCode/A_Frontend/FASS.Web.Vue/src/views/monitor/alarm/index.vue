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
import Pointer from "@iconify-icons/ep/pointer";

// defineOptions({
//   name: "Base-Map"
// });

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
  shortcuts,
  handleDetail,
  handleDelete,
  handleDeleteM3,
  handleDeleteM1,
  handleDeleteW1,
  handleDeleteD1,
  handleDeleteAll,
  handleExport,
  ListToSelectByTaskTemplateId
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
        <!-- <el-form-item :label="$t('table.startTime')" prop="createAtStart">
          <el-date-picker
            v-model="query.createAtStart"
            type="datetime"
            :placeholder="$t('table.startTime')"
            format="YYYY-MM-DD"
            value-format="YYYY-MM-DD"
          />
        </el-form-item>
        <el-form-item :label="$t('table.endTime')" prop="createAtEnd">
          <el-date-picker
            v-model="query.createAtEnd"
            type="datetime"
            :placeholder="$t('table.endTime')"
            format="YYYY-MM-DD"
            value-format="YYYY-MM-DD"
          />
        </el-form-item> -->
        <!-- <el-form-item :label="$t('table.code')" prop="code">
          <el-input
            v-model="query.code"
            :placeholder="$t('table.code')"
            clearable
            class="!w-[200px]"
          />
        </el-form-item> -->
        <el-form-item :label="$t('table.message')" prop="message">
          <el-input
            v-model="query.message"
            :placeholder="$t('table.message')"
            clearable
            class="!w-[200px]"
          />
        </el-form-item>
        <el-form-item :label="$t('table.ownVehicle')" prop="code">
          <el-select
            v-model="query.code"
            filterable
            class="!w-[200px]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.doNotSpecify')" value="" />
            <el-option
              v-for="item in ListToSelectByTaskTemplateId"
              :key="item.id"
              :label="item.code"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.createAt')" prop="time">
          <div class="block">
            <el-date-picker
              v-model="query.time"
              type="daterange"
              unlink-panels
              range-separator="To"
              start-placeholder="Start date"
              end-placeholder="End date"
              :shortcuts="shortcuts"
            />
          </div>
        </el-form-item>

        <!-- <el-form-item :label="$t('table.carName')" prop="carName">
          <el-input
            v-model="query.carName"
            :placeholder="$t('table.carName')"
            clearable
            class="!w-[200px]"
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
      <PureTableBar
        :title="$t('table.list')"
        :columns="columns"
        @refresh="handleSearch"
      >
        <template #buttons>
          <el-button
            type="primary"
            :icon="useRenderIcon(Pointer)"
            @click="handleExport()"
          >
            {{ $t("buttons.export") }}
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
              <!-- <el-button
                link
                type="primary"
                :size="size"
                :icon="useRenderIcon(View)"
                @click="handleDetail([row])"
              >
                {{ $t("buttons.view") }}
              </el-button> -->
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
