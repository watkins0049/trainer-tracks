import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { HttpClient } from 'app/utilities/http/http.client';
import { Client } from 'app/model/client';

@Component({
    selector: 'client-details',
    templateUrl: './client-details.component.html'
})
export class ClientDetailsComponent implements OnInit {

    public client = new Client();

    constructor(
        private httpClient: HttpClient,
        private activatedRoute: ActivatedRoute
    ) { }

    public ngOnInit(): void {
        const routeParams = this.activatedRoute.snapshot.params;
        this.loadClientDetails(routeParams.clientId);
    }

    private loadClientDetails(clientId: number): void {
        const params = {
            clientId: clientId
        }

        this.httpClient.get('client/clientDetails', params)
            .subscribe(res => {
                this.client = res.json();
            });
    }

    public saveClient(): void {
        this.httpClient.post('client/saveClient', this.client)
            .subscribe(res => {
                const test = "this";
            });
    }

}