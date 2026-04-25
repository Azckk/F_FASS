<script lang="ts" setup>
import { ref } from "vue";
import { Menu as IconMenu, Message, Setting } from "@element-plus/icons-vue";
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import { useHook } from "./utils/hook";
import View from "@iconify-icons/ep/view";
import Plus from "@iconify-icons/ep/plus";
import Edit from "@iconify-icons/ep/edit";

const formRef = ref();
const tableRef = ref();
const statustableRef = ref();
// const activeIndex = ref("0");

const handleMenuSelect = (index: string) => {
  activeId.value = index;
  // console.log("当前激活的索引为:", index);
};

const {
  leftList,
  statusData,
  fieldData,
  methodsData,
  methodsColumns,
  columns,
  statusColumns,
  handleExecute,
  changeData,
  activeId,
  handleAdd,
  handleUpdate,
  handleDelete,
  handleSelection,
  handleSearch
} = useHook(tableRef);

const radio = ref();
const collapseRef = ref();
const activeNames = ref(["1", "2", "3"]);

// const tableData = ref(Array.from({ length: 20 }).fill(item));
</script>

<template>
  <el-container class="layout-container-demo" style="height: 82vh">
    <el-aside width="350px">
      <el-scrollbar>
        <div class="left-title">进程类型列表</div>
        <el-menu :default-active="activeId" @select="handleMenuSelect">
          <el-menu-item
            v-for="item in leftList"
            :key="item.id"
            :index="item.id"
            @click="changeData(item.id)"
          >
            <div class="flex">
              <el-tooltip
                effect="dark"
                :content="item.typeName"
                placement="top-start"
              >
                <div style="width: 50%" class="over-text">
                  {{ item.typeName }}
                </div>
              </el-tooltip>
              <el-tooltip
                effect="dark"
                :content="item.state"
                placement="top-start"
              >
                <div style="width: 40%" class="over-text">
                  {{ item.state }}
                </div>
              </el-tooltip>

              <!-- <div style="width: 40%">{{ item.state }}</div> -->
            </div>
          </el-menu-item>
        </el-menu>
      </el-scrollbar>
    </el-aside>

    <el-container>
      <el-scrollbar class="main-right">
        <el-collapse ref="collapseRef" v-model="activeNames">
          <el-collapse-item name="1">
            <template #title>
              <div class="flex padding-lr">
                <div class="collapse-title">动作</div>
                <div>{{ activeNames.includes("1") ? "收起" : "展开" }}</div>
              </div>
            </template>
            <pure-table
              ref="statustableRef"
              row-key="statusid"
              :data="methodsData"
              :columns="methodsColumns"
              :header-cell-style="{
                background: 'var(--el-fill-color-light)',
                color: 'var(--el-text-color-primary)'
              }"
            >
              <template #operation="{ row }">
                <el-button color="#5a3092" @click="handleExecute([row])">
                  {{ $t("buttons.execute") }}
                </el-button>
              </template>
            </pure-table>
          </el-collapse-item>
          <el-collapse-item name="2">
            <template #title>
              <div class="flex padding-lr">
                <div class="collapse-title">属性</div>
                <div>{{ activeNames.includes("2") ? "收起" : "展开" }}</div>
              </div>
            </template>

            <PureTableBar title="" :columns="columns" @refresh="handleSearch">
              <template #buttons>
                <el-button
                  type="primary"
                  color="#5a3092"
                  :icon="useRenderIcon(Plus)"
                  @click="handleAdd()"
                >
                  {{ $t("buttons.add") }}
                </el-button>
                <!-- <el-button
                  type="primary"
                  :icon="useRenderIcon(Edit)"
                  @click="handleUpdate()"
                >
                  {{ $t("buttons.edit") }}
                </el-button> -->
              </template>
              <template v-slot="{ dynamicColumns }">
                <pure-table
                  ref="tableRef"
                  row-key="id"
                  :data="fieldData"
                  :columns="dynamicColumns"
                  :header-cell-style="{
                    background: 'var(--el-fill-color-light)',
                    color: 'var(--el-text-color-primary)'
                  }"
                  @selection-change="handleSelection"
                >
                  <template #operation="{ row }">
                    <el-button color="#5a3092" @click="handleUpdate([row])">
                      {{ $t("buttons.edit") }}
                    </el-button>
                    <el-button color="#5a3092" @click="handleDelete([row])">
                      {{ $t("buttons.delete") }}
                    </el-button>
                  </template>
                </pure-table>
              </template>
            </PureTableBar>
          </el-collapse-item>
          <el-collapse-item name="3">
            <template #title>
              <div class="flex padding-lr">
                <div class="collapse-title">状态</div>
                <div>{{ activeNames.includes("3") ? "收起" : "展开" }}</div>
              </div>
            </template>

            <pure-table
              ref="statustableRef"
              row-key="statusid"
              :data="statusData"
              :columns="statusColumns"
              :header-cell-style="{
                background: 'var(--el-fill-color-light)',
                color: 'var(--el-text-color-primary)'
              }"
            />
          </el-collapse-item>
        </el-collapse>
      </el-scrollbar>
    </el-container>
  </el-container>
</template>

<style scoped>
.over-text {
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
}

.left-title {
  padding: 18px;
  font-weight: 600;
}
.flex {
  display: flex;
  width: 100%;
  justify-content: space-between;
}
.padding-lr {
  padding: 0 15px;
}
.collapse-title {
  font-size: 16px;
  font-weight: 600;
}
.layout-container-demo .el-header {
  position: relative;
  background-color: #fff;
  color: var(--el-text-color-primary);
}
.layout-container-demo .el-aside {
  color: var(--el-text-color-primary);
  background: #fff;
}
.layout-container-demo .el-menu {
  border-right: none;
}
.layout-container-demo .el-main {
  padding: 0;
}
.layout-container-demo .toolbar {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  right: 20px;
}
.layout-theme-saucePurple
  body[layout="vertical"]
  .el-menu--vertical
  .el-menu-item,
.layout-theme-saucePurple
  body[layout="vertical"]
  .el-menu--vertical
  .el-sub-menu__title {
  background-color: #fff;
  color: #000;
}

.layout-theme-saucePurple
  body[layout="vertical"]
  .el-menu--vertical
  .el-menu-item,
.layout-theme-saucePurple
  body[layout="vertical"]
  .el-menu--vertical
  .el-sub-menu__title {
  /* color: #2d91fc !important; */
  color: #000 !important;
  /* background-color: #34064d; */
}

.el-menu-item:hover {
  background-color: #e6f7ff !important;
  color: #2d91fc !important;
}
:deep(.el-collapse-item__header) {
  border: none !important;
}
.main-right {
  width: 100%;
  margin-left: 20px;
  margin-right: 15px;
  height: 82vh;
  overflow-y: auto;
}

.el-menu-item.is-active {
  background-color: #e6f7ff !important;
  color: #2d91fc !important;
  font-weight: bold;
}
</style>
