interface FormItemProps {
  code: string;
  name: string;
  type: string;
  carTypeId: string;
  priority: number;
  taskTemplateId: string;
  carId: string;
  nodes: Array<string>;
  edges: Array<string>;
  destNodeId: string;
  srcNodeId: string;
  srcAreaId?: string;
  destAreaId?: string;
  isLoop?: boolean;
  condition?: string;
  srcNodeCode?: string;
  destNodeCode?: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
