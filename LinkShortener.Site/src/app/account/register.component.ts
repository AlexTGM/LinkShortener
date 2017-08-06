import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
 
import { AuthenticationService } from '../_services/index';
 
@Component({
    moduleId: module.id,
    templateUrl: 'register.component.html'
})
 
export class RegisterComponent {
    model: any = {};
    loading = false;
    error = '';
 
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService) { }
  
    public onSubmit(empForm: any, event: Event) {
        event.preventDefault();

        this.loading = true;
        this.authenticationService.register(this.model.username, this.model.password)
            .subscribe(() => this.router.navigate(['login']), error => {
                this.error = error._body;
                this.loading = false;
            });
    }
}