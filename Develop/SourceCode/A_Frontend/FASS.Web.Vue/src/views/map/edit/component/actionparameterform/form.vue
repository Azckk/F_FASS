<script setup lang="ts">
import { ref , reactive} from "vue";
import type { FormRules } from "element-plus";

// 声明 props 类型
export interface FormProps {
  formInline: {
    key: string;
    value: string;
  };
}

const formRules = reactive(<FormRules>{
  key: [{ required: true, message: "必填项", trigger: "blur" }],
  value: [{ required: true, message: "必填项", trigger: "blur" }],
});
// 声明 props 默认值
// 推荐阅读：https://cn.vuejs.org/guide/typescript/composition-api.html#typing-component-props
const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({ key: "", value: "" })
});
const ruleFormRef = ref();
function getRef() {
  return ruleFormRef.value;
}
defineExpose({ getRef });

// vue 规定所有的 prop 都遵循着单向绑定原则，直接修改 prop 时，Vue 会抛出警告。此处的写法仅仅是为了消除警告。
// 因为对一个 reactive 对象执行 ref，返回 Ref 对象的 value 值仍为传入的 reactive 对象，
// 即 newFormInline === props.formInline 为 true，所以此处代码的实际效果，仍是直接修改 props.formInline。
// 但该写法仅适用于 props.formInline 是一个对象类型的情况，原始类型需抛出事件
// 推荐阅读：https://cn.vuejs.org/guide/components/props.html#one-way-data-flow
const newFormInline = ref(props.formInline);
</script>

<template>
  <el-form ref="ruleFormRef"   :rules="formRules" :model="newFormInline">
    <el-form-item label="键" prop="key">
      <el-input
        v-model="newFormInline.key"
        class="!w-[220px]"
        placeholder="请输入键"
      />
    </el-form-item>
    <el-form-item label="值" prop="value">
      <el-input
        v-model="newFormInline.value"
        class="!w-[220px]"
        placeholder="请输入值"
      />
    </el-form-item>
  </el-form>
</template>
