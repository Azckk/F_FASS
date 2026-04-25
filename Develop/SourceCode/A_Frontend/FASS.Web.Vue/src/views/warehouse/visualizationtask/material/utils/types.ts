interface FormItemProps {
    code: string,
    name:string,
    isLock: boolean,
    type: string,
    state: string,
    barcode: string,
    batch: string,
    spec: string,
    unit: string,
    quantity:  string
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
