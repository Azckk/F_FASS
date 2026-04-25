interface FormItemProps {
  "code": string,
  "name": string
  "sortNumber": number
  "isEnable": boolean
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
