import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  systemName: [{ required: true, message: "必填项", trigger: "blur" }],
  name: [{ required: true, message: "必填项", trigger: "blur" }],
  communicatioMode: [{ required: true, message: "必填项", trigger: "blur" }],
  url: [{ required: true, message: "必填项", trigger: "blur" }],
  method: [{ required: true, message: "必填项", trigger: "blur" }],
  headers: [{ required: true, message: "必填项", trigger: "blur" }],
  queryParams: [{ required: true, message: "必填项", trigger: "blur" }],
  responseParams: [{ required: true, message: "必填项", trigger: "blur" }]
});
