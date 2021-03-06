import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { TrainerMenuComponent } from './trainerMenu/trainer-menu.component';
import { LoginComponent } from './login/login.component';
import { ClientComponent } from 'app/components/client/client.component';
import { ClientDetailsComponent } from 'app/components/clientDetails/client-details.component';
import { FormsComponent } from './forms/forms.component';
import { ClientFormsComponent } from './clientForms/client-forms.component';

const routes: Routes = [
    // TODO: change this to redirect to the login page...
    {
        path: '',
        redirectTo: '/login',
        pathMatch: 'full'
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'trainer',
        component: TrainerMenuComponent,
        children: [
            {
                path: 'client',
                component: ClientComponent
            },
            {
                path: 'client/:clientId',
                component: ClientDetailsComponent
            },
            {
                path: 'clientForms/:clientId',
                component: ClientFormsComponent
            },
            {
                path: 'forms',
                component: FormsComponent
            }
        ]
    },
    // { path: 'detail/:name', component: CountryDetailComponent },
    // { path: 'all-countries', component: AllCountriesComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})

export class AppRoutingModule { }