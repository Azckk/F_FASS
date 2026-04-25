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
import LocationFilled from "@iconify-icons/ep/location-filled";
import { formRules } from "./utils/rule";
import { indexProps } from "./utils/types";

const props = withDefaults(defineProps<indexProps>(), {
  formInline: () => ({
    id: "",
    nodeId: "",
    kind: "",
    type: "",
    code: "",
    name: "",
    isLock: false,
    maxCar: ""
  })
});
const newFormInline = ref(props.formInline);
const nodeId = ref(newFormInline.value.nodeId);
defineOptions({
  name: "Base-Zone"
});

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
  handleSearchArrList,
  handleReset,
  handleDetail,
  handleAdd,
  handleEdit,
  handleDelete,
  handleUpdate
} = useHook(tableRef, nodeId.value);
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
        <el-form-item :label="$t('table.actionType')">
          <el-input
            v-model="query.actionType"
            :placeholder="$t('table.actionType')"
            clearable
            class="!w-[200px]"
          />
        </el-form-item>
        <el-form-item :label="$t('table.blockageType')">
          <el-input
            v-model="query.blockingType"
            :placeholder="$t('table.blockageType')"
            clearable
            class="!w-[200px]"
          />
        </el-form-item>
        <el-form-item>
          <el-button
            type="primary"
            :icon="useRenderIcon(Search)"
            :loading="loading"
            @click="handleSearchArrList"
          >
            <!-- 当有真实接口之后替换掉当前搜索方法 -->
            {{ $t("table.search") }}
          </el-button>
          <el-button
            :icon="useRenderIcon(Refresh)"
            @click="handleReset(formRef)"
          >
            {{ $t("table.refresh") }}
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
                {{ $t("table.view") }}
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
