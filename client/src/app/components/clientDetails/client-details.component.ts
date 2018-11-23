import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { HttpClient } from 'app/utilities/http/http.client';
import { Client } from 'app/model/client';

@Component({
    selector: 'client-details',
    templateUrl: './client-details.component.html'
})
export class ClientDetailsComponent implements OnInit {

    public client = new Client();
    public isSaving = false;
    public isLoading = false;

    private clientId: string;

    constructor(
        private httpClient: HttpClient,
        private activatedRoute: ActivatedRoute,
        private router: Router
    ) { }

    public ngOnInit(): void {
        const routeParams = this.activatedRoute.snapshot.params;
        this.clientId = routeParams.clientId;
        this.loadClientDetails();
    }

    private loadClientDetails(): void {
        this.isLoading = true;

        const params = {
            clientId: this.clientId
        };

        this.httpClient.get('client/clientDetails', params)
            .subscribe(
                res => {
                    this.client = res.json();
                },
                () => { },
                () => { this.isLoading = false; });
    }

    public navigateBack(): void {
        this.router.navigateByUrl('trainer/client');
    }

    public saveClient(): void {
        this.isSaving = true;
        this.httpClient.post('client/saveClient', this.client)
            .subscribe(
                () => { },
                () => { },
                () => { this.isSaving = false; });
    }

    public navigateToClientForms(): void {
        this.router.navigateByUrl(`trainer/clientForms/${this.clientId}`);
    }

}