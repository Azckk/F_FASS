interface FormItemProps {
  name: string;
  colour: string;
  code: string;
  value: string;
  isEnable: boolean;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };  