interface FormItemProps {
  actionType: string
  actionDescription: string
  blockingType: string
  sortNumber: number,
  taskTemplateProcessId: string
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
