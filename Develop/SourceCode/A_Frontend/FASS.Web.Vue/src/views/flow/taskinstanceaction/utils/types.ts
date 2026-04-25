interface FormItemProps {
  actionType: string;
  actionDescription: string;
  blockingType: string;
  sortNumber: number;
  taskInstanceProcessId: string;
  state: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
