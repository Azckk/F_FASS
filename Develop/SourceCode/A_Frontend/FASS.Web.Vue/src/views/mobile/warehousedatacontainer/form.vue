<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { useHook } from "./utils/hook";

// const { carList, nodeList, StorageStateList } = useHook();

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    areaCode: "",
    code: "",
    isLock: false,
    state: "",
    nodeCode: "",
    barcode: ""
  }),
  StorageStateList: []
});
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
      <re-col :value="12" :xs="24" :sm="24">
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item :label="$t('table.region')" prop="code">
            <el-input
              v-model="newFormInline.areaCode"
              disabled
              clearable
              :placeholder="$t('table.region')"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item :label="$t('table.code')" prop="code">
            <el-input
              v-model="newFormInline.code"
              disabled
              clearable
              :placeholder="$t('table.code')"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item :label="$t('table.isLock')" prop="isLock">
            <el-checkbox
              v-model="newFormInline.isLock"
              :label="$t('table.isEnable')"
              size="large"
            />
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item :label="$t('table.status')" prop="state">
            <el-select
              v-model="newFormInline.state"
              filterable
              class="!w-[100%]"
              :placeholder="$t('table.pleaseSelect')"
            >
              <el-option
                v-for="item in StorageStateList"
                :label="item.name"
                :value="item.code"
                :key="item.id"
              />
            </el-select>
          </el-form-item>
        </re-col>
        <re-col :value="24" :xs="24" :sm="24">
          <el-form-item :label="$t('table.barcode')" prop="barcode">
            <el-input
              v-model="newFormInline.barcode"
              clearable
              :placeholder="$t('table.barcode')"
            />
          </el-form-item>
        </re-col>
      </re-col>
    </el-row>
  </el-form>
</template>
