interface FormItemProps {
  areaId: string,
  nodeId: string,
  nodeCode: string,
  isLock: false,
  code: string,
  name: string,
  type: string,
  state: string,
  barcode: string,
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
