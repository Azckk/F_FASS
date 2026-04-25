import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  username: [{ required: true, message: "必填项", trigger: "blur" }],
  name: [{ required: true, message: "必填项", trigger: "blur" }],
  gender: [{ required: true, message: "必填项", trigger: "blur" }],
  birthday: [{ required: true, message: "必填项", trigger: "blur" }],
  // phone: [{ required: true, message: "必填项", trigger: "blur" }],
  phone: [
    {
      required: true,
      pattern:
        /^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\d{8}$/,
      message: "电话格式错误",
      trigger: "blur"
    }
  ],
  mail: [
    { required: true, type: "email", message: "邮箱格式错误", trigger: "blur" }]
});
