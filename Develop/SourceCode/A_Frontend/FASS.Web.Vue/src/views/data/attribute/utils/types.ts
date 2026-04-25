interface FormItemProps {
  "nodeId": string,
  "code": string,
  "name": string,
  "state": string,
  extend: string,
  remark: string,
  isEnable: string,
  type: string
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };  