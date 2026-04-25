<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { useHook } from "./utils/hook";
const ruleFormRef = ref();
const selectGenderList = ref([]);
const selectAvatarList = ref([]);
const {
  TaskTemplateList, //模板选择
  ListToSelectByTaskTemplateId, //模板id获取车辆
  TaskInstanceTypeList,
  taskTemplateIdChange,
  nodeList,
  edgeList,
  // StorageStateList,
  // AreaTypeList,
  // AreaStateList,
  carTypeList
} = useHook(ruleFormRef);
const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    code: "",
    name: "",
    type: "Default",
    carTypeId: "",
    priority: 0,
    taskTemplateId: "",
    carId: null,
    nodes: [],
    edges: [],
    state: ""
  })
});
const newFormInline = ref(props.formInline);

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
        <el-form-item :label="$t('table.taskTemplates')" prop="taskTemplateId">
          <el-select
            v-model="newFormInline.taskTemplateId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
            @change="taskTemplateIdChange"
          >
            <el-option :label="$t('table.doNotSpecify')" value="" />
            <el-option
              v-for="item in TaskTemplateList"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
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
              :label="item.code"
              :value="item.id"
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
              :key="item.id"
              :label="item.name"
              :value="item.id"
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
        <el-form-item :label="$t('table.type')" prop="type">
          <el-select
            v-model="newFormInline.type"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in TaskInstanceTypeList"
              :key="item.id"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.priority')" prop="priority">
          <el-input-number
            v-model="newFormInline.priority"
            class="w-[calc(100%)]"
            :min="-99999"
            :max="99999"
            controls-position="right"
            :placeholder="$t('table.pleaseEnter')"
          />
          <!-- <el-input v-model="newFormInline.priority" clearable placeholder="优先级" /> -->
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.specifySite')" prop="nodes">
          <el-select
            v-model="newFormInline.nodes"
            filterable
            multiple
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option label="#" value="#" />
            <el-option label="-" value="-" />
            <el-option
              v-for="item in nodeList"
              :key="item.id"
              :label="item.code"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.specifyRoute')" prop="edges">
          <el-select
            v-model="newFormInline.edges"
            filterable
            multiple
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option label="#" value="#" />
            <el-option label="-" value="-" />
            <el-option
              v-for="item in edgeList"
              :key="item.id"
              :label="item.code"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
