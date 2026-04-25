interface FormInline {
  colour: string;
  id: string;
  name: string;
  isEnable: boolean;
  isLock: boolean;
  sortNumber: number;
  value: string;
  keyValue: string;
  type: string;
}

interface FormProps {
  formInline: FormInline;
}

export type { FormInline, FormProps };
