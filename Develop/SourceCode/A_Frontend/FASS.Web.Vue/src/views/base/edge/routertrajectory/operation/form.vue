<script setup lang="ts">
import { ref, onMounted, handleError } from "vue";
import ReCol from "@/components/ReCol";
import Fold from "@iconify-icons/ep/fold";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";

// import { getListToSelect } from "@/api/frame/dictItem";
import { formRules } from "./utils/rule";
import { operationProps } from "./utils/types";
const props = withDefaults(defineProps<operationProps>(), {
  formInline: () => ({
    coordinatesX: "",
    coordinatesY: "",
    weight: 0
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
        <el-form-item :label="$t('table.coordinatesX')" prop="coordinatesX">
          <el-input
            v-model="newFormInline.coordinatesX"
            disabled
            :placeholder="$t('table.coordinatesX')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.coordinatesY')" prop="coordinatesY">
          <el-input
            v-model="newFormInline.coordinatesY"
            clearable
            disabled
            :placeholder="$t('table.coordinatesY')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.weight')" prop="weight">
          <el-input-number
            v-model="newFormInline.weight"
            clearable
            :placeholder="$t('table.weight')"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
