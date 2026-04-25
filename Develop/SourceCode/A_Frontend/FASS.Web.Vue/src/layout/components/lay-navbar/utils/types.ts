interface FormItemProps {
  username: string;
  passwordOrigin: string;
  passwordNew: string;
  PasswordNewConfirm: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
