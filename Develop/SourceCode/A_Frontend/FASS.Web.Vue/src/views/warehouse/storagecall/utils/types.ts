interface FormItemProps {
  storageId: string;
  callMode: string;
  areaId: string;
  materialId: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
