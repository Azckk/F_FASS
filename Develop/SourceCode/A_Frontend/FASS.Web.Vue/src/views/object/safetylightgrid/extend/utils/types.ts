interface indexItemProps {
  id: string;
  // nodeId: string;
  // kind: string;
  // type: string;
  // code: string;
  // name: string;
  // isLock: false;
  // maxCar: string;
}
interface indexProps {
  formInline: indexItemProps;
}

interface FormItemProps {
  actionType: string;
  operationDescription: string;
  blockageType: string;
  sortNumber: string;
}
interface FormProps {
  formInline: FormItemProps;
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

interface operationItemProps {
  openCloseSignal: string;
  station: string;
  remark: string;
  safetyLightGridsId: string;
}
interface operationProps {
  formInline: operationItemProps;
}

export type {
  indexProps,
  FormItemProps,
  FormProps,
  sitelocationProps,
  operationProps
};
