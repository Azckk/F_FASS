<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { GetTypeListToSelect } from "@/api/flow/tasktemplate";
import { GetDictItemInLocal } from "@/utils/auth";
const carTypeList = ref([]);
const TaskTemplateTypeList = ref([]);
const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    code: "",
    name: "",
    type: "Default",
    carTypeId: "",
    priority: 0,
    isLoop: false,
    timeout: 86400,
    isEnable: true
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {
  const { data } = await GetTypeListToSelect();
  carTypeList.value = [...data];
  TaskTemplateTypeList.value = await GetDictItemInLocal("TaskTemplateType");
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
        <el-form-item :label="$t('table.type')" prop="type">
          <el-select
            v-model="newFormInline.type"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in TaskTemplateTypeList"
              :label="item.name"
              :value="item.code"
              :key="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.carType')" prop="carTypeId">
          <el-select
            v-model="newFormInline.carTypeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in carTypeList"
              :label="item.name"
              :value="item.id"
              :key="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.priority')" prop="priority">
          <el-input-number
            class="w-[calc(100%)]"
            :min="-99999"
            :max="99999"
            controls-position="right"
            v-model="newFormInline.priority"
            :placeholder="$t('table.pleaseEnter')"
          />
          <!-- <el-input v-model="newFormInline.priority" clearable placeholder="优先级" /> -->
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.cycle')" prop="isLoop">
          <el-checkbox
            v-model="newFormInline.isLoop"
            :label="$t('table.isEnable')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item
          :label="$t('table.timeout')"
          prop="timeout"
          class="w-[calc(100%)]"
        >
          <el-input-number
            class="w-[calc(100%)]"
            :min="0"
            :max="99999"
            controls-position="right"
            v-model="newFormInline.timeout"
            :placeholder="$t('table.pleaseEnter')"
          />
          <!-- <el-input
            v-model="newFormInline.timeout"
            placeholder="超时时间"
            clearable
          /> -->
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.effectiveness')" prop="isEnable">
          <el-checkbox
            v-model="newFormInline.isEnable"
            :label="$t('table.isEnable')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
