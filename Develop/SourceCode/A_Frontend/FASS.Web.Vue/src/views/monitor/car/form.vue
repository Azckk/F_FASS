<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
// import { usePublicHooks } from "../hooks";
// const { switchStyle } = usePublicHooks();
import { GetPcHardware } from "@/api/monitor/caraction";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import Pointer from "@iconify-icons/ep/pointer";

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
    electricity: "",
    CurrentTaskId: "",
    isAlarm: "",
    battery: "",
    stopAccept: false
  })
});
const newFormInline = ref(props.formInline);
// console.log( " CarControlTypeList  is"  , CarControlTypeList)

let text = "点击按钮获取";
const carPcInfo = ref({
  Cpu: text,
  Gpu: text,
  Tmp: text,
  Memory: text,
  NetW: text
});

let btnText = "获取信息";
const loading = ref(false);
async function getCarPcInfo() {
  loading.value = true;
  btnText = "正在加载中...";
  const res = await GetPcHardware({ carCode: newFormInline.value.code });
  carPcInfo.value = res.data;
  btnText = "获取信息";
  loading.value = false;
}
const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });
</script>
<template>
  <el-form ref="ruleFormRef" :model="newFormInline" label-width="100px">
    <el-row :gutter="20">
      <div class="grid-content ep-bg-purple" />
      <el-col :span="24">
        <el-form-item :label="$t('table.basicInformation')" class="w-100%" />
      </el-col>
      <el-col :span="12">
        <el-form-item :label="$t('table.vehicleID')" prop="code" class="w-100%">
          <el-input
            v-model="newFormInline.code"
            :placeholder="$t('table.code')"
          />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="$t('table.IPAddress')" prop="code" class="w-100%">
          <el-input
            v-model="newFormInline.ipAddress"
            :placeholder="$t('table.IPAddress')"
          />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="$t('table.carName')" prop="code" class="w-100%">
          <el-input
            v-model="newFormInline.name"
            :placeholder="$t('table.carName')"
          />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="$t('table.speed')" prop="code" class="w-100%">
          <el-input
            v-model="newFormInline.speed"
            :placeholder="$t('table.speed')"
          />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="$t('table.task')" prop="code" class="w-100%">
          <el-input
            v-model="newFormInline.CurrentTaskId"
            :placeholder="$t('table.task')"
          />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item
          :label="$t('table.alarmStatus')"
          prop="code"
          class="w-100%"
        >
          <el-input
            v-model="newFormInline.isAlarm as unknown as string"
            :placeholder="$t('table.alarmStatus')"
          />
        </el-form-item>
      </el-col>
      <!-- 调度状态 -->
      <el-col :span="12">
        <el-form-item :label="$t('table.schedulingStatus')" class="w-100%" />
      </el-col>
      <el-col :span="12">
        <el-form-item :label="$t('table.mark')">
          <span>{{ newFormInline.stopAccept ? "停接任务" : "" }}</span>
        </el-form-item>
        <!-- <el-form-item :label="$t('标记')" class="w-100%" />
        <span>{{ newFormInline.stopAccept ? "123" : "" }}</span> -->
      </el-col>
      <!-- 硬件状态 -->
      <el-col :span="24">
        <el-form-item :label="$t('table.hardwareStatus')" class="w-100%">
          <el-button
            text
            bg
            :loading="loading"
            size="small"
            :icon="useRenderIcon(Pointer)"
            @click="getCarPcInfo()"
            >{{ btnText }}</el-button
          >
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item
          :label="$t('table.electricity')"
          prop="code"
          class="w-100%"
        >
          <el-input
            v-model="newFormInline.battery"
            :placeholder="$t('table.electricity')"
          />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="$t('table.ethernet')" prop="code" class="w-100%">
          <el-input
            v-model="carPcInfo.NetW"
            :placeholder="$t('table.ethernet')"
          />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item label="CPU" prop="code" class="w-100%">
          <el-input v-model="carPcInfo.Cpu" placeholder="CPU" />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item
          :label="$t('table.temperature')"
          prop="code"
          class="w-100%"
        >
          <el-input
            v-model="carPcInfo.Tmp"
            :placeholder="$t('table.temperature')"
          />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item label="GPU" prop="code" class="w-100%">
          <el-input v-model="carPcInfo.Gpu" placeholder="GPU" />
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item :label="$t('table.memory')" prop="code" class="w-100%">
          <el-input
            v-model="carPcInfo.Memory"
            :placeholder="$t('table.memory')"
          />
        </el-form-item>
      </el-col>
    </el-row>
  </el-form>
</template>
