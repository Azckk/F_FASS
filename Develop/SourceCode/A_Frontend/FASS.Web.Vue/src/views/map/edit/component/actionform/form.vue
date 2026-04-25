<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import MoreFilled from "@iconify-icons/ep/more-filled";
import { useHook } from "./utils/hook";
const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    actionId: "",
    actionParameters: [],
    sortNumber: 1,
    actionType: "",
    actionDescription: "",
    blockingType: ""
  })
});
const newFormInline = ref(props.formInline);

const {
  ListToSelectByCarTypeCode,
  ListActionBlockingType,
  openActionParameterIndex
} = useHook(newFormInline.value);
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
        <el-form-item label="操作类型">
          <el-select
            v-model="newFormInline.actionType"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.pleaseSelect')" value=""></el-option>
            <el-option
              v-for="item in ListToSelectByCarTypeCode"
              :key="item.id"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item label="操作描述">
          <el-input
            v-model="newFormInline.actionDescription"
            placeholder="操作描述"
            maxlength="99999"
            :rows="3"
            type="textarea"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item label="堵塞类型">
          <el-select
            v-model="newFormInline.blockingType"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in ListActionBlockingType"
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
            class="w-[100%]"
            :min="1"
            :max="99999"
            controls-position="right"
            :placeholder="$t('table.pleaseEnter')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item label="操作参数" prop="actionParameters">
          <el-button type="primary" @click="openActionParameterIndex">
            <IconifyIconOffline :icon="MoreFilled" />
          </el-button>
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
