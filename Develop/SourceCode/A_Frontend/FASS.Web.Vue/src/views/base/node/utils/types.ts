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
  kind: string;
  type: string;
  isLock: boolean;
  sequenceId: string;
  nodeDescription: string;
}
interface FormProps {
  formInline: FormItemProps;
}
interface locationFrom {
  x: string;
  y: string;
  theta: string;
  allowedDeviationXY: string;
  allowedDeviationTheta: string;
  mapId: string;
  sequenceId: string;
}
interface locationFormProps {
  formInline: locationFrom;
}

interface indexItemProps {
  id: string;
  kind: string;
  type: string;
  code: string;
  name: string;
  isLock: false;
  maxCar: string;
}
interface indexProps {
  formInline?: indexItemProps;
}

export type {
  FormItemProps,
  FormProps,
  locationFrom,
  locationFormProps,
  indexProps
};
