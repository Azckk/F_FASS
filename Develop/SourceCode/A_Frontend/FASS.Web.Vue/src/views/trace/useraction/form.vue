<script setup lang="ts">
import { ref } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    controller: "",
    action: "",
    watch: "",
    requestUrl: "",
    requestToken: "",
    responseCode: "",
    userAgent: "",
    ipAddress: ""
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
        <el-form-item :label="$t('table.controller')" prop="controller">
          <el-input
            v-model="newFormInline.controller"
            clearable
            :placeholder="$t('table.controller')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.operation')" prop="action">
          <el-input
            v-model="newFormInline.action"
            clearable
            :placeholder="$t('table.operation')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.timing')" prop="watch">
          <el-input
            v-model="newFormInline.watch"
            clearable
            :placeholder="$t('table.timing')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.requestUrl')" prop="requestUrl">
          <el-input
            v-model="newFormInline.requestUrl"
            :placeholder="$t('table.requestUrl')"
            type="textarea"
            :rows="3"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.requestToken')" prop="requestToken">
          <el-input
            v-model="newFormInline.requestToken"
            :placeholder="$t('table.requestToken')"
            type="textarea"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.responseCode')" prop="responseCode">
          <el-input
            v-model="newFormInline.responseCode"
            clearable
            :placeholder="$t('table.responseCode')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.userAgent')" prop="userAgent">
          <el-input
            v-model="newFormInline.userAgent"
            :placeholder="$t('table.userAgent')"
            type="textarea"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.IPAddress')" prop="ipAddress">
          <el-input
            v-model="newFormInline.ipAddress"
            clearable
            :placeholder="$t('table.IPAddress')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
