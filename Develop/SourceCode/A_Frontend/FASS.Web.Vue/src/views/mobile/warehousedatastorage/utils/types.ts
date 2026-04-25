interface FormItemProps {
  areaCode: string;
  code: string;
  isLock: boolean;
  nodeCode: string;
  barcode: string;
  state: string;
}
interface FormProps {
  formInline: FormItemProps;
  StorageStateList: any[];
}

export type { FormItemProps, FormProps };
