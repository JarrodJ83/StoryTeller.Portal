import { Component, OnInit } from '@angular/core';
import * as signalR from '@aspnet/signalr';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
    public msg: any;

    ngOnInit(): void {
        let connection = new signalR.HubConnection("/dashboard");

        connection.on('send', data => {
            this.msg = data;
        });

        connection.start().then(() => connection.invoke('send', 'Hello'));        
    }

}
