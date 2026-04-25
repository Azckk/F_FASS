interface FormItemProps {
  code: string
  name: string
  type: string
  carTypeId: string
  priority: number
  isLoop: boolean,
  timeout: number
  isEnable: boolean,
  remark: string,
  description: string,
  value: string
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
