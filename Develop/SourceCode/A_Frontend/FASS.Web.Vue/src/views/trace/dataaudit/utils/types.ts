interface FormItemProps {
  tableName: string;
  entityState: string;
  originalValue: string;
  currentValue: string;
  currentAction: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
