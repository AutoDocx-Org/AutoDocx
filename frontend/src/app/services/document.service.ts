import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenerateDocumentRequest, DocumentResponse } from '../models/document.model';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class DocumentService {
    private apiUrl = `${environment.apiUrl}/documents`;

    constructor(private http: HttpClient) { }

    generate(request: GenerateDocumentRequest): Observable<DocumentResponse> {
        return this.http.post<DocumentResponse>(`${this.apiUrl}/generate`, request);
    }

    downloadWord(documentId: string): Observable<Blob> {
        return this.http.get(`${this.apiUrl}/${documentId}/download/word`, {
            responseType: 'blob'
        });
    }

    downloadPdf(documentId: string): Observable<Blob> {
        return this.http.get(`${this.apiUrl}/${documentId}/download/pdf`, {
            responseType: 'blob'
        });
    }

    getPreviewUrl(documentId: string): string {
        return `${environment.apiUrl}/documents/${documentId}/preview`;
    }
}
