<script setup lang="ts">
import { ref, reactive } from "vue";
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
import { formRules } from "./utils/rule";
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
  carTypeList,
  dataList,
  pagination,
  nodeListData,
  chargeNodeListData,
  dialogVisible,
  title,
  // sendData,
  deviceDetection,
  handleSelection,
  handlePageSize,
  handlePageCurrent,
  handleSearch,
  handleReset,
  handleDetail,
  handleClick,
  sendGoWhere,
  TaskRecordStateList
} = useHook(formRef);
const formLabelAlign = reactive({
  targetNodeId: ""
});
</script>

<template>
  <div>
    <div :class="['flex', 'justify-between', deviceDetection() && 'flex-wrap']">
      <div :class="[deviceDetection() ? ['w-full', 'mt-2'] : 'w-[calc(100%)]']">
        <el-form
          ref="formRef"
          :inline="true"
          :model="query"
          class="search-form bg-bg_color w-[99/100] pl-8 pt-[12px] overflow-auto"
        >
          <!-- <el-form-item :label="$t('table.carType')" prop="carTypeId">
            <el-select
              v-model="query.carTypeId"
              filterable
              class="!w-[200px]"
              :placeholder="$t('table.pleaseSelect')"
            >
              <el-option label="--全部--" value="" />
              <el-option
                v-for="item in carTypeList"
                :key="item.id"
                :label="item.name"
                :value="item.id"
              />
            </el-select>
          </el-form-item> -->
          <el-form-item :label="$t('table.name')" prop="name">
            <el-input
              v-model="query.name"
              :placeholder="$t('table.name')"
              clearable
              class="!w-[200px]"
            />
          </el-form-item>
          <el-form-item :label="$t('table.code')" prop="code">
            <el-input
              v-model="query.code"
              :placeholder="$t('table.code')"
              clearable
              class="!w-[200px]"
            />
          </el-form-item>
          <el-form-item :label="$t('table.carStatus')" prop="CurrState">
            <el-select
              v-model="query.CurrState"
              filterable
              class="!w-[200px]"
              :placeholder="$t('table.pleaseSelect')"
            >
              <el-option :label="$t('table.doNotSpecify')" value="" />
              <el-option
                v-for="item in TaskRecordStateList"
                :key="item.code"
                :label="item.name"
                :value="item.code"
              />
            </el-select>
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
              data-action="Restart"
              style="width: 60px; margin-right: 10px"
              @click="handleDetail()"
              >{{ $t("buttons.view") }}</el-button
            >
            <div class="operationBtnClick" @click="handleClick">
              <el-button
                type="warning"
                data-action="initialization"
                style="width: 90px"
                >{{ $t("buttons.initialization") }}</el-button
              >
              <el-button
                type="warning"
                data-action="endTask"
                style="width: 90px"
                >{{ $t("buttons.cancelTask") }}</el-button
              >
              <el-button
                type="warning"
                data-action="stopTask"
                style="width: 100px"
                >{{ $t("buttons.Stop/Recovery") }}</el-button
              >
              <el-button
                type="warning"
                data-action="completelyOffline"
                style="width: 90px"
                >{{ $t("buttons.completelyOffline") }}</el-button
              >
              <el-button
                type="warning"
                data-action="goStartPoint"
                style="width: 90px"
                >{{ $t("buttons.goStartPoint") }}</el-button
              >
              <el-button
                type="warning"
                data-action="goCharging"
                style="width: 90px"
                >{{ $t("buttons.goCharging") }}</el-button
              >
              <el-button
                type="warning"
                data-action="goWhere"
                style="width: 90px"
                >{{ $t("buttons.goWhere") }}</el-button
              >
              <el-button
                type="warning"
                data-action="Pause/Resume"
                style="width: 90px"
                >{{ $t("buttons.Pause/Resume") }}</el-button
              >
              <el-button
                type="warning"
                data-action="Restart"
                style="width: 90px"
                >{{ $t("buttons.restart") }}</el-button
              >
            </div>
          </template>
          <template v-slot="{ size, dynamicColumns }">
            <pure-table
              ref="tableRef"
              row-key="id"
              align-whole="center"
              adaptive
              showOverflowTooltip
              default-expand-all
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
    <el-dialog
      v-model="dialogVisible"
      :title="title"
      width="350"
      :append-to-body="true"
      :destroy-on-close="true"
    >
      <el-form
        v-if="title == '去某地' || title == 'goWhere'"
        ref="formRef"
        :rules="formRules"
        :model="formLabelAlign"
        label-width="auto"
      >
        <el-form-item :label="$t('table.site')" prop="targetNodeId">
          <el-select
            v-model="formLabelAlign.targetNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelectASiteToGo')"
          >
            <el-option
              v-for="item in nodeListData"
              :key="item.id"
              :label="`${item.name}(${item.code})`"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </el-form>
      <!-- <el-form
        v-if="title == '去充电' || title == 'goCharging'"
        ref="formRef"
        :rules="formRules"
        :model="formLabelAlign"
      >
        <el-form-item :label="$t('table.site')" prop="targetNodeId">
          <el-select
            v-model="formLabelAlign.targetNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelectAChargingStation')"
          >
            <el-option
              v-for="item in chargeNodeListData"
              :key="item.id"
              :label="`${item.name}(${item.code})`"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </el-form> -->
      <template #footer>
        <div class="dialog-footer">
          <el-button
            @click="
              dialogVisible = false;
              formLabelAlign.targetNodeId = '';
            "
            >{{ $t("buttons.cancel") }}</el-button
          >
          <el-button type="primary" @click="sendGoWhere(formLabelAlign)">
            {{ $t("buttons.ok") }}
          </el-button>
          <!-- <el-button
            v-if="title == '去充电' || title == 'goCharging'"
            type="primary"
            @click="sendGoWhere"
          >
            {{ $t("buttons.ok") }}
          </el-button> -->
        </div>
      </template>
    </el-dialog>
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
