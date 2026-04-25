interface FormItemProps {
  "code": string,
  "name": string
  "type": string
  "state": string
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
