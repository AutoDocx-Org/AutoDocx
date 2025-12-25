import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TemplateService } from '../../services/template.service';
import { Template } from '../../models/template.model';

@Component({
    selector: 'app-template-list',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './template-list.component.html',
    styleUrls: ['./template-list.component.scss']
})
export class TemplateListComponent implements OnInit {
    templates: Template[] = [];
    loading = false;
    error: string | null = null;

    constructor(
        private templateService: TemplateService,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.loadTemplates();
    }

    loadTemplates(): void {
        this.loading = true;
        this.templateService.getAll().subscribe({
            next: (templates) => {
                this.templates = templates;
                this.loading = false;
            },
            error: (error) => {
                this.error = 'Failed to load templates';
                this.loading = false;
                console.error(error);
            }
        });
    }

    createTemplate(): void {
        this.router.navigate(['/templates/create']);
    }

    editTemplate(id: string): void {
        this.router.navigate(['/templates/edit', id]);
    }

    deleteTemplate(id: string): void {
        if (confirm('Are you sure you want to delete this template?')) {
            this.templateService.delete(id).subscribe({
                next: () => {
                    this.loadTemplates();
                },
                error: (error) => {
                    this.error = 'Failed to delete template';
                    console.error(error);
                }
            });
        }
    }

    useTemplate(id: string): void {
        this.router.navigate(['/generate', id]);
    }
}
