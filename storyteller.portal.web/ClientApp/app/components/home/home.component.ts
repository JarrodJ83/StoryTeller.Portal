import { Component, OnInit } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { RunSummary } from '../../models/run-summary';
import { RunsService } from '../../services/runs';
import { Observable } from 'rxjs/Observable';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
    public runs: RunSummary[];

    constructor(private runsSvc: RunsService) {

    }

    ngOnInit(): void {
        this.initializeSignalR();   

        this.runsSvc.getRunsSummary().subscribe(data => this.runs = data);
    }

    initializeSignalR() {
        let connection = new signalR.HubConnection("/dashboard");

        connection.on('RunCreated', data => {
            
        });

        connection.on('RunSpecUpdated', data => {
            
        });

        connection.on('RunCompleted', data => {
            
        });

        connection.start();   
    }
}
