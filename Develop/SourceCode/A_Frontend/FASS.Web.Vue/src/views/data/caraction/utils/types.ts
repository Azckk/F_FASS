interface FormItemProps {
  carTypeId: string
  "code": string,
  "name": string
  "sortNumber": number
  "isEnable": boolean
  createAt: string
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
