interface FormItemProps {
  code: string;
  name: string;
  isEnable: boolean;
  remark: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
