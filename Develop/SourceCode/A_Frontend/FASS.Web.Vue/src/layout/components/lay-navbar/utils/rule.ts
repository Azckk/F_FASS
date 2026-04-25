import { reactive, ref } from "vue";
import type { FormRules } from "element-plus";

const passwordNew = ref("");
const passwordNewConfirm = ref("");

export function changePassword(pw: string, id: number) {
  if (id === 1) {
    passwordNew.value = pw;
  } else if (id === 2) {
    passwordNewConfirm.value = pw;
  }
}

const equalToPassword = (rule: any, value: any, callback: any) => {
  if (passwordNew.value !== passwordNewConfirm.value) {
    callback(new Error("两次输入的密码不一致"));
  } else {
    callback();
  }
};
export const formRules = reactive<FormRules>({
  username: [{ required: true, message: "必填项", trigger: "blur" }],
  passwordOrigin: [
    { required: true, message: "必填项", trigger: "blur" },
    { min: 6, max: 20, message: "密码长度应为6到20位", trigger: "blur" },
    {
      pattern: /^[\w\-\@\.]*$/,
      message: "密码不能包含特殊符号",
      trigger: "blur"
    }
  ],
  passwordNew: [
    { min: 6, max: 20, message: "密码长度应为6到20位", trigger: "blur" },
    {
      pattern: /^[\w\-\@\.]*$/,
      message: "密码不能包含特殊符号",
      trigger: "blur"
    },
    { required: true, message: "必填项", trigger: "blur" }
  ],
  PasswordNewConfirm: [
    { required: true, message: "必填项", trigger: "blur" },
    { min: 6, max: 20, message: "密码长度应为6到20位", trigger: "blur" },
    {
      pattern: /^[\w\-\@\.]*$/,
      message: "密码不能包含特殊符号",
      trigger: "blur"
    },
    { validator: equalToPassword, trigger: "blur" }
  ]
});
