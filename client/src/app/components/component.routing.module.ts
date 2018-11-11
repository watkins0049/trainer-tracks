import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { TrainerMenuComponent } from './trainerMenu/trainer-menu.component';
import { LoginComponent } from './login/login.component';
import { ClientComponent } from 'app/components/client/client.component';

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