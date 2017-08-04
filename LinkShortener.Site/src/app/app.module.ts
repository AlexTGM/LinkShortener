import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule }   from '@angular/forms';

import { AppComponent }  from './app.component';
import { LinkShortenerComponent } from './link-shortener.component'

@NgModule({
  imports:      [ BrowserModule, FormsModule ],
  declarations: [ AppComponent, LinkShortenerComponent ],
  bootstrap:    [ AppComponent ]
})
export class AppModule { }
