<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { useHook } from "./utils/hook";
// import { usePublicHooks } from "../hooks";
// const { switchStyle } = usePublicHooks();

const selectGenderList = ref([]);
const selectAvatarList = ref([]);

const {
  query,
  loading,
  columns,
  nodeList,
  carTypeList,
  CarControlTypeList,
  CarAvoidTypeList
} = useHook();

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
    extend: "",
    isNormal: "",
    isOnline: "",
    speed: "",
    angle: "",
    X: "",
    Y: "",
    electricity: ""
  })
});
const newFormInline = ref(props.formInline);
// console.log( " CarControlTypeList  is"  , CarControlTypeList)

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });
</script>
<template>
  <el-form
    ref="ruleFormRef"
    :model="newFormInline"
    :rules="formRules"
    label-width="100px"
  >
    <el-row :gutter="20">
      <el-col :span="8"
        ><div class="grid-content ep-bg-purple" />
        <el-form-item :label="$t('table.carType')" prop="carTypeId">
          <el-select
            v-model="newFormInline.carTypeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in carTypeList"
              :label="item.name"
              :value="item.id"
              :key="item.id"
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
              :label="item.code"
              :value="item.id"
              :key="item.id"
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
              :label="item.code"
              :value="item.id"
              :key="item.id"
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
              :label="item.code"
              :value="item.id"
              :key="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.startSite')" prop="startEdgeId">
          <el-select
            v-model="newFormInline.nextNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <!-- <el-option
              v-for="item in nodeList"
              :label="item.code"
              :value="item.id"
              :key="item.id"
            /> -->
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.endSite')" prop="endEdgeId">
          <el-select
            v-model="newFormInline.nextNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <!-- <el-option
              v-for="item in nodeList"
              :label="item.code"
              :value="item.id"
              :key="item.id"
            /> -->
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.frontRoute')" prop="frontRoute">
          <el-select
            v-model="newFormInline.nextNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <!-- <el-option
              v-for="item in nodeList"
              :label="item.code"
              :value="item.id"
              :key="item.id"
            /> -->
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.currentRoute')" prop="currentRoute">
          <el-select
            v-model="newFormInline.nextNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <!-- <el-option
              v-for="item in nodeList"
              :label="item.code"
              :value="item.id"
              :key="item.id"
            /> -->
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.rearRoute')" prop="rearRoute">
          <el-select
            v-model="newFormInline.nextNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <!-- <el-option
              v-for="item in nodeList"
              :label="item.code"
              :value="item.id"
              :key="item.id"
            /> -->
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.startRoute')" prop="startRoute">
          <el-select
            v-model="newFormInline.nextNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <!-- <el-option
              v-for="item in nodeList"
              :label="item.code"
              :value="item.id"
              :key="item.id"
            /> -->
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.endRoute')" prop="endRoute">
          <el-select
            v-model="newFormInline.nextNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <!-- <el-option
              v-for="item in nodeList"
              :label="item.code"
              :value="item.id"
              :key="item.id"
            /> -->
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.code')" prop="code" class="w-100%">
          <el-input
            class="w-[calc(100%)]"
            :controls="false"
            v-model="newFormInline.code"
            :placeholder="$t('table.code')"
          />
        </el-form-item>
        <el-form-item :label="$t('table.name')" prop="name" class="w-100%">
          <el-input
            class="w-[calc(100%)]"
            :controls="false"
            v-model="newFormInline.name"
            :placeholder="$t('table.name')"
          />
        </el-form-item>
      </el-col>
      <el-col :span="8"
        ><div class="grid-content ep-bg-purple" />

        <el-form-item
          :label="$t('table.IPAddress')"
          prop="ipAddress"
          class="w-100%"
        >
          <el-input
            class="w-[calc(100%)]"
            :controls="false"
            v-model="newFormInline.ipAddress"
            :placeholder="$t('table.IPAddress')"
          />
        </el-form-item>
        <el-form-item :label="$t('table.port')" prop="port" class="w-100%">
          <el-input
            class="w-[calc(100%)]"
            :controls="false"
            v-model="newFormInline.port"
            :placeholder="$t('table.port')"
          />
        </el-form-item>
        <el-form-item :label="$t('table.type')" prop="type" class="w-100%">
          <el-input
            class="w-[calc(100%)]"
            :controls="false"
            v-model="newFormInline.type"
            :placeholder="$t('table.type')"
          />
        </el-form-item>
        <el-form-item
          :label="$t('table.manufacturer')"
          prop="manufacturer"
          class="w-100%"
        >
          <el-input
            class="w-[calc(100%)]"
            :controls="false"
            v-model="newFormInline.manufacturer"
            :placeholder="$t('table.manufacturer')"
          />
        </el-form-item>
        <el-form-item
          :label="$t('table.serialNumber')"
          prop="serialNumber"
          class="w-100%"
        >
          <el-input
            class="w-[calc(100%)]"
            :controls="false"
            v-model="newFormInline.serialNumber"
            :placeholder="$t('table.serialNumber')"
          />
        </el-form-item>
        <el-form-item
          :label="$t('table.length')"
          prop="length"
          class="!w-[100%]"
        >
          <el-input-number
            class="w-[calc(100%)]"
            :min="0"
            :max="99999"
            controls-position="right"
            v-model="newFormInline.length"
            :placeholder="$t('table.length')"
          />
        </el-form-item>
        <el-form-item :label="$t('table.width')" prop="width" class="!w-[100%]">
          <el-input-number
            class="w-[calc(100%)]"
            :min="0"
            :max="99999"
            controls-position="right"
            v-model="newFormInline.width"
            :placeholder="$t('table.width')"
          />
        </el-form-item>
        <el-form-item
          :label="$t('table.height')"
          prop="height"
          class="!w-[100%]"
        >
          <el-input-number
            class="w-[calc(100%)]"
            :min="0"
            :max="99999"
            controls-position="right"
            v-model="newFormInline.height"
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
              :label="item.name"
              :value="item.id"
              :key="item.id"
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
              :label="item.name"
              :value="item.id"
              :key="item.id"
            />
          </el-select>
          <!-- <el-input v-model="newFormInline.avoidType" clearable placeholder="请选择" /> -->
        </el-form-item>
        <el-form-item :label="$t('table.preState')" prop="preState">
          <el-select
            v-model="newFormInline.avoidType"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <!-- <el-option
              v-for="item in CarAvoidTypeList"
              :label="item.name"
              :value="item.id"
              :key="item.id"
            /> -->
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.postState')" prop="postState">
          <el-select
            v-model="newFormInline.avoidType"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <!-- <el-option
              v-for="item in CarAvoidTypeList"
              :label="item.name"
              :value="item.id"
              :key="item.id"
            /> -->
          </el-select>
        </el-form-item>
      </el-col>
      <el-col :span="8"
        ><div class="grid-content ep-bg-purple" />
        <el-form-item
          :label="$t('table.electricity')"
          prop="minBattery"
          class="w-100%"
        >
          <el-input
            v-model="newFormInline.electricity"
            clearable
            :placeholder="$t('table.electricity')"
          />
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
          :label="$t('table.coordinatesX')"
          prop="coordinatesX"
          class="w-100%"
        >
          <el-input
            v-model="newFormInline.X"
            clearable
            :placeholder="$t('table.coordinatesX')"
          />
        </el-form-item>
        <el-form-item :label="$t('table.coordinatesY')" prop="Y" class="w-100%">
          <el-input
            v-model="newFormInline.Y"
            clearable
            :placeholder="$t('table.coordinatesY')"
          />
        </el-form-item>
        <el-form-item :label="$t('table.angle')" prop="angle" class="w-100%">
          <el-input
            v-model="newFormInline.angle"
            clearable
            :placeholder="$t('table.angle')"
          />
        </el-form-item>
        <el-form-item :label="$t('table.speed')" prop="speed" class="w-100%">
          <el-input
            v-model="newFormInline.speed"
            clearable
            :placeholder="$t('table.speed')"
          />
        </el-form-item>
        <el-form-item :label="$t('table.remark')" prop="remark" class="w-100%">
          <el-input
            v-model="newFormInline.X"
            clearable
            type="textarea"
            :placeholder="$t('table.remark')"
          />
        </el-form-item>
        <el-form-item
          :label="$t('table.isOnline')"
          prop="isOnline"
          class="w-100%"
        >
          <el-checkbox
            v-model="newFormInline.isOnline"
            :label="$t('table.isOnline')"
            size="large"
          />
        </el-form-item>
        <el-form-item
          :label="$t('table.isNormal')"
          prop="isNormal"
          class="w-100%"
        >
          <el-checkbox
            v-model="newFormInline.isNormal"
            :label="$t('table.isNormal')"
            size="large"
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
      </el-col>
    </el-row>
  </el-form>
</template>
