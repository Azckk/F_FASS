<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
// import { GetDictItemInLocal } from "@/utils/auth";
// import { usePublicHooks } from "../hooks";
// const { switchStyle } = usePublicHooks();

const AreaTypeList = ref([]);
const AreaStateList = ref([]);
// const { AreaTypeList, AreaStateList } = useHook();

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    key: "",
    value: "",
    isDisable: true
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

// onMounted(async () => {
//   AreaTypeList.value = await GetDictItemInLocal("AreaType");
//   AreaStateList.value = await GetDictItemInLocal("AreaState");
// });
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
        <el-form-item :label="$t('table.key')" prop="key">
          <el-input
            v-model="newFormInline.key"
            :disabled="newFormInline.isDisable"
            clearable
            :placeholder="$t('table.key')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.value')" prop="value">
          <el-input
            v-model="newFormInline.value"
            clearable
            :placeholder="$t('table.value')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
