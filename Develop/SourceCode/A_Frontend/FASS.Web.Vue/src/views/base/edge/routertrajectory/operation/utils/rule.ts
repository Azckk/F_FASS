import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  // weight: [{ required: true, message: "必填项", trigger: "blur" }],
  coordinatesY: [{ required: true, message: "必填项", trigger: "blur" }],
  coordinatesX: [{ required: true, message: "必填项", trigger: "blur" }]
});
