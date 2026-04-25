interface FormItemProps {
  "code": string,
  "name": string
  "type": string
  "state": string
  containerId: string,
  task: string,
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
