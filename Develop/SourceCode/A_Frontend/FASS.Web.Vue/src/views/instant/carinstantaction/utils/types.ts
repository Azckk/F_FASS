interface FormItemProps {
  carId: string;
  actionType: string;
  actionDescription: string;
  blockingType: string;
  remark: string;
  state: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
