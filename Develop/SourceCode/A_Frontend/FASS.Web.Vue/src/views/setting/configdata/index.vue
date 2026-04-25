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
          <el-form-item
            :label="$t('table.userLogDayLimit')"
            prop="userLogDayLimit"
          >
            <el-input
              v-model="form.userLogDayLimit"
              clearable
              :placeholder="$t('table.userLogDayLimit')"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item
            :label="$t('table.auditLogDayLimit')"
            prop="auditLogDayLimit"
          >
            <el-input
              v-model="form.auditLogDayLimit"
              clearable
              :placeholder="$t('table.auditLogDayLimit')"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item :label="$t('table.diaryDayLimit')" prop="diaryDayLimit">
            <el-input
              v-model="form.diaryDayLimit"
              clearable
              :placeholder="$t('table.diaryDayLimit')"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item :label="$t('table.alarmDayLimit')" prop="alarmDayLimit">
            <el-input
              v-model="form.alarmDayLimit"
              clearable
              :placeholder="$t('table.alarmDayLimit')"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item
            :label="$t('table.carInstantActionDayLimit')"
            prop="carInstantActionDayLimit"
          >
            <el-input
              v-model="form.carInstantActionDayLimit"
              clearable
              :placeholder="$t('table.carInstantActionDayLimit')"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item
            :label="$t('table.taskInstanceDayLimit')"
            prop="taskInstanceDayLimit"
          >
            <el-input
              v-model="form.taskInstanceDayLimit"
              clearable
              :placeholder="$t('table.taskInstanceDayLimit')"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item :label="$t('table.workDayLimit')" prop="workDayLimit">
            <el-input
              v-model="form.workDayLimit"
              clearable
              :placeholder="$t('table.workDayLimit')"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item :label="$t('table.carMinBattery')" prop="carMinBattery">
            <el-input
              v-model="form.carMinBattery"
              clearable
              :placeholder="$t('table.carMinBattery')"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item :label="$t('table.carMaxBattery')" prop="carMaxBattery">
            <el-input
              v-model="form.carMaxBattery"
              clearable
              :placeholder="$t('table.carMaxBattery')"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item :label="$t('table.carIdleSecond')" prop="carIdleSecond">
            <el-input
              v-model="form.carIdleSecond"
              clearable
              :placeholder="$t('table.carIdleSecond')"
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
