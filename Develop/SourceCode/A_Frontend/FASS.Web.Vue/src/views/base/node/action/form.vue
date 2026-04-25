<script setup lang="ts">
import { ref, onMounted, handleError } from "vue";
import ReCol from "@/components/ReCol";
import Fold from "@iconify-icons/ep/fold";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { GetListToSelectByCarTypeCode } from "@/api/base/node";
import { GetDictItemInLocal } from "@/utils/auth";

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    actionType: "",
    operationDescription: "",
    blockingType: "",
    sortNumber: 0
  }),
  operation: Function
});
const newFormInline = ref(props.formInline);
const actionTypeData = ref();
const TaskTemplateActionBlockingTypeDataList = ref();
const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {
  const { data } = await GetListToSelectByCarTypeCode("Car");
  actionTypeData.value = data;
  const res = await GetDictItemInLocal("TaskTemplateActionBlockingType");
  TaskTemplateActionBlockingTypeDataList.value = res;
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
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.operationType')" prop="actionType">
          <el-select
            v-model="newFormInline.actionType"
            filterable
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in actionTypeData"
              :key="item.id"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.operationDescription')">
          <el-input
            v-model="newFormInline.operationDescription"
            :placeholder="$t('table.operationDescription')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.blockageType')" prop="blockingType">
          <el-select
            v-model="newFormInline.blockingType"
            filterable
            :placeholder="$t('table.pleaseSelect')"
          >
            <!-- <el-option label="无限制" value="NONE" />
            <el-option label="禁止行驶" value="SOFT" />
            <el-option label="禁止行驶且禁止其它动作" value="HARD" /> -->
            <el-option
              v-for="item in TaskTemplateActionBlockingTypeDataList"
              :key="item.id"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.sortNumber')" prop="sortNumber">
          <el-input-number
            v-model="newFormInline.sortNumber"
            clearable
            :placeholder="$t('table.sortNumber')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.operation')">
          <el-button
            style="width: 100%"
            type="primary"
            :icon="useRenderIcon(Fold)"
            @click="operation()"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
