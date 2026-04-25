import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  openCloseSignal: [{ required: true, message: "必填项", trigger: "blur" }],
  station: [{ required: true, message: "必填项", trigger: "blur" }],
  remark: [{ required: true, message: "必填项", trigger: "blur" }]
});
