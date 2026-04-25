<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
// import { usePublicHooks } from "../hooks";
import { getNodeList } from "@/api/data/charge";
import { useMyI18n } from "@/plugins/i18n";
import { GetDictItemInLocal } from "@/utils/auth";
const { t, locale } = useMyI18n();
const nodeList = ref([]);
const standbyStateList = ref([]);

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    nodeId: "",
    code: "",
    name: "",
    state: ""
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {
  const { data } = await getNodeList();
  nodeList.value = [...data];
  standbyStateList.value = await GetDictItemInLocal("ChargeState");
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
        <el-form-item :label="$t('table.site')" prop="nodeId">
          <el-select
            v-model="newFormInline.nodeId"
            filterable
            class="!w-[100%]"
            placeholder="请选择"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in nodeList"
              :label="item.code"
              :value="item.id"
              :key="item.id"
            />
          </el-select>
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
        <el-form-item :label="$t('table.status')" prop="state">
          <el-select
            v-model="newFormInline.state"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.status')"
          >
            <el-option
              v-for="item in standbyStateList"
              :label="`${locale == 'zh' ? item.name : item.code}`"
              :value="item.code"
              :key="item.id"
            />
          </el-select>
          <!-- <el-input-number class="w-[100%]"
            :min="-99999"
            :max="99999"
            controls-position="right" 
            v-model="newFormInline.state"   placeholder="请输入" /> -->
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
