interface FormItemProps {
  areaCode: string;
  code: string;
  isLock: boolean;
  state: string;
  nodeCode: string;
  barcode: string;
}
interface FormProps {
  formInline: FormItemProps;
  StorageStateList: any[];
}

export type { FormItemProps, FormProps };
