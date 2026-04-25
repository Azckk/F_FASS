import { reactive } from "vue";
import type { FormRules } from "element-plus";

// export const formData = reactive({
//   srcStorageId: "",
//   destStorageId: "",
//   taskTemplateId: "",
//   state: "",
//   carId: ""
// });
// 自定义校验函数
// const validateStorageIds = (rule: any, value: string, callback: Function) => {
//   const form = rule.form; // 获取表单实例
//   if (form.srcStorageId === form.destStorageId) {
//     callback(new Error("起点和终点不能为同一库位"));
//   } else {
//     callback();
//   }
// };

export const formRules = reactive(<FormRules>{
  taskTemplateId: [{ required: true, message: "必填项", trigger: "blur" }],
  callStorageId: [{ required: true, message: "必填项", trigger: "blur" }],
  srcStorageId: [
    { required: true, message: "必填项", trigger: "blur" }
    // { validator: validateStorageIds, trigger: "blur" }
  ],
  destStorageId: [
    { required: true, message: "必填项", trigger: "blur" }

    // { validator: validateStorageIds, trigger: "blur" }
  ],
  // carId: [{ required: true, message: "必填项", trigger: "blur" }],
  state: [{ required: true, message: "必填项", trigger: "blur" }]
});
// export const formRules2 = reactive(<FormRules>{
//   taskTemplateId: [{ required: true, message: "必填项", trigger: "blur" }],
//   carId: [{ required: true, message: "必填项", trigger: "blur" }]
// });
