<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { useHook } from "./utils/hook";
// import { usePublicHooks } from "../hooks";
// const { switchStyle } = usePublicHooks();

const selectGenderList = ref([]);
const selectAvatarList = ref([]);

const {
  carList,
  operationTypeList,
  TaskTemplateActionBlockingTypeDataList,
  getListToSelect
} = useHook();

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    carId: "",
    actionType: "",
    actionDescription: "",
    blockingType: "",
    remark: ""
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {
  if (newFormInline.value.carId) {
    getListToSelect(newFormInline.value.carId);
  }
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
        <el-form-item :label="$t('table.ownVehicle')" prop="carId">
          <el-select
            v-model="newFormInline.carId"
            filterable
            :placeholder="$t('table.pleaseSelect')"
            @change="getListToSelect(newFormInline.carId)"
          >
            <el-option
              v-for="item in carList"
              :key="item.id"
              :label="`${item.code}(${item.name})`"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.operationType')" prop="actionType">
          <el-select
            v-model="newFormInline.actionType"
            filterable
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in operationTypeList"
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
        <el-form-item :label="$t('table.remark')" prop="name">
          <el-input
            v-model="newFormInline.remark"
            clearable
            :placeholder="$t('table.remark')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
