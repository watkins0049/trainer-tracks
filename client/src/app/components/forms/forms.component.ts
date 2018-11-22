import { Component, OnInit } from '@angular/core';

import { Forms } from 'app/model/forms';
import { HttpClient } from 'app/utilities/http/http.client';

@Component({
    selector: 'forms',
    templateUrl: './forms.component.html'
})
export class FormsComponent implements OnInit {

    public forms = new Array<Forms>();

    constructor(private httpClient: HttpClient) {

    }

    public ngOnInit(): void {
        this.httpClient.get('forms/trainerForms', {})
            .subscribe((res) => { this.forms = res.json(); });
    }

}