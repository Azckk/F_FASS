interface FormItemProps {
  kind: string
  type: string
  code: string
  name: string
  isLock: boolean,
  maxCar: string
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
