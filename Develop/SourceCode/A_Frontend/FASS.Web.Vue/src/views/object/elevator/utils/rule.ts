import { reactive } from "vue";
import type { FormRules } from "element-plus";
const checkBattery = (rule: any, value: any, callback: any) => {
  if (!value) {
    return callback(new Error("请输入"));
  }
  if (!Number.isInteger(Number(value))) {
    return callback(new Error("请输入数字"));
  }
  if (value < 0 || value > 100) {
    return callback(new Error("取值范围0-100"));
  }
  callback();
};
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
// 端口
const checkPort = (rule: any, value: any, callback: any) => {
  if (!value) {
    return callback(new Error("请输入端口号"));
  }
  const portNumber = Number(value);
  if (!Number.isInteger(portNumber) || portNumber < 0 || portNumber > 65535) {
    return callback(new Error("请输入有效的端口号（0-65535）"));
  }
  callback();
};

export const formRules = reactive(<FormRules>{
  name: [{ required: true, message: "必填项", trigger: "blur" }],
  // name: [{ required: true, message: "必填项", trigger: "blur" }],
  ip: [
    { required: true, message: "必填项", trigger: "blur" },
    { validator: checkIP, trigger: "blur" }
  ],
  port: [
    { required: true, message: "必填项", trigger: "blur" },
    { validator: checkPort, trigger: "blur" }
  ],
  protocolType: [{ required: true, message: "必填项", trigger: "blur" }],
  code: [{ required: true, message: "必填项", trigger: "blur" }]
});

export const strategyRules = reactive(<FormRules>{
  carMinBattery: [
    { required: true, message: "必填项", trigger: "blur" },
    { validator: checkBattery, trigger: "blur" }
  ],
  carMaxBattery: [
    { required: true, message: "必填项", trigger: "blur" },
    { validator: checkBattery, trigger: "blur" }
  ],
  carIdleChargeBattery: [
    { required: true, message: "必填项", trigger: "blur" },
    { validator: checkBattery, trigger: "blur" }
  ],
  carIdleSecond: [
    { required: true, message: "必填项", trigger: "blur" },
    { validator: checkBattery, trigger: "blur" }
  ],
  taskAvailableBattery: [
    { required: true, message: "必填项", trigger: "blur" },
    { validator: checkBattery, trigger: "blur" }
  ]
});
