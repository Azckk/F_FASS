interface FormItemProps {
  "fromCarId": string,
  "toCarId": string
  "count": number,
  "isMutual": boolean,
  "IsFinish": boolean
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
