<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules, changePassword } from "./utils/rule";
import { FormProps } from "./utils/types";
import { getUserInfo } from "@/utils/auth";
const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    username: "",
    passwordOrigin: "",
    passwordNew: "",
    PasswordNewConfirm: ""
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {
  const userInfo = getUserInfo();
  newFormInline.value.username = userInfo.username;
});
const handleFocus = () => {
  ruleFormRef.value?.clearValidate(["passwordNew", "PasswordNewConfirm"]);
};
const handleBlur = () => {
  if (
    newFormInline.value.PasswordNewConfirm !== "" &&
    newFormInline.value.PasswordNewConfirm !== undefined &&
    newFormInline.value.PasswordNewConfirm !== null
  ) {
    ruleFormRef.value?.validateField("PasswordNewConfirm");
  }
};
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
        <el-form-item :label="$t('table.username2')" prop="username">
          <el-input
            v-model="newFormInline.username"
            clearable
            disabled
            :placeholder="$t('table.username2')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.passwordOrigin')" prop="passwordOrigin">
          <el-input
            v-model="newFormInline.passwordOrigin"
            clearable
            show-password
            :placeholder="$t('table.passwordOrigin')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.passwordNew')" prop="passwordNew">
          <el-input
            v-model="newFormInline.passwordNew"
            clearable
            show-password
            :placeholder="$t('table.passwordNew')"
            @focus="handleFocus"
            @blur="handleBlur"
            @change="changePassword(newFormInline.passwordNew, 1)"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item
          :label="$t('table.PasswordNewConfirm')"
          prop="PasswordNewConfirm"
        >
          <el-input
            v-model="newFormInline.PasswordNewConfirm"
            clearable
            show-password
            :placeholder="$t('table.PasswordNewConfirm')"
            @change="changePassword(newFormInline.PasswordNewConfirm, 2)"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
