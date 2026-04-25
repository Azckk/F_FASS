<script setup lang="ts">
import { ref, onMounted, handleError } from "vue";
import ReCol from "@/components/ReCol";
import Fold from "@iconify-icons/ep/fold";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { GetTrajectory } from "@/api/base/edge";
import { GetDictItemInLocal } from "@/utils/auth";

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    edgeId: ""
  }),
  operation: Function
});
const newFormInline = ref(props.formInline);
const actionTypeData = ref({
  degree: 0,
  knotVector: [0],
  controlPoints: []
});
const TaskTemplateActionBlockingTypeDataList = ref();
const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {
  const { data } = await GetTrajectory(newFormInline.value.edgeId);
  actionTypeData.value = data;
});
</script>

<template>
  <el-form
    ref="ruleFormRef"
    :model="actionTypeData"
    :rules="formRules"
    label-width="150px"
  >
    <el-row :gutter="30">
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.controlPointNum')" prop="degree">
          <el-input
            v-model="actionTypeData.degree"
            disabled
            :placeholder="$t('table.controlPointNum')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.controlPointNURSB')" prop="knotVector">
          <el-input
            v-model="actionTypeData.knotVector[0]"
            disabled
            :placeholder="$t('table.controlPointNURSB')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.controlPoint')">
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
