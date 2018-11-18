import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

import { HttpClient } from 'app/utilities/http/http.client';
import { TrainerClient } from 'app/model/trainerClient';

@Component({
    selector: 'client',
    templateUrl: './client.component.html'
})
export class ClientComponent implements OnInit {

    public searchForm: FormGroup;
    public clients = new Array<TrainerClient>();
    public isLoading: boolean;

    constructor(
        private httpClient: HttpClient,
        private formBuilder: FormBuilder,
        private router: Router
    ) { }

    public ngOnInit(): void {
        this.searchForm = this.formBuilder.group({
            firstName: [''],
            lastName: ['']
        });
    }

    public searchClients(): void {
        this.isLoading = true;
        var parameters = {
            firstName: this.searchForm.controls['firstName'].value,
            lastName: this.searchForm.controls['lastName'].value
        };

        this.httpClient.get('client/searchClients', parameters)
            .subscribe(
                res => {
                    this.clients = res.json();
                },
                () => { },
                () => { this.isLoading = false; });
    }

    public navigateToClientDetails(clientId: number): void {
        this.router.navigateByUrl(`trainer/client/${clientId}`);
    }

}