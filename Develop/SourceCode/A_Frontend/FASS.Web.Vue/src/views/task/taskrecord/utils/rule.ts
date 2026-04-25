import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  code: [{ required: true, message: "必填项", trigger: "blur" }],
  type: [{ required: true, message: "必填项", trigger: "blur" }],
  srcNodeId: [{ required: true, message: "必填项", trigger: "blur" }],
  destNodeId: [{ required: true, message: "必填项", trigger: "blur" }],
  taskTemplateId: [{ required: true, message: "必填项", trigger: "blur" }]
});
