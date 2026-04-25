import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  actionType: [{ required: true, message: "必填项", trigger: "blur" }],
  blockingType: [{ required: true, message: "必填项", trigger: "blur" }],
  sortNumber: [{ required: true, message: "必填项", trigger: "blur" }]
});
