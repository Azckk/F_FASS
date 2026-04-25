interface FormItemProps {
  id: string;
  name: string;
  code: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
