interface FormItemProps {
  name: string,
  description: string,
  value: string,
  isEnable: boolean
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };  