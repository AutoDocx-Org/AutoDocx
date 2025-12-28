import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TemplateFieldDto } from '../../models/template.model';

@Component({
    selector: 'app-field-builder',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './field-builder.component.html',
    styleUrls: ['./field-builder.component.scss']
})
export class FieldBuilderComponent implements OnChanges {
    @Input() fields: TemplateFieldDto[] = [];
    @Output() fieldsChange = new EventEmitter<TemplateFieldDto[]>();

    fieldTypes = [
        { value: 'text', label: 'Text' },
        { value: 'textarea', label: 'Text Area' },
        { value: 'number', label: 'Number' },
        { value: 'date', label: 'Date' },
        { value: 'select', label: 'Dropdown' },
        { value: 'radio', label: 'Radio Buttons' },
        { value: 'checkbox', label: 'Checkbox' }
    ];

    editingField: TemplateFieldDto | null = null;
    showFieldForm = false;

    // Field form properties
    fieldKey = '';
    label = '';
    type = 'text';
    isRequired = false;
    placeholder = '';
    options: string[] = [];
    optionsText = '';

    ngOnChanges(changes: SimpleChanges): void {
        if (changes['fields'] && changes['fields'].currentValue) {
            // Ensure we have a proper array reference
            if (!this.fields) {
                this.fields = [];
            }
        }
    }

    addField(): void {
        this.resetFieldForm();
        this.showFieldForm = true;
    }

    editField(field: TemplateFieldDto): void {
        this.editingField = field;
        this.fieldKey = field.fieldKey;
        this.label = field.label;
        this.type = field.type;
        this.isRequired = field.isRequired;
        this.placeholder = field.placeholder;
        this.options = field.options || [];
        this.optionsText = this.options.join('\n');
        this.showFieldForm = true;
    }

    saveField(): void {
        if (!this.validateField()) {
            return;
        }

        const field: TemplateFieldDto = {
            fieldKey: this.fieldKey,
            label: this.label,
            type: this.type,
            isRequired: this.isRequired,
            placeholder: this.placeholder,
            options: this.needsOptions() ? this.optionsText.split('\n').filter(o => o.trim()) : undefined,
            order: this.editingField ? this.editingField.order : this.fields.length + 1
        };

        if (this.editingField) {
            const index = this.fields.indexOf(this.editingField);
            this.fields[index] = field;
        } else {
            this.fields.push(field);
        }

        console.log('Emitting fields:', [...this.fields]);
        this.fieldsChange.emit([...this.fields]);
        this.cancelFieldForm();
    }

    deleteField(field: TemplateFieldDto): void {
        const index = this.fields.indexOf(field);
        if (index > -1) {
            this.fields.splice(index, 1);
            this.reorderFields();
            this.fieldsChange.emit([...this.fields]);
        }
    }

    moveFieldUp(field: TemplateFieldDto): void {
        const index = this.fields.indexOf(field);
        if (index > 0) {
            [this.fields[index - 1], this.fields[index]] = [this.fields[index], this.fields[index - 1]];
            this.reorderFields();
            this.fieldsChange.emit([...this.fields]);
        }
    }

    moveFieldDown(field: TemplateFieldDto): void {
        const index = this.fields.indexOf(field);
        if (index < this.fields.length - 1) {
            [this.fields[index], this.fields[index + 1]] = [this.fields[index + 1], this.fields[index]];
            this.reorderFields();
            this.fieldsChange.emit([...this.fields]);
        }
    }

    cancelFieldForm(): void {
        this.showFieldForm = false;
        this.editingField = null;
        this.resetFieldForm();
    }

    needsOptions(): boolean {
        return this.type === 'select' || this.type === 'radio';
    }

    private validateField(): boolean {
        if (!this.fieldKey.trim()) {
            alert('Field key is required');
            return false;
        }

        if (!this.label.trim()) {
            alert('Field label is required');
            return false;
        }

        if (!this.placeholder.trim()) {
            alert('Placeholder is required');
            return false;
        }

        // Check for duplicate field keys
        const duplicateKey = this.fields.find(
            f => f.fieldKey === this.fieldKey && f !== this.editingField
        );
        if (duplicateKey) {
            alert('Field key must be unique');
            return false;
        }

        if (this.needsOptions() && !this.optionsText.trim()) {
            alert('Options are required for dropdown and radio fields');
            return false;
        }

        return true;
    }

    private resetFieldForm(): void {
        this.fieldKey = '';
        this.label = '';
        this.type = 'text';
        this.isRequired = false;
        this.placeholder = '';
        this.options = [];
        this.optionsText = '';
    }

    private reorderFields(): void {
        this.fields.forEach((field, index) => {
            field.order = index + 1;
        });
    }
}
