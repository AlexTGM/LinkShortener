import { Router } from "@angular/router";
import { Component, OnInit } from "@angular/core";

import { AuthenticationService } from "../_services/index"

@Component({
    template: ''
})

export class LogoutComponent implements OnInit {

    constructor(private _authService: AuthenticationService, 
                private router: Router) { }

    ngOnInit() {
        this._authService.logout();
        this.router.navigate(['']);
    }
}