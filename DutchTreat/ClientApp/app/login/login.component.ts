import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from '../shared/dataService';

@Component({
    selector: 'login',
    templateUrl: 'login.component.html'
})

export class Login{

    constructor(private readonly data: DataService, private readonly router: Router) { }

    errorMessage: string = '';

    credentials = {
        username: '',
        password: ''
    };

    onLogin() {
        this.data.login(this.credentials)
            .subscribe(success => {
                if (success) {
                    if (this.data.order.items.length === 0) {
                        this.router.navigate(['']);
                    } else {
                        this.router.navigate(['checkout']);

                    }
                }
            }, err => this.errorMessage = 'Failed to login');
    }
}