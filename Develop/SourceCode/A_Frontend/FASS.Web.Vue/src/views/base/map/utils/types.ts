interface FormItemProps {
  username: string;
  code: string;
  name: string;
  nick: string;
  gender: string;
  birthday: string;
  phone: string;
  mail: string;
  avatar: string;
  isSystem: boolean;
  isEnable: boolean;
  remark: string;
  version: string;
  fileName: string;
  fileContent: string;
}
interface FormProps {
  formInline: FormItemProps;
}

interface FormNameProps {
  fileName: string;
}

interface FormNameItemProps {
  formNameInline: FormNameProps;
}

export type { FormItemProps, FormProps, FormNameProps, FormNameItemProps };
