import { Component } from '@angular/core';
import { Http, Headers } from "@angular/http";
import { User } from '../model/user';

// import { Observable } from 'rxjs/Observable';
// import 'rxjs/add/operator/map';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {

    public user = new User();

    constructor(private http: Http) {

    }

    public login(): void {

        this.http.post('/api/account/login', { emailAddress: this.user.email, password: this.user.password })
            .subscribe(res => {
                let json = res.json();
                localStorage.setItem('TrainerTracksCookie', json['token']);
            });

    }

    private test(): void {
        const token = localStorage.getItem('TrainerTracksCookie');

        let headers = new Headers();
        headers.append('Authorization', 'Bearer ' + token);

        this.http.get('/api/account/test', { headers: headers })
            .subscribe(res => {
                console.log(res);
            });
    }

}
