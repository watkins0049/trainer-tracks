import { Component, OnInit } from '@angular/core';
import { Response, ResponseContentType } from '@angular/http';

import { Forms } from 'app/model/forms';
import { HttpClient } from 'app/utilities/http/http.client';

@Component({
    selector: 'forms',
    templateUrl: './forms.component.html'
})
export class FormsComponent implements OnInit {

    public forms = new Array<Forms>();
    public isLoading = false;

    constructor(private httpClient: HttpClient) {

    }

    public ngOnInit(): void {
        this.httpClient.get('forms/trainerForms', {})
            .subscribe((res) => { this.forms = res.json(); });
    }

    public downloadTrainerForm(formName: string): void {
        const params = {
            formName: formName
        };

        this.httpClient.get('forms/downloadTrainerForm', params, { responseType: ResponseContentType.Blob })
            .subscribe((response: Response) => this.downloadFile(response));
    }

    private downloadFile(response: Response) {
        const blob = response.blob();
        const url = window.URL.createObjectURL(blob);
        window.open(url);
    }

}