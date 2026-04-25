import { reactive } from "vue";
import type { FormRules } from "element-plus";

export const formRules = reactive(<FormRules>{
  username: [{ required: true, message: "必填项", trigger: "blur" }],
  phone: [
    {
      pattern:
        /^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\d{8}$/,
      message: "电话格式错误",
      trigger: "blur"
    }
  ],
  mail: [{ type: "email", message: "邮箱格式错误", trigger: "blur" }]
});

// export const nameRules = reactive(<FormRules>{
//   fileName: [{ required: true, message: "必填项", trigger: "blur" }]
// });

export const nameRules = reactive(<FormRules>{
  fileName: [
    {
      required: true,
      message: "文件名不能为空",
      trigger: "blur"
    },
    {
      pattern: /^(?!\.json$).*\.json$/, // 校验后缀为 .json，但不能直接是 .json
      message: "文件名必须以 .json 结尾",
      trigger: "blur"
    }
  ]
});
