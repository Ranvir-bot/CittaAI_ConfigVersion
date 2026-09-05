import { Component, inject ,signal } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { Config } from '../../services/config';
import { DiffService } from '../../services/diff.service';

import { Version } from '../../models/version.model';
import { DiffItem } from '../../models/diff-item.model';

@Component({
  selector: 'app-diff',
  imports: [ReactiveFormsModule, AsyncPipe],
  templateUrl: './diff.html',
  styleUrl: './diff.css'
})
export class Diff {

  private readonly configService = inject(Config);
  private readonly diffService = inject(DiffService);
  private readonly fb = inject(FormBuilder);

  versions$ = this.configService.getVersions();

  //diffItems: DiffItem[] = [];
diffItems = signal<DiffItem[]>([]);
  form = this.fb.group({
    from: ['', Validators.required],
    to: ['', Validators.required]
  });

  compare(): void {

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