import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  name: [{ required: true, message: "必填项", trigger: "blur" }],
  srcAreaId: [{ required: true, message: "必填项", trigger: "blur" }],
  destAreaId: [{ required: true, message: "必填项", trigger: "blur" }]
});
