<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { useHook } from "./utils/hook";

const { nodeList, carTypeList, CarControlTypeList, CarAvoidTypeList } =
  useHook();

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
    level: "",
    createAt: "",
    message: ""
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {});
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
        <el-form-item :label="$t('table.level')" prop="level" class="w-100%">
          <el-input
            class="w-[calc(100%)]"
            :controls="false"
            v-model="newFormInline.level"
            :placeholder="$t('table.level')"
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
        <el-form-item :label="$t('table.code')" prop="code" class="w-100%">
          <el-input
            class="w-[calc(100%)]"
            :controls="false"
            v-model="newFormInline.code"
            :placeholder="$t('table.code')"
          />
        </el-form-item>
        <el-form-item
          :label="$t('table.message')"
          prop="message"
          class="w-100%"
        >
          <el-input
            class="w-[calc(100%)]"
            :controls="false"
            v-model="newFormInline.message"
            :placeholder="$t('table.message')"
          />
        </el-form-item>
        <el-form-item
          :label="$t('table.createAt')"
          prop="createAt"
          class="w-100%"
        >
          <el-input
            class="w-[calc(100%)]"
            :controls="false"
            v-model="newFormInline.createAt"
            :placeholder="$t('table.createAt')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
