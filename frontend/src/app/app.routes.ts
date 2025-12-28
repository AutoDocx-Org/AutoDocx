import { Routes } from '@angular/router';
import { TemplateListComponent } from './components/template-list/template-list.component';
import { TemplateFormComponent } from './components/template-form/template-form.component';
import { GenerateDocumentComponent } from './components/generate-document/generate-document.component';

export const routes: Routes = [
    { path: '', redirectTo: '/templates', pathMatch: 'full' },
    { path: 'templates', component: TemplateListComponent },
    { path: 'templates/create', component: TemplateFormComponent },
    { path: 'templates/edit/:id', component: TemplateFormComponent },
    { path: 'generate/:id', component: GenerateDocumentComponent },
    { path: '**', redirectTo: '/templates' }
];
