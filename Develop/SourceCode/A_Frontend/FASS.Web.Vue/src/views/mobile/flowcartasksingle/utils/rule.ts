import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  carId: [{ required: true, message: "必填项", trigger: "blur" }],
  targetNodeId: [{ required: true, message: "必填项", trigger: "blur" }]
});
