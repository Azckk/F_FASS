<script setup lang="ts">
import { ref } from "vue";
import { useHook } from "./utils/hook";

defineOptions({
  name: "Role-Permission"
});

const props = defineProps({ roleId: { type: String, default: "" } });
const keyValue = ref(props.roleId);

const treeRef = ref();

function Update() {
  handleUpdate();
}

defineExpose({ Update });

const { query, treeData, treeProps, handleSearch, handleFilter, handleUpdate } =
  useHook(treeRef, keyValue);
</script>

<template>
  <div class="main">
    <el-input
      v-model="query"
      placeholder="请输入菜单进行搜索"
      class="mb-1"
      clearable
      @input="handleSearch"
    />
    <el-scrollbar height="80vh">
      <el-tree
        ref="treeRef"
        show-checkbox
        node-key="id"
        :data="treeData"
        :props="treeProps"
        :default-expand-all="true"
        :filter-node-method="handleFilter"
      >
        <template #default="{ node }">
          <span>{{ node.label }}</span>
        </template>
      </el-tree>
    </el-scrollbar>
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
