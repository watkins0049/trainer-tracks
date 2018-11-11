import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'trainer-menu',
    templateUrl: './trainer-menu.component.html',
})
export class TrainerMenuComponent {

    constructor(private router: Router) { }

    public navigateTo(component: string): void {
        this.router.navigateByUrl(`trainer/${component}`);
    }

}