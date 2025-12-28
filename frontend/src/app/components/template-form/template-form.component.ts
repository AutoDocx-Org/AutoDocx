import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TemplateService } from '../../services/template.service';
import { Template, TemplateFieldDto } from '../../models/template.model';
import { FieldBuilderComponent } from '../field-builder/field-builder.component';

@Component({
    selector: 'app-template-form',
    standalone: true,
    imports: [CommonModule, FormsModule, FieldBuilderComponent],
    templateUrl: './template-form.component.html',
    styleUrls: ['./template-form.component.scss']
})
export class TemplateFormComponent implements OnInit {
    templateId: string | null = null;
    isEditMode = false;
    
    name = '';
    description = '';
    fields: TemplateFieldDto[] = [];
    wordFile: File | null = null;
    wordFileName = '';
    
    loading = false;
    saving = false;
    error: string | null = null;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private templateService: TemplateService
    ) { }

    ngOnInit(): void {
        this.templateId = this.route.snapshot.paramMap.get('id');
        this.isEditMode = !!this.templateId;

        if (this.isEditMode && this.templateId) {
            this.loadTemplate(this.templateId);
        }
    }

    loadTemplate(id: string): void {
        this.loading = true;
        this.templateService.getById(id).subscribe({
            next: (template) => {
                this.name = template.name;
                this.description = template.description || '';
                this.fields = template.fields.map(f => ({
                    fieldKey: f.fieldKey,
                    label: f.label,
                    type: f.type,
                    isRequired: f.isRequired,
                    options: f.options,
                    placeholder: f.placeholder,
                    order: f.order
                }));
                this.wordFileName = template.wordFilePath.split('/').pop() || '';
                this.loading = false;
            },
            error: (error) => {
                this.error = 'Failed to load template';
                this.loading = false;
                console.error(error);
            }
        });
    }

    onFileSelected(event: any): void {
        const file = event.target.files[0];
        if (file) {
            if (file.type === 'application/vnd.openxmlformats-officedocument.wordprocessingml.document') {
                this.wordFile = file;
                this.wordFileName = file.name;
                this.error = null;
            } else {
                this.error = 'Please select a valid Word document (.docx)';
                this.wordFile = null;
                this.wordFileName = '';
            }
        }
    }

    onFieldsChanged(fields: TemplateFieldDto[]): void {
        this.fields = fields;
        console.log('Fields updated:', this.fields);
    }

    save(): void {
        console.log('Saving template with fields:', this.fields);
        
        if (!this.validate()) {
            return;
        }

        this.saving = true;
        this.error = null;

        const request = {
            name: this.name,
            description: this.description,
            fields: this.fields
        };

        if (this.isEditMode && this.templateId) {
            this.templateService.update(this.templateId, request, this.wordFile || undefined).subscribe({
                next: () => {
                    this.router.navigate(['/templates']);
                },
                error: (error) => {
                    this.error = 'Failed to update template';
                    this.saving = false;
                    console.error(error);
                }
            });
        } else {
            if (!this.wordFile) {
                this.error = 'Please select a Word document';
                this.saving = false;
                return;
            }

            this.templateService.create(request, this.wordFile).subscribe({
                next: () => {
                    this.router.navigate(['/templates']);
                },
                error: (error) => {
                    this.error = 'Failed to create template';
                    this.saving = false;
                    console.error(error);
                }
            });
        }
    }

    validate(): boolean {
        if (!this.name.trim()) {
            this.error = 'Template name is required';
            return false;
        }

        if (this.fields.length === 0) {
            this.error = 'At least one field is required';
            return false;
        }

        if (!this.isEditMode && !this.wordFile) {
            this.error = 'Word document is required';
            return false;
        }

        return true;
    }

    cancel(): void {
        this.router.navigate(['/templates']);
    }
}
