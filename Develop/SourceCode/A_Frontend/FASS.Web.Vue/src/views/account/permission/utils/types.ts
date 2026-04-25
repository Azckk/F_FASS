interface FormItemProps {
  parentId: string;
  parents: Record<string, unknown>[];
  type: string;
  code: string;
  name: string;
  icon: string;
  method: string;
  target: string;
  address: string;
  sortNumber: number;
  isEnable: boolean;
  remark: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
