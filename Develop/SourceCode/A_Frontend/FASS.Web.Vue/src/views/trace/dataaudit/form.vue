<script setup lang="ts">
import { ref } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    tableName: "",
    entityState: "",
    originalValue: "",
    currentValue: "",
    currentAction: ""
  })
});
const newFormInline = ref(props.formInline);

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
    <el-row :gutter="30">
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.tableName')" prop="tableName">
          <el-input
            v-model="newFormInline.tableName"
            clearable
            :placeholder="$t('table.tableName')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.entityState')" prop="entityState">
          <el-input
            v-model="newFormInline.entityState"
            clearable
            :placeholder="$t('table.entityState')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.originalValue')" prop="originalValue">
          <el-input
            v-model="newFormInline.originalValue"
            :placeholder="$t('table.originalValue')"
            type="textarea"
            :rows="5"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.currentValue')" prop="currentValue">
          <el-input
            v-model="newFormInline.currentValue"
            :placeholder="$t('table.currentValue')"
            type="textarea"
            :rows="5"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.currentAction')" prop="currentAction">
          <el-input
            v-model="newFormInline.currentAction"
            :placeholder="$t('table.currentAction')"
            type="textarea"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
