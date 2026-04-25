<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { useHook } from "./utils/hook";

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    actionType: "",
    actionDescription: "",
    blockingType: "",
    sortNumber: null,
    taskInstanceProcessId: ""
  })
});
const keyValue = ref(props.formInline.taskInstanceProcessId);
console.log(" keyValue is", keyValue);
const { TemplateActionBlockingTypeList, ListByCarTypeCode } = useHook(
  null,
  keyValue
);

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
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.operationType')" prop="actionType">
          <el-select
            v-model="newFormInline.actionType"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option label="--请选择--" value="" />
            <el-option
              v-for="item in ListByCarTypeCode"
              :label="item.name"
              :value="item.code"
              :key="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item
          :label="$t('table.operationDescription')"
          prop="actionDescription"
        >
          <el-input
            type="textarea"
            v-model="newFormInline.actionDescription"
            clearable
            :placeholder="$t('table.operationDescription')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.blockageType')" prop="blockingType">
          <el-select
            v-model="newFormInline.blockingType"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in TemplateActionBlockingTypeList"
              :label="item.name"
              :value="item.code"
              :key="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.sortNumber')" prop="sortNumber">
          <el-input-number
            class="w-[calc(100%)]"
            :min="-99999"
            :max="99999"
            controls-position="right"
            v-model="newFormInline.sortNumber"
            :placeholder="$t('table.pleaseSelect')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
