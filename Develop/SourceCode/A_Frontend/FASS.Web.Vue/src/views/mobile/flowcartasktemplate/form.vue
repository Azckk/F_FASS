<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { GetListToSelect } from "@/api/base/node";
import { getCarList } from "@/api/data/car";
import { useHook } from "./utils/hook";
const carList = ref([]);
const nodeList = ref([]);

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    carId: "",
    code: ""
  })
});
const { TaskTemplateList } = useHook();

const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });

onMounted(async () => {
  let res = await GetListToSelect();
  nodeList.value = res.data;
  let { data } = await getCarList();
  carList.value = data;
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
        <el-form-item :label="$t('table.ownVehicle')">
          <el-select
            v-model="newFormInline.carId"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in carList"
              :key="item.id"
              :label="`${item.code} (${item.name})`"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.taskTemplates')" prop="code">
          <el-select
            v-model="newFormInline.code"
            filterable
            class="!w-[100%]"
            :placeholder="$t('table.pleaseSelect')"
          >
            <el-option
              v-for="item in TaskTemplateList"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
