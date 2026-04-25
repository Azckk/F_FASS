interface FormItemProps {
  nodeId: string;
  code: string;
  name: string;
  state: string;
  occupiedCarId: string;
  voltage: string;
  mode: string;
  ip: string;
  port: string;
  protocol: string;
  chargeCode: string;
  chargeId: string;
  current: string;
  isOccupied: boolean;
  isEnable: boolean;
}
interface FormProps {
  formInline: FormItemProps;
}

interface StrategyFormItemProps {
  carMinBattery: string;
  carMaxBattery: string;
  carIdleChargeBattery: string;
  carIdleSecond: string;
  taskAvailableBattery: string;
}
interface StrategyFormProps {
  formInline: StrategyFormItemProps;
}
export type {
  FormItemProps,
  FormProps,
  StrategyFormItemProps,
  StrategyFormProps
};
