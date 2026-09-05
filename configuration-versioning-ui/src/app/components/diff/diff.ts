import { Component, inject, signal } from '@angular/core';
import { AsyncPipe, JsonPipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { Config } from '../../services/config';
import { DiffService } from '../../services/diff.service';

import { Version } from '../../models/version.model';
import { DiffItem } from '../../models/diff-item.model';

@Component({
  selector: 'app-diff',
  imports: [ReactiveFormsModule, AsyncPipe, JsonPipe],
  templateUrl: './diff.html',
  styleUrl: './diff.css'
})
export class Diff {

  private readonly configService = inject(Config);
  private readonly diffService = inject(DiffService);
  private readonly fb = inject(FormBuilder);
  showResult = signal(false);

  versions$ = this.configService.getVersions();

  diffItems = signal<DiffItem[]>([]);
  form = this.fb.group({
    from: ['', Validators.required],
    to: ['', Validators.required]
  });

  compare(): void {
//    this.showResult = false;
    this.showResult.set(false);
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const from = Number(this.form.value.from);
    const to = Number(this.form.value.to);

    this.configService.getVersionById(from)
      .subscribe({
        next: (fromVersion: Version) => {
          this.configService.getVersionById(to)
            .subscribe({
              next: (toVersion: Version) => {

                const items = this.diffService.getDiffItems(
                  fromVersion.configurationJson,
                  toVersion.configurationJson
                );

                this.diffItems.set(items);
                this.showResult.set(true);
                console.log('Diff Items:', this.diffItems());
              },
              error: (error) => {
                console.error('To version error:', error);
              }
            });
        },
        error: (error) => {
          console.error('From version error:', error);
        }
      });
  }
}