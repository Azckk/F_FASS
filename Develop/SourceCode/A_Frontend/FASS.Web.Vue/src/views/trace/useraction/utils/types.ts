interface FormItemProps {
  controller: string;
  action: string;
  watch: string;
  requestUrl: string;
  requestToken: string;
  responseCode: string;
  userAgent: string;
  ipAddress: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
