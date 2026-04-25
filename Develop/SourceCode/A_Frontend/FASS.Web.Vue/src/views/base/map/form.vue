<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
// import { usePublicHooks } from "../hooks";
// const { switchStyle } = usePublicHooks();

const selectGenderList = ref([]);
const selectAvatarList = ref([]);

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
    fileContent: ""
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
          <el-input v-model="newFormInline.name" clearable placeholder="名称" />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.version')" prop="version">
          <el-input
            v-model="newFormInline.version"
            clearable
            :placeholder="$t('table.version')"
          />
        </el-form-item>
      </re-col>
      <!-- <re-col :value="12" :xs="24" :sm="24">
        <el-form-item label="性别" prop="gender">
          <el-select
            v-model="newFormInline.gender"
            placeholder="性别"
            clearable
            filterable
          >
            <el-option
              v-for="item in selectGenderList"
              :key="item.id"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col> 
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item label="生日" prop="birthday">
          <el-date-picker
            clearable
            v-model="newFormInline.birthday"
            type="date"
            placeholder="生日"
          />
        </el-form-item>
      </re-col>
      -->
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.fileName')" prop="fileName">
          <el-input
            v-model="newFormInline.fileName"
            clearable
            :placeholder="$t('table.fileName')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.fileContent')">
          <el-input
            v-model="newFormInline.fileContent"
            :placeholder="$t('table.fileContent')"
            :autosize="{ minRows: 12, maxRows: 18 }"
            type="textarea"
          />
        </el-form-item>
      </re-col>
      <!--
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item label="邮箱" prop="mail">
          <el-input v-model="newFormInline.mail" clearable placeholder="邮箱" />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item label="头像" prop="avatar">
          <el-select
            v-model="newFormInline.avatar"
            placeholder="头像"
            clearable
            filterable
          >
            <el-option
              v-for="item in selectAvatarList"
              :key="item.id"
              :label="item.label"
              :value="item.value"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item label="系统用户">
           <el-switch
            v-model="newFormInline.isSystem"
            inline-prompt
            :active-value="true"
            :inactive-value="false"
            active-text="是"
            inactive-text="否"
            :style="switchStyle"
          /> 
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item label="有效性">
          <el-switch
            v-model="newFormInline.isEnable"
            inline-prompt
            :active-value="true"
            :inactive-value="false"
            active-text="是"
            inactive-text="否"
            :style="switchStyle"
            />
        </el-form-item>
      </re-col>
      -->
    </el-row>
  </el-form>
</template>
