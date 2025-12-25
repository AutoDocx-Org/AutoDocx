export interface Template {
  id: string;
  name: string;
  description?: string;
  wordFilePath: string;
  createdAt: Date;
  updatedAt: Date;
  fields: TemplateField[];
}

export interface TemplateField {
  id: string;
  fieldKey: string;
  label: string;
  type: string;
  isRequired: boolean;
  options?: string[];
  placeholder: string;
  order: number;
}

export interface CreateTemplateRequest {
  name: string;
  description?: string;
  fields: TemplateFieldDto[];
}

export interface TemplateFieldDto {
  fieldKey: string;
  label: string;
  type: string;
  isRequired: boolean;
  options?: string[];
  placeholder: string;
  order: number;
}
