interface FormItemProps {
  code: string;
  name: string;
  ip: string;
  port: string;
  protocolType: string;
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
