<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
// import { usePublicHooks } from "../hooks";
// const { switchStyle } = usePublicHooks();

const selectGenderList = ref([]);
const selectAvatarList = ref([]);

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    code: "",
    name: "",
    sortNumber: 1,
    isEnable: true
  })
});
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
        <!-- "code": "",
        "name": "",
        "sortNumber": 1,
        "isEnable": true -->

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
            placeholder="$t('table.name')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.sortNumber')" prop="sortNumber">
          <el-input-number
            class="w-[100%]"
            :min="-99999"
            :max="99999"
            controls-position="right"
            v-model="newFormInline.sortNumber"
            :placeholder="$t('table.pleaseEnter')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.effectiveness')">
          <el-checkbox
            v-model="newFormInline.isEnable"
            :label="$t('table.isEnable')"
            size="large"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
