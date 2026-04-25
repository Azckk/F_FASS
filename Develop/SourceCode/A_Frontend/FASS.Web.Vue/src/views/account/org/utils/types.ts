interface FormItemProps {
  parentId: string;
  parents: Record<string, unknown>[];
  code: string;
  name: string;
  sortNumber: number;
  isEnable: boolean;
  remark: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
