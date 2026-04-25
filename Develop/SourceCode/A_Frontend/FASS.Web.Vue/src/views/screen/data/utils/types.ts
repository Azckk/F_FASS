interface FormItemProps {
  carId:string,
  startNodeId:string,
  endNodeId:string,
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
