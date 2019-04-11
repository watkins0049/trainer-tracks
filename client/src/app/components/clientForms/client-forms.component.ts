import { Component, OnInit } from '@angular/core';
import { Response, ResponseContentType } from '@angular/http';
import { ActivatedRoute } from '@angular/router';

import { Forms } from 'app/model/forms';
import { HttpClient } from 'app/utilities/http/http.client';

@Component({
    selector: 'client-forms',
    templateUrl: './client-forms.component.html'
})
export class ClientFormsComponent implements OnInit {

    public forms = new Array<Forms>();
    private clientId: string;
    public isLoading = false;

    constructor(
        private httpClient: HttpClient,
        private activatedRoute: ActivatedRoute
    ) { }

    public ngOnInit(): void {
        const routeParams = this.activatedRoute.snapshot.params;
        this.clientId = routeParams.clientId;
        this.loadClientForms();
    }

    private loadClientForms(): void {
        this.httpClient.get('client/clientForms', { clientId: this.clientId })
            .subscribe((res) => { this.forms = res.json(); });
    }

    public downloadClientForm(formName: string): void {
        const params = {
            clientId: this.clientId,
            formName: formName
        };

        this.httpClient.get('client/downloadClientForm', params, { responseType: ResponseContentType.Blob })
            .subscribe((response: Response) => this.downloadFile(response));
    }

    private downloadFile(response: Response) {
        const blob = response.blob();
        const url = window.URL.createObjectURL(blob);
        window.open(url);
    }

}