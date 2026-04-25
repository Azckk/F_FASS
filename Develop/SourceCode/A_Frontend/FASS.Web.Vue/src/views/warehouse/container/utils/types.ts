interface FormItemProps {
  areaId: string;
  isLock: false;
  barcode: string;
  name: string;
  type: string;
  state: string;
  code: string;
  length: number;
  width: number;
  height: number;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
