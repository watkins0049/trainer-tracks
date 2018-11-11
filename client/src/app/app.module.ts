import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { HttpClient } from './http/http.client';

import { AppComponent } from './app.component';

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule, FormsModule, HttpModule
    ],
    providers: [
        HttpClient
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
