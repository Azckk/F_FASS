<script setup lang="ts">
import { ref } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
// import { usePublicHooks } from "../hooks";
// const { switchStyle } = usePublicHooks();

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    username: "",
    code: "",
    name: "",
    nick: "",
    gender: "",
    birthday: "",
    phone: "",
    mail: "",
    avatar: "",
    isOnline: false,
    isSystem: false,
    isEnable: true,
    remark: "",
    version: "",
    fileName: "",
    fileContent: "",
    kind: "",
    type: "",
    isLock: true,
    sequenceId: "",
    nodeDescription: ""
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });
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
        <el-form-item :label="$t('table.classification')" prop="kind">
          <el-input
            v-model="newFormInline.kind"
            clearable
            :placeholder="$t('table.classification')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.type')" prop="type">
          <el-input
            v-model="newFormInline.type"
            clearable
            :placeholder="$t('table.type')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.code')" prop="code">
          <el-input
            v-model="newFormInline.code"
            clearable
            :placeholder="$t('table.code')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.name')" prop="name">
          <el-input
            v-model="newFormInline.name"
            clearable
            :placeholder="$t('table.name')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.isLock')" prop="isLock">
          <el-switch
            v-model="newFormInline.isLock"
            class="ml-2"
            active-text="是"
            inactive-text="否"
            inline-prompt
            style="
              --el-switch-on-color: #13ce66;
              --el-switch-off-color: #ff4949;
            "
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.sequenceId')" prop="sequenceId">
          <el-input
            v-model="newFormInline.sequenceId"
            clearable
            :placeholder="$t('table.sequenceId')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item
          :label="$t('table.nodeDescription')"
          prop="nodeDescription"
        >
          <el-input
            type="textarea"
            v-model="newFormInline.nodeDescription"
            clearable
            :placeholder="$t('table.nodeDescription')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.isEnable')" prop="isEnable">
          <input type="checkbox" v-model="newFormInline.isEnable" />
          <span>{{ $t("table.isEnable") }}</span>
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
