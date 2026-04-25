<script setup lang="ts">
import { ref, onMounted } from "vue";
import ReCol from "@/components/ReCol";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { usePublicHooks } from "../hooks";
const { switchStyle } = usePublicHooks();

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    name: "",
    colour: "",
    value: "",
    code: "",
    isEnable: true
  })
});
const newFormInline = ref(props.formInline);

const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}
const color1 = ref("#409EFF");
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
        <el-form-item :label="$t('table.tagName')" prop="name">
          <el-input
            v-model="newFormInline.name"
            clearable
            :placeholder="$t('table.tagName')"
          />
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
      <!-- code: row?.code ?? "", -->
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.tagColor')" prop="colour">
          <!-- <el-input
            v-model="newFormInline.colour"
            clearable
            placeholder="标签颜色"
          /> -->
          <el-color-picker v-model="newFormInline.colour" />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.tagParameters')" prop="value">
          <el-input
            v-model="newFormInline.value"
            clearable
            :placeholder="$t('table.tagParameters')"
          />
        </el-form-item>
      </re-col>
      <re-col :value="24" :xs="24" :sm="24">
        <el-form-item :label="$t('table.isEnable')" prop="isEnable">
          <el-switch
            v-model="newFormInline.isEnable"
            inline-prompt
            :active-value="true"
            :inactive-value="false"
            active-text="是"
            inactive-text="否"
            :style="switchStyle"
          />
        </el-form-item>
      </re-col>
    </el-row>
  </el-form>
</template>
<style scoped>
.demo-color-block {
  display: flex;
  align-items: center;
  margin-bottom: 16px;
}
.demo-color-block .demonstration {
  margin-right: 16px;
}
</style>
