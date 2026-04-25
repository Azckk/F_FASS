import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  areaId: [{ required: true, message: "必填项", trigger: "blur" }],
  storageId: [{ required: true, message: "必填项", trigger: "blur" }],
  callMode: [{ required: true, message: "必填项", trigger: "blur" }]
});
