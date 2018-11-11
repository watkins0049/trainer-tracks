import { Component } from '@angular/core';
import { Http, Headers } from "@angular/http";
import { User } from '../model/user';
import { HttpClient } from './http/http.client';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {

    public user = new User();

    constructor(private httpClient: HttpClient) {

    }

    public login(): void {

        this.httpClient.post('account/login', { emailAddress: this.user.email, password: this.user.password })
            .subscribe(res => {
                let json = res.json();
                localStorage.setItem('TrainerTracksCookie', json['token']);
            });
    }

}
