import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { HttpClient } from 'app/utilities/http/http.client';

import { AppComponent } from './app.component';
import { AppRoutingModule } from 'app/components/component.routing.module';
import { TrainerMenuComponent } from 'app/components/trainerMenu/trainer-menu.component';
import { LoginComponent } from 'app/components/login/login.component';
import { ClientComponent } from 'app/components/client/client.component';
import { ClientDetailsComponent } from 'app/components/clientDetails/client-details.component';

@NgModule({
    declarations: [
        AppComponent,
        TrainerMenuComponent,
        LoginComponent,
        ClientComponent,
        ClientDetailsComponent
    ],
    imports: [
        BrowserModule, FormsModule, HttpModule, ReactiveFormsModule,
        AppRoutingModule
    ],
    providers: [
        HttpClient
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
