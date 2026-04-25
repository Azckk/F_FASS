<script setup lang="ts">
import { ref, onMounted, reactive, watch } from "vue";
import ReCol from "@/components/ReCol";
// import { formRules } from "./utils/rule";
import type { FormRules } from "element-plus";
import { FormProps } from "./utils/types";
import { useHook } from "./utils/hook";

const {
  areaList,
  modelList,
  storageList,
  MaterialList,
  FunctionGetListByTypeToSelect,
  AreaChange
} = useHook();

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    storageId: "",
    callMode: "",
    areaId: "",
    materialId: ""
  })
});
const newFormInline = reactive(props.formInline);

const formRules = reactive(<FormRules>{
  areaId: [{ required: true, message: "必填项", trigger: "blur" }],
  storageId: [{ required: true, message: "必填项", trigger: "blur" }],
  callMode: [{ required: true, message: "必填项", trigger: "blur" }],
  materialId: [{ required: true, message: "必填项", trigger: "blur" }]
});

watch(
  () => newFormInline.callMode,
  newValue => {
    if (newValue !== "EmptyOffline") {
      formRules.materialId[0].required = true;
    } else {
      formRules.materialId[0].required = false;
    }
  }
);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

const callback = () => {
  newFormInline.storageId = "";
  newFormInline.callMode = "";
};

defineExpose({ getRef });

onMounted(async () => {
  FunctionGetListByTypeToSelect();
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
      <re-col :value="12" :xs="12" :sm="12">
        <el-form-item :label="$t('table.callarea')" prop="areaId">
          <el-select
            v-model="newFormInline.areaId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
            @change="AreaChange(newFormInline.areaId, callback)"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in areaList"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="12" :sm="12">
        <el-form-item :label="$t('table.callmodel')" prop="callMode">
          <el-select
            v-model="newFormInline.callMode"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in modelList"
              :key="item.id"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="12" :sm="12">
        <el-form-item :label="$t('table.callsite')" prop="storageId">
          <el-select
            v-model="newFormInline.storageId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in storageList"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <!-- <re-col :value="12" :xs="12" :sm="12">
        <el-form-item :label="$t('table.vehicleID')">
          <el-select
            v-model="newFormInline.carId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in storageList"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col> -->
      <re-col :value="12" :xs="12" :sm="12">
        <el-form-item :label="$t('table.materialCode')" prop="materialId">
          <el-select
            v-model="newFormInline.materialId"
            filterable
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in MaterialList"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
