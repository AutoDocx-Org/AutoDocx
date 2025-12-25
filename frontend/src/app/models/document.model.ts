export interface GenerateDocumentRequest {
  templateId: string;
  data: { [key: string]: any };
}

export interface DocumentResponse {
  documentId: string;
  previewUrl: string;
  wordDownloadUrl: string;
  pdfDownloadUrl: string;
  expiresAt: Date;
}
