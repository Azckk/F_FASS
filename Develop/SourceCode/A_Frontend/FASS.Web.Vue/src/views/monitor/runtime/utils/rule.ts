import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  targetNodeId: [{ required: true, message: "必填项", trigger: "blur" }]
});
