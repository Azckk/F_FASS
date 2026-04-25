import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  code: [{ required: true, message: "必填项", trigger: "blur" }],
  type: [{ required: true, message: "必填项", trigger: "blur" }],
  carTypeId: [{ required: true, message: "必填项", trigger: "blur" }],
  priority: [{ required: true, message: "必填项", trigger: "blur" }],
  timeout: [{ required: true, message: "必填项", trigger: "blur" }],
  targetNodeId: [{ required: true, message: "必填项", trigger: "blur" }]
});
