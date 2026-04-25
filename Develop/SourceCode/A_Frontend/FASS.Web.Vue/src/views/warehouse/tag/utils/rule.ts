import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  name: [{ required: true, message: "必填项", trigger: "blur" }],
  code: [{ required: true, message: "必填项", trigger: "blur" }],
  colour: [{ required: true, message: "必填项", trigger: "blur" }],
  value: [{ required: true, message: "必填项", trigger: "blur" }],
});
