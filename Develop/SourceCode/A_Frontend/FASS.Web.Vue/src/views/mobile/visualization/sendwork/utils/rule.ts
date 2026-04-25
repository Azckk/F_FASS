import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  taskTemplateId: [{ required: true, message: "必填项", trigger: "blur" }],
  srcStorageId: [{ required: true, message: "必填项", trigger: "blur" }],
  destStorageId: [{ required: true, message: "必填项", trigger: "blur" }],
  // carId: [{ required: true, message: "必填项", trigger: "blur" }],
  state: [{ required: true, message: "必填项", trigger: "blur" }]
});
export const formRules2 = reactive(<FormRules>{
  taskTemplateId: [{ required: true, message: "必填项", trigger: "blur" }],
  carId: [{ required: true, message: "必填项", trigger: "blur" }]
});
