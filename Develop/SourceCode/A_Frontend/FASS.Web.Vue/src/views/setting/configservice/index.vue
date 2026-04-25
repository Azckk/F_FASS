<script setup lang="ts">
import { ref } from "vue";
import { useHook } from "./utils/hook";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";

defineOptions({
  name: "Setting-ConfigData"
});
const { form, formRef, loading, handleGetData, handleSetData } = useHook();
</script>

<template>
  <el-card shadow="never" :body-style="{ height: 'calc(100vh - 260px)' }">
    <el-form
      ref="formRef"
      :model="form"
      :rules="formRules"
      label-width="200px"
      class="w-[600px] m-auto"
    >
      <el-row :gutter="30">
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item :label="$t('table.configurationItem1')" prop="item1">
            <el-input
              v-model="form.item1"
              clearable
              :placeholder="$t('table.configurationItem1')"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item :label="$t('table.configurationItem2')" prop="item2">
            <el-input
              v-model="form.item2"
              clearable
              :placeholder="$t('table.configurationItem2')"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item :label="$t('table.configurationItem3')" prop="item3">
            <el-input
              v-model="form.item3"
              clearable
              :placeholder="$t('table.configurationItem3')"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24" justify="end">
          <el-form-item>
            <el-button type="primary" @click="handleSetData">{{
              $t("table.save")
            }}</el-button>
          </el-form-item>
        </re-col>
      </el-row>
    </el-form>
  </el-card>
</template>
