interface FormItemProps {
  userLogDayLimit: string;
  auditLogDayLimit: string;
  diaryDayLimit: string;
  alarmDayLimit: string;
  carInstantActionDayLimit: string;
  taskInstanceDayLimit: string;
  workDayLimit: string;
  carMinBattery: string;
  carMaxBattery: string;
  carIdleSecond: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
