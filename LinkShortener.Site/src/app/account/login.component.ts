import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
 
import { AuthenticationService } from '../_services/index';
 
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
 
    login() {
        this.loading = true;
        this.authenticationService.login(this.model.username, this.model.password)
            .subscribe(() => this.router.navigate(['/']), error => this.error = error);
    }
}