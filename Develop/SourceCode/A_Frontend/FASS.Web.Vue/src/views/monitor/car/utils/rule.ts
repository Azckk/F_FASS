import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  carTypeId: [{ required: true, message: "必填项", trigger: "blur" }],
  code: [{ required: true, message: "必填项", trigger: "blur" }],
  ipAddress: [{ required: true, message: "必填项", trigger: "blur" }],
  port: [{ required: true, message: "必填项", trigger: "blur" }],
  width: [{ required: true, message: "必填项", trigger: "blur" }],
  height: [{ required: true, message: "必填项", trigger: "blur" }],
  length: [{ required: true, message: "必填项", trigger: "blur" }],
  controlType: [{ required: true, message: "必填项", trigger: "blur" }],
  avoidType: [{ required: true, message: "必填项", trigger: "blur" }],
  targetNodeId: [{ required: true, message: "必填项", trigger: "blur" }],
  // username: [{ required: true, message: "必填项", trigger: "blur" }],
  // phone: [
  //   {
  //     pattern:
  //       /^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\d{8}$/,
  //     message: "电话格式错误",
  //     trigger: "blur"
  //   }
  // ],
  // mail: [{ type: "email", message: "邮箱格式错误", trigger: "blur" }]
});
