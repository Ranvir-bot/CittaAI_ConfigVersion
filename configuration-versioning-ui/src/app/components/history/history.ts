import { Component, inject } from '@angular/core';
import { DatePipe,AsyncPipe } from '@angular/common';

import { Config } from '../../services/config';
import { Version } from '../../models/version.model';

@Component({
  selector: 'app-history',
  imports: [DatePipe,AsyncPipe],
  templateUrl: './history.html',
  styleUrl: './history.css'
})
export class History {

  private readonly configService = inject(Config);

  versions: Version[] = [];
  selectedVersion?: Version;

  versions$ = this.configService.getVersions();

  view(id: number): void {
    this.configService.getVersionById(id)
      .subscribe({
        next: (response: Version) => {
          console.log('Selected version:', response);

          this.selectedVersion = response;
        },

        error: (error) => {
          console.error('Error:', error);
        }
      });
  }

 back(): void {
    this.selectedVersion = undefined;
  }

}