import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Template, CreateTemplateRequest } from '../models/template.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TemplateService {
  private apiUrl = `${environment.apiUrl}/templates`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Template[]> {
    return this.http.get<Template[]>(this.apiUrl);
  }

  getById(id: string): Observable<Template> {
    return this.http.get<Template>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateTemplateRequest, wordFile: File): Observable<Template> {
    const formData = new FormData();
    formData.append('name', request.name);
    if (request.description) {
      formData.append('description', request.description);
    }
    formData.append('fields', JSON.stringify(request.fields));
    formData.append('wordFile', wordFile);

    return this.http.post<Template>(this.apiUrl, formData);
  }

  update(id: string, request: CreateTemplateRequest, wordFile?: File): Observable<Template> {
    const formData = new FormData();
    formData.append('name', request.name);
    if (request.description) {
      formData.append('description', request.description);
    }
    formData.append('fields', JSON.stringify(request.fields));
    if (wordFile) {
      formData.append('wordFile', wordFile);
    }

    return this.http.put<Template>(`${this.apiUrl}/${id}`, formData);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
