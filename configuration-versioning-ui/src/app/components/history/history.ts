import { Component, inject } from '@angular/core';
import { DatePipe,AsyncPipe } from '@angular/common';
import { Router } from '@angular/router';

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
  private readonly router = inject(Router);

  versions: Version[] = [];

  versions$ = this.configService.getVersions();

 edit(id: number): void {
    this.router.navigate(['/'], {
      queryParams: { versionId: id }
    });
  }



}