interface FormItemProps {
  sortNumber: number;
  isEnable: boolean;
  isDelete: boolean;
  remark: string;
  extend: string;
  type: string;
  code: string;
  name: string;
  prevNodeId: string;
  nextNodeId: string;
  isAdvance: boolean;
  state: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
