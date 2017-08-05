import { NgModule }               from '@angular/core';
import { BrowserModule }          from '@angular/platform-browser';
import { FormsModule }            from '@angular/forms';
import { HttpModule }             from '@angular/http';

import { AppComponent }           from './app.component';
import { routing }                from './app.routing';
import { LinkShortenerComponent } from './link-shortener/link-shortener.component';

import { AuthGuard } from './_guards/index';
import { AuthenticationService, LinksService } from './_services/index';
import { LoginComponent } from './login/index';
import { CompactLoginComponent } from './compact-login/compact-login.component'
import { LinksComponent } from './links/links.component';

@NgModule({
  imports:      [ BrowserModule, FormsModule, HttpModule, routing ],
  declarations: [ AppComponent, LinkShortenerComponent, LoginComponent, LinksComponent, CompactLoginComponent ],
  providers:    [ AuthGuard, AuthenticationService, LinksService ],
  bootstrap:    [ AppComponent ]
})

export class AppModule { }