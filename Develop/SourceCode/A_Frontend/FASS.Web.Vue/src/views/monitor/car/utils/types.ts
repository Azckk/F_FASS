interface FormItemProps {
  carTypeId: string;
  prevNodeId: string;
  currNodeId: string;
  nextNodeId: string;
  code: string;
  name: string;
  isEnable: boolean;
  remark: string;
  ipAddress: string;
  port: string;
  type: string;
  manufacturer: string;
  serialNumber: string;
  length: number;
  width: number;
  height: number;
  controlType: string;
  avoidType: string;
  minBattery: string;
  maxBattery: string;
  extend: string;
  isNormal: string;
  isOnline: string;
  angle: string;
  X: string;
  Y: string;
  electricity: string;
  CurrentTaskId: string;
  isAlarm: string;
  speed: string;
  battery: string;
  stopAccept: boolean;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
