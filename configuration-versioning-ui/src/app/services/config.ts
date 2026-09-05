import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Save } from '../models/save.model';
import { Version } from '../models/version.model';
import { API_CONFIG } from '../config/api.config';
import { SaveResponse } from '../models/save-response.model';



@Injectable({
    providedIn: 'root'
})
export class Config {

    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${API_CONFIG.baseUrl}/config`;

    saveConfiguration(request: Save): Observable<SaveResponse> {
        debugger;
        return this.http.post<SaveResponse>(`${this.apiUrl}/save`, request);
    }

    getVersions(): Observable<Version[]> {
        return this.http.get<Version[]>(`${this.apiUrl}/versions`);
    }

    getVersionById(id: number): Observable<Version> {
        return this.http.get<Version>(`${this.apiUrl}/versions/${id}`
        );
    }

    getDiff(from: number, to: number): Observable<any> {
        return this.http.get<any>(
            `${this.apiUrl}/diff?from=${from}&to=${to}`
        );
    }

}
