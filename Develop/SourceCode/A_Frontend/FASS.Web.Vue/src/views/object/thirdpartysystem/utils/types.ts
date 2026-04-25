interface FormItemProps {
  systemName: string;
  name: string;
  sortNumber: number;
  isEnable: boolean;
  isDelete: boolean;
  remark: string;
  extend: string;
  communicatioMode: string;
  url: string;
  method: string;
  headers: string;
  queryParams: string;
  responseParams: string;
  state: string;
  isRetransmit: boolean;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
