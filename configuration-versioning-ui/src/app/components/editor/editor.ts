import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';

import { Config } from '../../services/config';
import { Save } from '../../models/save.model';
import { Version } from '../../models/version.model';

@Component({
  selector: 'app-editor',
  imports: [ReactiveFormsModule],
  templateUrl: './editor.html',
  styleUrl: './editor.css'
})
export class Editor {

  private readonly configService = inject(Config);
  private readonly fb = inject(FormBuilder);

  message = '';

  form = this.fb.group({
    data: [`{"servers": [ "server1", "server2"]}`, Validators.required],
    createdBy: ['',Validators.required]
  });

  save(): void {

  if (this.form.invalid) {
    this.form.markAllAsTouched();
    return;
  }

  const request: Save = {
    data: this.form.value.data ?? '',
    createdBy: this.form.value.createdBy ?? ''
  };

  this.configService.saveConfiguration(request)
    .subscribe({
      next: (response: Version) => {
        this.message =`Version ${response.versionNumber} saved successfully.`;
        this.form.reset({
          data: '',
          createdBy: ''
        });
      },

      error: (error) => {
        console.error(error);
        this.message = 'Failed to save configuration.';
      }
    });
}

}