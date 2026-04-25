interface indexItemProps {
  id: string;
  nodeId: string;
  edgeId: string;
  kind: string;
  type: string;
  code: string;
  name: string;
  isLock: false;
  maxCar: string;
}
interface indexProps {
  formInline: indexItemProps;
}

interface FormItemProps {
  actionType: string;
  operationDescription: string;
  blockingType: string;
  sortNumber: number;
  edgeId: string;
}
interface FormProps {
  formInline: FormItemProps;
  operation: Function;
}

interface sitelocationItemProps {
  x: string;
  y: string;
  theta: string;
  allowedDeviationXY: string;
  allowedDeviationTheta: string;
  mapId: string;
  mapDescription: string;
}
interface sitelocationProps {
  formInline: sitelocationItemProps;
}

export type { indexProps, FormItemProps, FormProps, sitelocationProps };
