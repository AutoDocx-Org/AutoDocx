import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { TemplateService } from '../../services/template.service';
import { DocumentService } from '../../services/document.service';
import { Template, TemplateFieldDto } from '../../models/template.model';
import { DocumentResponse } from '../../models/document.model';

@Component({
    selector: 'app-generate-document',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './generate-document.component.html',
    styleUrls: ['./generate-document.component.scss']
})
export class GenerateDocumentComponent implements OnInit {
    template: Template | null = null;
    formData: { [key: string]: any } = {};
    
    loading = false;
    generating = false;
    generated = false;
    error: string | null = null;
    
    documentResponse: DocumentResponse | null = null;
    previewUrl: SafeResourceUrl | null = null;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private templateService: TemplateService,
        private documentService: DocumentService,
        private sanitizer: DomSanitizer
    ) { }

    ngOnInit(): void {
        const templateId = this.route.snapshot.paramMap.get('id');
        if (templateId) {
            this.loadTemplate(templateId);
        } else {
            this.error = 'Template ID is required';
        }
    }

    loadTemplate(id: string): void {
        this.loading = true;
        this.templateService.getById(id).subscribe({
            next: (template) => {
                console.log('Template loaded:', template);
                console.log('Template fields:', template.fields);
                console.log('Number of fields:', template.fields?.length || 0);
                
                if (!template.fields || template.fields.length === 0) {
                    console.error('Template has no fields!');
                    this.error = 'This template has no fields defined. Please edit the template to add fields.';
                    this.loading = false;
                    return;
                }
                
                this.template = template;
                this.initializeFormData();
                this.loading = false;
            },
            error: (error) => {
                this.error = 'Failed to load template';
                this.loading = false;
                console.error('Error loading template:', error);
            }
        });
    }

    initializeFormData(): void {
        if (!this.template) return;
        
        console.log('Initializing form data for fields:', this.template.fields);
        this.formData = {};
        
        if (!this.template.fields || this.template.fields.length === 0) {
            console.error('Cannot initialize form data - no fields available');
            return;
        }
        
        this.template.fields.forEach(field => {
            console.log('Initializing field:', field.fieldKey, 'type:', field.type);
            if (field.type === 'checkbox') {
                this.formData[field.fieldKey] = false;
            } else {
                this.formData[field.fieldKey] = '';
            }
        });
        console.log('Initial form data:', this.formData);
    }

    generateDocument(): void {
        console.log('Generate document called with formData:', this.formData);
        
        if (!this.validate()) {
            return;
        }

        this.generating = true;
        this.error = null;
        this.generated = false;

        const request = {
            templateId: this.template!.id,
            data: this.formData
        };

        console.log('Sending request:', request);

        this.documentService.generate(request).subscribe({
            next: (response) => {
                this.documentResponse = response;
                this.previewUrl = this.sanitizer.bypassSecurityTrustResourceUrl(response.previewUrl);
                this.generated = true;
                this.generating = false;
            },
            error: (error) => {
                this.error = 'Failed to generate document';
                this.generating = false;
                console.error(error);
            }
        });
    }

    validate(): boolean {
        if (!this.template) return false;

        for (const field of this.template.fields) {
            if (field.isRequired) {
                const value = this.formData[field.fieldKey];
                if (value === null || value === undefined || value === '') {
                    this.error = `${field.label} is required`;
                    return false;
                }
            }
        }

        return true;
    }

    downloadWord(): void {
        if (!this.documentResponse) return;

        this.documentService.downloadWord(this.documentResponse.documentId).subscribe({
            next: (blob) => {
                this.downloadFile(blob, 'document.docx');
            },
            error: (error) => {
                this.error = 'Failed to download Word document';
                console.error(error);
            }
        });
    }

    downloadPdf(): void {
        if (!this.documentResponse) return;

        this.documentService.downloadPdf(this.documentResponse.documentId).subscribe({
            next: (blob) => {
                this.downloadFile(blob, 'document.pdf');
            },
            error: (error) => {
                this.error = 'Failed to download PDF';
                console.error(error);
            }
        });
    }

    private downloadFile(blob: Blob, filename: string): void {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        link.click();
        window.URL.revokeObjectURL(url);
    }

    reset(): void {
        this.generated = false;
        this.documentResponse = null;
        this.previewUrl = null;
        this.initializeFormData();
    }

    backToTemplates(): void {
        this.router.navigate(['/templates']);
    }
}
