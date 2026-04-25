interface FormItemProps {
    "nodeId": string,
    "code": string
    "name": string
  "state": string,
  occupiedCarId: string,
  }
  interface FormProps {
    formInline: FormItemProps;
  }
  
  export type { FormItemProps, FormProps };