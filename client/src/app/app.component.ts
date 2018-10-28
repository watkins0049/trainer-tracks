import { Component } from '@angular/core';
import { Http } from "@angular/http";
import { User } from '../model/user';
import { map } from 'rxjs/operators';

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
                console.log(res.json());
            });

    }

}
