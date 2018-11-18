import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

import { HttpClient } from 'app/utilities/http/http.client';
// import { Client } from 'app/model/client';

@Component({
    selector: 'client-details',
    templateUrl: './client-details.component.html'
})
export class ClientDetailsComponent implements OnInit {

    constructor(private httpClient: HttpClient) {

    }

    public ngOnInit(): void {

    }

}