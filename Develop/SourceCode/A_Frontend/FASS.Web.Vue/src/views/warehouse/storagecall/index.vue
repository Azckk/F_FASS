<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter, useRoute } from "vue-router";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
const router = useRouter();
import { PureTableBar } from "@/components/RePureTableBar";
import { useHook } from "./utils/hook";
import Plus from "@iconify-icons/ep/plus";
import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";
import Delete from "@iconify-icons/ep/delete";
const tableRef = ref();
defineExpose({ tableRef });
const formRef = ref();
const {
  query,
  loading,
  columns,
  dataList,
  pagination,
  deviceDetection,
  handlePageSize,
  handlePageCurrent,
  handleSearch,
  handleAdd,
  handleReset,
  shortcuts,
  TaskInstanceStateList,
  handleDelete,
  handleSelection,
  handleDeleteM3,
  handleDeleteM1,
  handleDeleteW1,
  handleDeleteD1,
  handleDeleteAll
} = useHook(tableRef);
onMounted(async () => {
  handleSearch();
});
</script>

<template>
  <div :class="['flex', 'justify-between', deviceDetection() && 'flex-wrap']">
    <div :class="[deviceDetection() ? ['w-full', 'mt-2'] : 'w-[calc(100%)]']">
      <div :class="[deviceDetection() ? ['w-full', 'mt-2'] : 'w-[calc(100%)]']">
        <el-form
          ref="formRef"
          :inline="true"
          :model="query"
          class="search-form bg-bg_color w-[99/100] pl-8 pt-[12px] overflow-auto"
        >
          <el-form-item :label="$t('table.code')" prop="code">
            <el-input
              v-model="query.code"
              :placeholder="$t('table.code')"
              clearable
              class="!w-[200px]"
            />
          </el-form-item>

          <el-form-item :label="$t('table.taskStatus')" prop="state">
            <el-select
              v-model="query.state"
              filterable
              class="!w-[200px]"
              :placeholder="$t('table.pleaseSelect')"
            >
              <el-option label="--全部--" value="" />
              <el-option
                v-for="item in TaskInstanceStateList"
                :key="item.id"
                :label="item.name"
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
              :icon="useRenderIcon(Plus)"
              @click="handleAdd()"
            >
              {{ $t("buttons.add") }}
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
              <!-- <template #operation="{ row }">
                <el-button
                  link
                  type="primary"
                  :size="size"
                  :icon="useRenderIcon(Delete)"
                  @click="handleDelete([row])"
                >
                  删除
                </el-button>
              </template> -->
            </pure-table>
          </template>
        </PureTableBar>
      </div>
    </div>
  </div>
</template>

<style scoped>
.menuItem {
  margin: 0 !important;
}
</style>
