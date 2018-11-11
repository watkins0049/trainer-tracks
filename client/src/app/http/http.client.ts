import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';

@Injectable()
export class HttpClient {

    constructor(private http: Http) { }

    private createAuthorizationHeader(headers: Headers) {
        const token = localStorage.getItem('TrainerTracksCookie');
        headers.append('Authorization', 'Bearer ' + token);
    }

    private generateUri(apiUri: string): string {
        return `/api/${apiUri}`;
    }

    public get(url: string) {
        let headers = new Headers();
        this.createAuthorizationHeader(headers);
        return this.http.get(this.generateUri(url), {
            headers: headers
        });
    }

    public post(url: string, data: object) {
        let headers = new Headers();
        this.createAuthorizationHeader(headers);
        return this.http.post(this.generateUri(url), data, {
            headers: headers
        });
    }
}