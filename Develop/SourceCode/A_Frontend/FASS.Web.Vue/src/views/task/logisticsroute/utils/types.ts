interface FormItemProps {
  code: string
  name: string
  type: string
  carTypeId: string
  priority: number
  isLoop: boolean,
  timeout: number
  isEnable: boolean,
  destAreaCode: string,
  srcAreaCode: string,
  srcAreaId: string,
  destAreaId: string,
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
