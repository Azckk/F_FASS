<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { useHook } from "./utils/hook";

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    nodeId: "",
    code: "",
    name: "",
    type: "Default",
    sortNumber: null,
    taskTemplateId: ""
  })
});
const keyValue = ref(props.formInline.taskTemplateId);

const { TaskTemplateProcessTypeList, nodeList } = useHook(null, keyValue);

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
        <el-form-item :label="$t('table.site')" prop="nodeId">
          <el-select
            v-model="newFormInline.nodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option label="--请选择--" value="" />
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
            clearable:placeholder="$t('table.code')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.type')" prop="type">
          <el-select
            v-model="newFormInline.type"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.type')"
          >
            <el-option
              v-for="item in TaskTemplateProcessTypeList"
              :label="item.name"
              :value="item.code"
              :key="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.sortNumber')" prop="sortNumber">
          <el-input-number
            class="w-[calc(100%)]"
            :min="-99999"
            :max="99999"
            controls-position="right"
            v-model="newFormInline.sortNumber"
            :placeholder="$t('table.pleaseSelect')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
