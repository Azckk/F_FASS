<script setup lang="ts">
import { ref } from "vue";
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import { useHook } from "./utils/hook";
// import { useHook } from "../sendwork/utils/hook";
import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";
import View from "@iconify-icons/ep/view";
import Plus from "@iconify-icons/ep/plus";
import Edit from "@iconify-icons/ep/edit";
import Delete from "@iconify-icons/ep/delete";
import Check from "@iconify-icons/ep/check";
import Close from "@iconify-icons/ep/close";
import User from "@iconify-icons/ep/user";
import { watch } from "fs";
import { useCounter } from "@vueuse/core";
import { useFormStateStore } from "@/store/from";
import empty from "@/assets/visualizationtask/empty.png";
import emptyUp from "@/assets/visualizationtask/emptyUp.png";
import fill from "@/assets/visualizationtask/fill.png";
import fillUp from "@/assets/visualizationtask/fillUp.png";
const formStateStore = useFormStateStore();

defineOptions({
  name: "settings-destinationORstart"
});

const props = defineProps({
  roleId: { type: String, default: "" },
  handleSendWorkFns: { type: ref, default: "" }
});
// const formRefs = ref();
const tableRef = ref();
const keyValue = ref(props.roleId);
const isChecked = ref();

// const { settingDestination, settingStart, test } = useHook();

const settingDestination = () => {
  isChecked.value = "end";
  formStateStore.setDestStorageId(keyValue.value);
  // formStateStore.setSrcStorageId("");
};
const settingStart = () => {
  isChecked.value = "start";
  formStateStore.setSrcStorageId(keyValue.value);
  // formStateStore.setDestStorageId("");
};

const setCallId = () => {
  isChecked.value = "start1";
  formStateStore.setCallId(keyValue.value);
  // formStateStore.setDestStorageId("");
};
</script>

<template>
  <!-- <el-row :gutter="20"> -->
  <div v-if="formStateStore.siteStatus === 0">
    <div class="imgBtn">
      <img src="../../../../assets/visualizationtask/start.png" alt="" />
      <el-button
        type="info"
        :class="isChecked === 'start' ? 'checked' : ''"
        @click="settingStart"
        >设置起点</el-button
      >
    </div>
    <div class="imgBtn">
      <img src="../../../../assets/visualizationtask/end.png" alt="" />
      <el-button
        type="info"
        :class="isChecked === 'end' ? 'checked' : ''"
        @click="settingDestination"
        >设置终点</el-button
      >
    </div>
  </div>

  <div v-else>
    <div class="imgBtn">
      <img src="../../../../assets/visualizationtask/call.png" alt="" />
      <el-button
        type="info"
        :class="isChecked === 'start1' ? 'checked' : ''"
        @click="setCallId"
        >设为呼叫库位</el-button
      >
    </div>
    <div class="imgBtn">
      <img :src="formStateStore.siteStatus === 1 ? fillUp : fill" alt="" />
      <el-button
        type="info"
        :class="isChecked === 'start' ? 'checked' : ''"
        @click="settingStart"
      >
        {{ formStateStore.siteStatus === 1 ? "设为放满库位" : "设为放空库位" }}
      </el-button>
    </div>

    <div class="imgBtn">
      <img :src="formStateStore.siteStatus === 1 ? empty : emptyUp" alt="" />
      <el-button
        type="info"
        :class="isChecked === 'end' ? 'checked' : ''"
        @click="settingDestination"
      >
        {{ formStateStore.siteStatus === 1 ? "设为取空库位" : "设为取满库位" }}
      </el-button>
    </div>
  </div>

  <!-- </el-row> -->
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
.imgBtn {
  display: flex;
  align-items: center;
  padding: 10px 0;
}
.imgBtn .el-button {
  width: 95px;
  height: 30px;
  border-radius: 2px;
  border: 1px solid #acacac;
  background: #ffffff;
  color: #333333;
  font-family: Microsoft YaHei;
  margin-left: 20px;
  // border: 1px solid #acacac;
}
.imgBtn .el-button:hover {
  color: #eee;
  background-color: #7710d6; /* 鼠标悬停时的背景颜色 */
}
.imgBtn .checked {
  color: #eee;
  background-color: #7710d6; /* 鼠标悬停时的背景颜色 */
}
</style>
