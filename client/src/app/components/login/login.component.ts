import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { User } from 'app/model/user';
import { HttpClient } from 'app/utilities/http/http.client';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    // styleUrls: ['./app.component.css']
})
export class LoginComponent {

    public user = new User();

    constructor(private httpClient: HttpClient,
        private router: Router) {

    }

    public login(): void {

        this.httpClient.post('account/login', { emailAddress: this.user.email, password: this.user.password })
            .subscribe(res => {
                let json = res.json();
                localStorage.setItem('TrainerTracksCookie', json['token']);
                this.router.navigateByUrl('/home');
            });
    }

}
