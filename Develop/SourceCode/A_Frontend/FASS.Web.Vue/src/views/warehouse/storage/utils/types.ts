interface FormItemProps {
  areaId: string,
  nodeId: string,
  nodeCode: string,
  isLock: boolean,
  code: string,
  name: string,
  type: string,
  state: string,
  barcode: string,
  sortNumber: number,
  isEnable: boolean
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
