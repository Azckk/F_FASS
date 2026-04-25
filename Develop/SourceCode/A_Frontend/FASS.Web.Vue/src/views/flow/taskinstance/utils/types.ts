interface FormItemProps {
  code: string;
  name: string;
  type: string;
  carTypeId: string;
  priority: number;
  taskTemplateId: string;
  carId: string | null;
  nodes: Array<string>;
  edges: Array<string>;
  state: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
