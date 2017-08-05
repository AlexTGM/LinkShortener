import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
 
import { AuthenticationService } from '../_services/index';
 
@Component({
    moduleId: module.id,
    selector: 'login',
    templateUrl: 'compact-login.component.html'
})
 
export class CompactLoginComponent implements OnInit {
    username: string;
    logged: boolean;

    constructor(
        private router: Router,
        private authenticationService: AuthenticationService) { }

    ngOnInit() {
        var currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.logged = currentUser;
        this.username = currentUser && currentUser.username;
    }

    login() {
        this.router.navigate(['login']);
    }

    logout() {
        this.authenticationService.logout();
        
        this.logged = false;
    }
}