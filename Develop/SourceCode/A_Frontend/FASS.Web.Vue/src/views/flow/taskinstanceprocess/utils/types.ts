interface FormItemProps {
  nodeId: string;
  code: string;
  name: string;
  type: string;
  sortNumber: number;
  taskTemplateId: string;
  state: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
