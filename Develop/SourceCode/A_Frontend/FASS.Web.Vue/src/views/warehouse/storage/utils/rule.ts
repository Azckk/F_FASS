import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  areaId: [{ required: true, message: "必填项", trigger: "blur" }],
  nodeId: [{ required: true, message: "必填项", trigger: "blur" }],
  code: [{ required: true, message: "必填项", trigger: "blur" }],
  type: [{ required: true, message: "必填项", trigger: "blur" }],
  state: [{ required: true, message: "必填项", trigger: "blur" }],
});