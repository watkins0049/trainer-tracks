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

    public get(url: string, queryParameters?: object) {

        let headers = new Headers();
        this.createAuthorizationHeader(headers);

        // Straight lifted from https://stackoverflow.com/questions/41761523/how-to-convert-json-to-query-string-in-angular2
        let params = new URLSearchParams();
        for (let key in queryParameters) {
            params.set(key, queryParameters[key])
        }

        return this.http.get(`${this.generateUri(url)}?${params.toString()}`, {
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