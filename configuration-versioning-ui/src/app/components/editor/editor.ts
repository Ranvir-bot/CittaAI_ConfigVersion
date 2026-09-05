import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { Config } from '../../services/config';
import { Save } from '../../models/save.model';
import { Version } from '../../models/version.model';
import { SaveResponse } from '../../models/save-response.model';

@Component({
  selector: 'app-editor',
  imports: [ReactiveFormsModule],
  templateUrl: './editor.html',
  styleUrl: './editor.css'
})
export class Editor {

  private readonly configService = inject(Config);
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);

  message = '';
  configurationId?: number;
  baseVersionId?: number;

  form = this.fb.group({
    data: ['', Validators.required],
    createdBy: ['',Validators.required]
  });

  constructor() {
    this.loadVersion();
  }

  loadVersion(): void {
    const versionId = Number(this.route.snapshot.queryParamMap.get('versionId'));

    if (!versionId) {
      return;
    }

    this.configService.getVersionById(versionId)
      .subscribe({
        next: (response: Version) => {
          this.configurationId = response.configurationId;
          this.baseVersionId = response.id;
          this.form.patchValue({ data: response.configurationJson, createdBy: response.author });

          console.log('Loaded version:', response);
           console.log('Base Version ID:', this.baseVersionId);
        },

        error: (error) => {
          console.error('Error loading version:', error);
        }
      });
  }


  save(): void {

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const request: Save = {
      data: this.form.value.data ?? '',
      createdBy: this.form.value.createdBy ?? '',
      configurationId: this.configurationId ? this.configurationId : undefined,
      baseVersionId: this.baseVersionId,
    };

    this.configService.saveConfiguration(request)
    .subscribe({
        next: (response: SaveResponse) => {
          console.log("Response for save", response);
          this.message = `${response.message}`;
          this.form.reset({ data: '', createdBy: '' });
        },

        error: (error) => {
          console.error(error);
          this.message = error.error?.message || 'Failed to save configuration.';
        }
      });
  }

}