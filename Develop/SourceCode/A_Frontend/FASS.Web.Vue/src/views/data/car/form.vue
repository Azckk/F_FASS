<script setup lang="ts">
import { ref, onBeforeMount } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { GetDictItemInLocal } from "@/utils/auth";
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

const nodeList = ref([]);
const carTypeList = ref([]);
const CarControlTypeList = ref([]);
const CarProtocolTypeList = ref([]);
const CarAvoidTypeList = ref([]);

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    carTypeId: "",
    prevNodeId: "",
    currNodeId: "",
    nextNodeId: "",
    code: "",
    name: "",
    ipAddress: "",
    port: "",
    type: "",
    manufacturer: "",
    serialNumber: "",
    length: 0,
    width: 0,
    height: 0,
    controlType: "",
    avoidType: "",
    minBattery: "30",
    maxBattery: "80",
    isEnable: true,
    remark: "",
    id: "",
    extend: ""
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

const test = "None";
defineExpose({ getRef });

onBeforeMount(async () => {
  const getNodeListRes = await getNodeList();
  nodeList.value = [...getNodeListRes.data];
  const GetTypeListToSelectRes = await GetTypeListToSelect();
  carTypeList.value = [...GetTypeListToSelectRes.data];
  CarControlTypeList.value = await GetDictItemInLocal("CarControlType");
  console.log("CarAvoidTypeList", CarControlTypeList.value);
  CarProtocolTypeList.value = await GetDictItemInLocal("CarProtocolType");
  CarAvoidTypeList.value = await GetDictItemInLocal("CarAvoidType");
  console.log("CarAvoidTypeList", CarAvoidTypeList.value);
});
</script>
<template>
  <el-form
    ref="ruleFormRef"
    :model="newFormInline"
    :rules="formRules"
    label-width="100px"
  >
    <el-row :gutter="30">
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.carType')" prop="carTypeId">
          <el-select
            v-model="newFormInline.carTypeId"
            filterable
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
        <el-form-item :label="$t('table.frontSite')" prop="prevNodeId">
          <el-select
            v-model="newFormInline.prevNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in nodeList"
              :key="item.id"
              :label="item.code"
              :value="item.id"
            />
          </el-select>
          <!-- <el-input v-model="newFormInline.prevNodeId" clearable placeholder="请选择" /> -->
        </el-form-item>
        <el-form-item :label="$t('table.currentSite')" prop="currNodeId">
          <el-select
            v-model="newFormInline.currNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in nodeList"
              :key="item.id"
              :label="item.code"
              :value="item.id"
            />
          </el-select>
          <!-- <el-input v-model="newFormInline.currNodeId" clearable placeholder="请选择" /> -->
        </el-form-item>
        <el-form-item :label="$t('table.rearSite')" prop="nextNodeId">
          <el-select
            v-model="newFormInline.nextNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in nodeList"
              :key="item.id"
              :label="item.code"
              :value="item.id"
            />
          </el-select>
          <!-- <el-input v-model="newFormInline.nextNodeId" clearable placeholder="请选择" /> -->
        </el-form-item>
        <el-form-item :label="$t('table.code')" prop="code" class="w-100%">
          <el-input
            v-model="newFormInline.code"
            class="w-[calc(100%)]"
            :controls="false"
            :placeholder="$t('table.code')"
          />
        </el-form-item>
        <el-form-item :label="$t('table.name')" prop="name" class="w-100%">
          <el-input
            v-model="newFormInline.name"
            class="w-[calc(100%)]"
            :controls="false"
            :placeholder="$t('table.name')"
          />
        </el-form-item>
        <el-form-item
          :label="$t('table.IPAddress')"
          prop="ipAddress"
          class="w-100%"
        >
          <el-input
            v-model="newFormInline.ipAddress"
            class="w-[calc(100%)]"
            :controls="false"
            :placeholder="$t('table.IPAddress')"
          />
        </el-form-item>
        <el-form-item :label="$t('table.port')" prop="port" class="w-100%">
          <el-input
            v-model="newFormInline.port"
            class="w-[calc(100%)]"
            :controls="false"
            :placeholder="$t('table.port')"
          />
        </el-form-item>
        <el-form-item
          :label="$t('table.agreementType')"
          prop="type"
          class="w-100%"
        >
          <el-select
            v-model="newFormInline.type"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in CarProtocolTypeList"
              :key="item.id"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          :label="$t('table.manufacturer')"
          prop="manufacturer"
          class="w-100%"
        >
          <el-input
            v-model="newFormInline.manufacturer"
            class="w-[calc(100%)]"
            :controls="false"
            :placeholder="$t('table.manufacturer')"
          />
        </el-form-item>
        <el-form-item
          :label="$t('table.serialNumber')"
          prop="serialNumber"
          class="w-100%"
        >
          <el-input
            v-model="newFormInline.serialNumber"
            class="w-[calc(100%)]"
            :controls="false"
            :placeholder="$t('table.serialNumber')"
          />
        </el-form-item>
        <el-form-item
          :label="$t('table.length')"
          prop="length"
          class="!w-[100%]"
        >
          <el-input-number
            v-model="newFormInline.length"
            class="w-[calc(100%)]"
            :min="0"
            :max="99999"
            controls-position="right"
            :placeholder="$t('table.length')"
          />
        </el-form-item>
        <el-form-item :label="$t('table.width')" prop="width" class="!w-[100%]">
          <el-input-number
            v-model="newFormInline.width"
            class="w-[calc(100%)]"
            :min="0"
            :max="99999"
            controls-position="right"
            :placeholder="$t('table.width')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item
          :label="$t('table.height')"
          prop="height"
          class="!w-[100%]"
        >
          <el-input-number
            v-model="newFormInline.height"
            class="w-[calc(100%)]"
            :min="0"
            :max="99999"
            controls-position="right"
            :placeholder="$t('table.height')"
          />
        </el-form-item>
        <el-form-item :label="$t('table.controlType')" prop="controlType">
          <el-select
            v-model="newFormInline.controlType"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <!-- <el-option label="--全部--" value=""/>  -->
            <el-option
              v-for="item in CarControlTypeList"
              :key="item.code"
              :label="`${locale == 'zh' ? item.name : item.code}`"
              :value="item.code"
            />
          </el-select>
          <!-- <el-input v-model="newFormInline.controlType" clearable placeholder="请选择" /> -->
        </el-form-item>
        <el-form-item :label="$t('table.avoidType')" prop="avoidType">
          <el-select
            v-model="newFormInline.avoidType"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <!-- <el-option label="--全部--" value=""/>  -->
            <el-option
              v-for="item in CarAvoidTypeList"
              :key="item.code"
              :label="`${locale == 'zh' ? item.name : item.code}`"
              :value="item.code"
            />
          </el-select>
          <!-- <el-input v-model="newFormInline.avoidType" clearable placeholder="请选择" /> -->
        </el-form-item>
        <el-form-item
          :label="$t('table.minBattery')"
          prop="minBattery"
          class="w-100%"
        >
          <el-input
            v-model="newFormInline.minBattery"
            clearable
            :placeholder="$t('table.pleaseSelect')"
          />
        </el-form-item>
        <el-form-item
          :label="$t('table.maxBattery')"
          prop="maxBattery"
          class="w-100%"
        >
          <el-input
            v-model="newFormInline.maxBattery"
            clearable
            :placeholder="$t('table.pleaseSelect')"
          />
        </el-form-item>
        <el-form-item
          :label="$t('table.effectiveness')"
          prop="isEnable"
          class="w-100%"
        >
          <el-checkbox
            v-model="newFormInline.isEnable"
            :label="$t('table.isEnable')"
            size="large"
          />
        </el-form-item>
        <el-form-item :label="$t('table.remark')" prop="remark">
          <el-input
            v-model="newFormInline.remark"
            :placeholder="$t('table.remark')"
            :autosize="{ minRows: 5, maxRows: 8 }"
            type="textarea"
          />
        </el-form-item>
        <el-form-item :label="$t('table.extend')" prop="extend">
          <el-input
            v-model="newFormInline.extend"
            :placeholder="$t('table.extend')"
            :autosize="{ minRows: 5, maxRows: 8 }"
            type="textarea"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
