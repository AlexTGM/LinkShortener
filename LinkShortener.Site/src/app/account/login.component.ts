import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
 
import { AuthenticationService } from '../_services/index';
import 'rxjs/add/operator/map'

@Component({
    moduleId: module.id,
    templateUrl: 'login.component.html'
})
 
export class LoginComponent {
    model: any = {};
    loading = false;
    error = '';
 
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService) { }
 
    public onSubmit(empForm: any, event: Event) {
        event.preventDefault();

        this.loading = true;
        this.authenticationService.login(this.model.username, this.model.password)
            .subscribe(() => this.router.navigate(['/']), error => {
                this.error = error._body;
                this.loading = false;
            });
    }
}