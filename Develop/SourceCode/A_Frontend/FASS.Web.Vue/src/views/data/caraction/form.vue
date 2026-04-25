<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { GetTypeListToSelect } from "@/api/data/caraction";
// const { switchStyle } = usePublicHooks();

const carTypeList = ref([]);
const selectAvatarList = ref([]);

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    carTypeId: "",
    code: "",
    name: "",
    sortNumber: 1,
    isEnable: true,
    createAt: ""
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
        <el-form-item :label="$t('table.isEnable')">
          <el-checkbox
            v-model="newFormInline.isEnable"
            :label="$t('table.isEnable')"
            size="large"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.sortNumber')" prop="sortNumber">
          <el-input-number
            v-model="newFormInline.sortNumber"
            class="w-[100%]"
            :min="-99999"
            :max="99999"
            controls-position="right"
            :placeholder="$t('table.pleaseEnter')"
          />
        </el-form-item>
      </re-col>
      <!-- <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.createAt')" prop="createAt">
          <el-input
            v-model="newFormInline.createAt"
            clearable
            :placeholder="$t('table.createAt')"
          />
        </el-form-item>
      </re-col> -->
    </el-row>
  </el-form>
</template>
