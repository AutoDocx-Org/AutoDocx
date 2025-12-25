import { Routes } from '@angular/router';
import { TemplateListComponent } from './components/template-list/template-list.component';

export const routes: Routes = [
  { path: '', component: TemplateListComponent },
  { path: '**', redirectTo: '' }
];
