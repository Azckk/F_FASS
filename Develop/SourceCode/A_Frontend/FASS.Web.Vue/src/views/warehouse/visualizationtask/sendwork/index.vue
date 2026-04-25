<script setup lang="ts">
import { reactive, ref, watchEffect, watch, onBeforeMount } from "vue";
import { useHook } from "./utils/hook";
import { formRules } from "./utils/rule";
import { useFormStateStore } from "@/store/from";

import {
  GetTypeListToSelect,
  getPage,
  getNodeList,
  addOrUpdate,
  deletes,
  enable,
  disable
} from "@/api/data/car";
import { useMyI18n } from "@/plugins/i18n";
const { locale } = useMyI18n();

const formStateStore = useFormStateStore(); //引入pinia中的状态

// const props = defineProps(["handleSendWorkFns"]);
const {
  formRef,
  CommonCallMode,
  nodeDataList,
  carDataList,
  LogisticsRouteDataList,
  areaDataList,
  handleSendWorkFn,
  handleCarChange
} = useHook();
const style1 = {
  background: "blue"
};
const style2 = {
  background: "red"
};
const carTypeList = ref([]);
const formLabelAlign = reactive({
  taskTemplateId: "",
  carId: "",
  carTypeId: "",
  code: "",
  type: "",
  callStorageId: "",
  srcStorageId: "",
  destStorageId: "",
  srcAreaId: "",
  destAreaId: ""
});
// const codePage = ref("123");
const switchSelect = ref(true);
const changLable = ref(0);
const filteredCarDataList = ref([]);
defineOptions({
  name: ""
});

onBeforeMount(async () => {
  // const getNodeListRes = await getNodeList();
  // nodeList.value = [...getNodeListRes.data];
  const GetTypeListToSelectRes = await GetTypeListToSelect();
  carTypeList.value = [...GetTypeListToSelectRes.data];
  if (carTypeList.value) {
    formLabelAlign.carTypeId = carTypeList.value[0].id;
  }
});

// 监听 carDataList 的变化，确保在其加载完成后进行操作
watchEffect(() => {
  if (carDataList.value && carDataList.value.length > 0) {
    //首次筛选默认车辆类型
    filteredCarDataList.value = carDataList.value.filter(item => {
      return item.carTypeId == formLabelAlign.carTypeId;
    });
  }
});

//监听车辆类型筛选对应类型的车辆
watch(
  () => formLabelAlign.carTypeId,
  newValue => {
    formLabelAlign.carId = ""; //切换类型清空当前选择的车辆
    if (Array.isArray(carDataList.value) && carDataList.value !== undefined) {
      filteredCarDataList.value = carDataList.value.filter(item => {
        return item.carTypeId == newValue;
      });
    }
  }
);

//监听状态发生改变修改表单的值
watch(
  () => formStateStore.srcStorageId,
  newValue => {
    formLabelAlign.srcStorageId = newValue;
  }
);
watch(
  () => formStateStore.destStorageId,
  newValue => {
    formLabelAlign.destStorageId = newValue;
  }
);
watch(
  () => formStateStore.callId,
  newValue => {
    formLabelAlign.callStorageId = newValue;
  }
);

watch(
  () => formLabelAlign.taskTemplateId,
  newValue => {
    switch (newValue) {
      case "EmptyOffline":
      case "FullOnline":
        switchSelect.value = true; //
        changLable.value = 0;
        formStateStore.setSiteStatus(0);
        break;
      case "FullEmptyExchange":
        switchSelect.value = false; // 页面变化
        changLable.value = 1;
        formStateStore.setSiteStatus(1);
        break;
      case "EmptyFullExchange":
        switchSelect.value = false; // 页面变化
        changLable.value = 2;
        formStateStore.setSiteStatus(2);
        break;
      default:
        switchSelect.value = true; //
        changLable.value = 0;
    }
  }
);
function handleChange() {
  // formLabelAlign.carId = null;
  // formLabelAlign.carTypeId = null;
  // formLabelAlign.code = "";
  // formLabelAlign.type = null;
  // formLabelAlign.callStorageId = null;
  // formLabelAlign.srcStorageId = null;
  // formLabelAlign.destStorageId = null;
  // formLabelAlign.srcAreaId = null;
  // formLabelAlign.destAreaId = null;
  initForm();
}
function initForm() {
  // formLabelAlign.carId = null;
  // formLabelAlign.carTypeId = null;
  formLabelAlign.code = null;
  formLabelAlign.type = null;
  formLabelAlign.callStorageId = null;
  formLabelAlign.srcStorageId = null;
  formLabelAlign.destStorageId = null;
  formLabelAlign.srcAreaId = null;
  formLabelAlign.destAreaId = null;
  formStateStore.srcStorageId = null;
  formStateStore.destStorageId = null;
  formStateStore.callId = null;
}
</script>

<template>
  <div class="container">
    <h5>{{ $t("table.sendWork") }}</h5>
    <div style="margin: 20px" />
    <div>
      <el-form
        ref="formRef"
        label-width="auto"
        :model="formLabelAlign"
        :rules="formRules"
        label-position="top"
        style="max-width: 120px"
      >
        <el-form-item :label="$t('table.templateType')" prop="taskTemplateId">
          <el-select
            v-model="formLabelAlign.taskTemplateId"
            :placeholder="$t('table.pleaseSelect')"
            clearable
            @change="handleChange"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in CommonCallMode"
              :key="item.id"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>

        <el-form-item
          v-if="!switchSelect"
          :label="$t('呼叫库位')"
          prop="callStorageId"
        >
          <el-select
            v-model="formLabelAlign.callStorageId"
            :placeholder="$t('table.pleaseSelect')"
            clearable
            filterable
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in areaDataList"
              :key="item.id"
              :label="`${item.name} (${item.code})`"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          :label="
            changLable === 1
              ? '放满库位'
              : changLable === 2
                ? '放空库位'
                : $t('table.taskStartingPoint')
          "
          prop="srcStorageId"
        >
          <el-select
            v-model="formLabelAlign.srcStorageId"
            :placeholder="$t('table.pleaseSelect')"
            clearable
            filterable
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in areaDataList"
              :key="item.id"
              :label="`${item.name} (${item.code})`"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          :label="
            changLable === 1
              ? ' 取空库位'
              : changLable === 2
                ? '取满库位'
                : $t('table.taskEndingPoint')
          "
          prop="destStorageId"
        >
          <el-select
            v-model="formLabelAlign.destStorageId"
            :placeholder="$t('table.pleaseSelect')"
            clearable
            filterable
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in areaDataList"
              :key="item.id"
              :label="`${item.name} (${item.code})`"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.carType')" prop="carTypeId">
          <el-select
            v-model="formLabelAlign.carTypeId"
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in carTypeList"
              :key="item.id"
              :label="`${locale == 'zh' ? item.name : item.code}`"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.designatedVehicle')" prop="carId">
          <el-select
            v-model="formLabelAlign.carId"
            :placeholder="$t('table.pleaseSelect')"
            clearable
            filterable
            @change="handleCarChange"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in filteredCarDataList"
              :key="item.id"
              :label="`${item.code}(${item.name})`"
              :value="item.id"
              :data-carTypeId="item.carTypeId"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          ><div class="sendWorkBtn">
            <el-button
              type="primary"
              style="width: 120px"
              @click="handleSendWorkFn(formLabelAlign)"
              >{{ $t("buttons.ok") }}</el-button
            >
          </div>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.flex-grow {
  flex-grow: 1;
}
.test {
  width: 120px;
  height: 120px;
  border-radius: 100px;
  text-align: center;
  line-height: 120px;
  color: #fff;
  font-size: 20px;
  overflow: hidden;
}
</style>
