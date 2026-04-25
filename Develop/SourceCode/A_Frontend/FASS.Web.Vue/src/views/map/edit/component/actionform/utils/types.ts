interface FormItemProps {
  "actionId": string,
  "actionType": string
  "actionDescription": string
  "blockingType": string
  "sortNumber": number
  "actionParameters":Array<{
    key: string,
    value: string,
  }>
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
