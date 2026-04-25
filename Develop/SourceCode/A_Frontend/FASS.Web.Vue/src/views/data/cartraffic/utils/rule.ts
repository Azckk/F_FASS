import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  fromCarId: [{ required: true, message: "必填项", trigger: "blur" }],
  toCarId: [{ required: true, message: "必填项", trigger: "blur" }],
  count: [{ required: true, message: "必填项", trigger: "blur" }],
  // code: [{ required: true, message: "必填项", trigger: "blur" }],
  // // name: [{ required: true, message: "必填项", trigger: "blur" }],
  // sortNumber: [{ required: true, message: "必填项", trigger: "blur" }],
});
