<script setup lang="ts">
import { ref } from "vue";
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import { useHook } from "./utils/hook";
import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";
import View from "@iconify-icons/ep/view";
import Plus from "@iconify-icons/ep/plus";
import Delete from "@iconify-icons/ep/delete";
import Edit from "@iconify-icons/ep/edit";
import { FormProps } from "../actionparameterform/form.vue";
defineOptions({
  name: "Role-User"
});
const props = withDefaults(defineProps<{
  formInline: FormProps[];
}>(), {
  formInline: () => ([
    {
      key: "",
      value: "",
    }
  ])
});
const newFormInline = ref(props.formInline);
console.log(" newFormInline is " , newFormInline)
// const props = defineProps({ roleId: { type: String, default: "" } });
// const keyValue = ref(props.roleId);

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
  handleAdd,
  handleUpdate,
  handleDelete
} = useHook(newFormInline);
</script>

<template>
  <div class="main">
    <!-- <el-form
      ref="formRef"
      :inline="true"
      :model="query"
      class="search-form bg-bg_color w-[99/100] pl-8 pt-[12px]"
    >
      <el-form-item label="键" prop="key">
        <el-input
          v-model="query.key"
          placeholder="键"
          clearable
          class="!w-[200px]"
        />
      </el-form-item>
      <el-form-item label="值" prop="value">
        <el-input
          v-model="query.value"
          placeholder="值"
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
          搜索
        </el-button>
        <el-button :icon="useRenderIcon(Refresh)" @click="handleReset(formRef)">
          重置
        </el-button>
      </el-form-item>
    </el-form> -->
    <PureTableBar title="列表" :columns="columns" @refresh="handleSearch">
      <template #buttons>
        <el-button
          type="primary"
          :icon="useRenderIcon(View)"
          @click="handleDetail()"
        >
          查看
        </el-button>
        <el-button
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
            <el-button
              link
              type="primary"
              :size="size"
              :icon="useRenderIcon(Delete)"
              @click="handleDelete([row])"
            >
              删除
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
