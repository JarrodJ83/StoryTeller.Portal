import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RunSummary } from '../models/run-summary';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class RunsService {

    constructor(private http: HttpClient) { }

    public getRunsSummary(): Observable<RunSummary[]> {
        return this.http.get<RunSummary[]>('runs/summaries');
    }
}   