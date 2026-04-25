import { reactive } from "vue";
import type { FormRules } from "element-plus";
// IP 地址校验函数
const checkIP = (rule: any, value: any, callback: any) => {
  if (!value) {
    return callback(new Error("请输入 IP 地址"));
  }
  const ipPattern =
    /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
  if (!ipPattern.test(value)) {
    return callback(new Error("请输入有效的 IP 地址"));
  }
  callback();
};
export const formRules = reactive(<FormRules>{
  carTypeId: [{ required: true, message: "必填项", trigger: "blur" }],
  code: [{ required: true, message: "必填项", trigger: "blur" }],
  name: [{ required: true, message: "必填项", trigger: "blur" }],
  type: [{ required: true, message: "必填项", trigger: "blur" }],
  ipAddress: [
    { required: true, message: "必填项", trigger: "blur" },
    { validator: checkIP, trigger: "blur" }
  ],
  port: [{ required: true, message: "必填项", trigger: "blur" }],
  width: [{ required: true, message: "必填项", trigger: "blur" }],
  height: [{ required: true, message: "必填项", trigger: "blur" }],
  length: [{ required: true, message: "必填项", trigger: "blur" }],
  controlType: [{ required: true, message: "必填项", trigger: "blur" }],
  avoidType: [{ required: true, message: "必填项", trigger: "blur" }],
  minBattery: [{ required: true, message: "必填项", trigger: "blur" }],
  maxBattery: [{ required: true, message: "必填项", trigger: "blur" }]
});
