import { NgModule }               from '@angular/core';
import { BrowserModule }          from '@angular/platform-browser';
import { FormsModule }            from '@angular/forms';
import { HttpModule }             from '@angular/http';
import { AuthHttp, AuthConfig, provideAuth }   from 'angular2-jwt';
import { AppComponent }           from './app.component';
import { routing }                from './app.routing';
import { LinkShortenerComponent } from './link-shortener/link-shortener.component';

import { AuthGuard } from './_guards/index';
import { AuthenticationService, LinksService } from './_services/index';
import { LoginComponent, LogoutComponent } from './account/index';
import { LinksComponent } from './links/links.component';

@NgModule({
  imports:      [ BrowserModule, FormsModule, HttpModule, routing,  ],
  declarations: [ AppComponent, LinkShortenerComponent, LoginComponent, LogoutComponent, LinksComponent ],
  providers:    [ AuthGuard, AuthenticationService, LinksService, AuthHttp, 
                  provideAuth({
                      headerName: 'Authorization',
                      headerPrefix: 'bearer',
                      tokenName: 'token',
                      tokenGetter: (() => localStorage.getItem('token')),
                      globalHeaders: [{ 'Content-Type': 'application/json' }],
                      noJwtError: true
                  }) ],
  bootstrap:    [ AppComponent ]
})

export class AppModule { }