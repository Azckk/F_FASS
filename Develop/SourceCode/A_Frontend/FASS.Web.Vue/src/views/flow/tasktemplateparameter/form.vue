<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { useHook } from "./utils/hook";

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    actionId: "",
    key: "",
    value: ""
  })
});
const keyValue = ref(props.formInline.actionId);
console.log(" keyValue is", keyValue);
const {} = useHook(null, keyValue);

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
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.key')" prop="key">
          <el-input
            class="w-[calc(100%)]"
            v-model="newFormInline.key"
            :placeholder="$t('table.key')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.value')" prop="value">
          <el-input
            :autosize="{ minRows: 4, maxRows: 99 }"
            type="textarea"
            class="w-[calc(100%)]"
            v-model="newFormInline.value"
            :placeholder="$t('table.value')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
