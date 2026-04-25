interface FormItemProps {
  dictId: string;
  code: string;
  name: string;
  value: number;
  sortNumber: number;
  param: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
