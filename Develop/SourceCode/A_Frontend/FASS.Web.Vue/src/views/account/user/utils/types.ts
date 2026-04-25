interface FormItemProps {
  username: string;
  name: string;
  nick: string;
  gender: string;
  birthday: string;
  phone: string;
  mail: string;
  avatar: string;
  isSystem: boolean;
  isEnable: boolean;
  remark: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
