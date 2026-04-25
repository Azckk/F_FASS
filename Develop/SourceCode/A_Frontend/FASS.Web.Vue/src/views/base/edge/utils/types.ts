interface FormItemProps {
  kind: string
  type: string
  code: string
  name: string
  isLock: boolean,
  // maxCar: string
  isOneway: boolean,
  sequenceId: string
  released: boolean,
  startNodeCode: string
  endNodeCode: string
  maxSpeed: number,
  maxHeight: number,
  minHeight: number,
  orientation: number,
  orientationType: string
  rotationAllowed: string
  maxRotationSpeed: number,
  startNodeId: string
  endNodeId: string
  length: number,
  direction: string
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
