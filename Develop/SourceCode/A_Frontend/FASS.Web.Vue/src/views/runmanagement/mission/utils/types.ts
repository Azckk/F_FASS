interface FormItemProps {
  key: string;
  value: string;
  isDisable: boolean;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
