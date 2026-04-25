<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import dayjs from "dayjs";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { GetDictItemInLocal } from "@/utils/auth";
import {
  GetListToSelectByTaskTemplateId,
  GetTypeListToSelect,
  GetNodeListToSelect,
  GetTemplateListToSelect
} from "@/api/task/taskrecord";
const ruleFormRef = ref();
const TaskTemplateList = ref([]);
const taskTypeList = ref([]);
const ListToSelectByTaskTemplateId = ref([]);
const taskNodeList = ref([]);
const carTypeList = ref([]);
const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    code: dayjs().format("YYYYMMDDTHHmmssSSS"),
    name: "",
    type: "",
    carTypeId: "",
    isLoop: false,
    priority: 0,
    taskTemplateId: "",
    carId: "",
    nodes: [],
    edges: [],
    destNodeId: "",
    srcNodeId: "",
    condition: ""
  })
});
const newFormInline = ref(props.formInline);

function getRef() {
  return ruleFormRef.value;
}

function srcNodeIdChange(value) {
  const selectedItem = taskNodeList.value.find(item => item.id === value);
  if (selectedItem) {
    // const label = `${selectedItem.name} (${selectedItem.code})`;
    return selectedItem.code;
  }
}

function destNodeIdChange(value) {
  const selectedItem = taskNodeList.value.find(item => item.id === value);
  if (selectedItem) {
    const label = `${selectedItem.name} (${selectedItem.code})`;
    return `${selectedItem.name} (${selectedItem.code})`;
  }
}

defineExpose({ getRef });

onMounted(async () => {
  const { data } = await GetTemplateListToSelect();
  TaskTemplateList.value = data;
  let taskTypeDictItem = await GetDictItemInLocal("TaskType");
  taskTypeDictItem = taskTypeDictItem.filter(
    item => item.code !== "LogisticsRoute"
  );
  taskTypeList.value = [...taskTypeDictItem];
  const TaskTemplateIdList = await GetListToSelectByTaskTemplateId(null);
  ListToSelectByTaskTemplateId.value = [...TaskTemplateIdList.data];
  const NodeListToSelect = await GetNodeListToSelect();
  taskNodeList.value = NodeListToSelect.data;
  const TypeList = await GetTypeListToSelect();
  carTypeList.value = [...TypeList.data];
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
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.code')" prop="code">
          <el-input
            v-model="newFormInline.code"
            clearable
            :placeholder="$t('table.code')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.carType')" prop="carTypeId">
          <el-select
            v-model="newFormInline.carTypeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in carTypeList"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.taskType')" prop="type">
          <el-select
            v-model="newFormInline.type"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in taskTypeList"
              :key="item.id"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.taskTemplates')" prop="taskTemplateId">
          <el-select
            v-model="newFormInline.taskTemplateId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.doNotSpecify')" value="" />
            <el-option
              v-for="item in TaskTemplateList"
              :key="item.id"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.taskStartingPoint')" prop="srcNodeId">
          <el-select
            v-model="newFormInline.srcNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
            @change="srcNodeIdChange"
          >
            <el-option
              v-for="item in taskNodeList"
              :key="item.id"
              :label="`${item.name} (${item.code})`"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.taskEndingPoint')" prop="destNodeId">
          <el-select
            v-model="newFormInline.destNodeId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
            @change="destNodeIdChange"
          >
            <el-option
              v-for="item in taskNodeList"
              :key="item.id"
              :label="`${item.name} (${item.code})`"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.priority')">
          <el-input-number
            v-model="newFormInline.priority"
            clearable
            :placeholder="$t('table.pleaseEnter')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.taskOver')" prop="condition">
          <el-input
            v-model="newFormInline.condition"
            clearable
            :placeholder="$t('table.taskOver')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.ownVehicle')" prop="carId">
          <el-select
            v-model="newFormInline.carId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.doNotSpecify')" value="" />
            <el-option
              v-for="item in ListToSelectByTaskTemplateId"
              :key="item.id"
              :label="`${item.code}(${item.name})`"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="12" :xs="24" :sm="24">
        <el-form-item :label="$t('table.duplicate')" prop="extend">
          <el-select
            v-model="newFormInline.isLoop"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option label="否" :value="false" />
            <el-option label="是" :value="true" />
          </el-select>
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
