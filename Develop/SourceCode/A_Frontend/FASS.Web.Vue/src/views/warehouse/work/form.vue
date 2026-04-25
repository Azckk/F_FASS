<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { GetDictItemInLocal } from "@/utils/auth";
import { GetListToSelect } from "@/api/warehouse/work/index";

const containerList = ref([]);
const AreaStateList = ref([]);
const AreaTypeList = ref([]);

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    code: "",
    name: "",
    type: "",
    state: "",
    containerId: "",
    task: ""
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {
  const { data } = await GetListToSelect();
  containerList.value = data;
  AreaStateList.value = await GetDictItemInLocal("AreaState");
  // AreaStateList.value = await GetDictItemInLocal("CarState");
  AreaTypeList.value = await GetDictItemInLocal("AreaType");
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
        <el-form-item :label="$t('buttons.container')" prop="containerId">
          <el-select
            v-model="newFormInline.containerId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in containerList"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.task')" prop="task">
          <el-select
            v-model="newFormInline.task"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in AreaTypeList"
              :key="item.code"
              :label="item.name"
              :value="item.code"
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
              v-for="item in AreaTypeList"
              :key="item.code"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.status')" prop="state">
          <el-select
            v-model="newFormInline.state"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option :label="$t('table.pleaseSelect')" value="" />
            <el-option
              v-for="item in AreaStateList"
              :key="item.code"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
