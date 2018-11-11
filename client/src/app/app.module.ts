import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { HttpClient } from 'app/utilities/http/http.client';

import { AppComponent } from './app.component';
import { AppRoutingModule } from 'app/components/component.routing.module';
import { HomeComponent } from 'app/components/home/home.component';
import { LoginComponent } from 'app/components/login/login.component';

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        LoginComponent
    ],
    imports: [
        BrowserModule, FormsModule, HttpModule,
        AppRoutingModule
    ],
    providers: [
        HttpClient
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
