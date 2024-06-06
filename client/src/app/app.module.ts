import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserService } from './user.service';
import { CommonModule } from '@angular/common';
import { NgbModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { ErrorService } from './error.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    CommonModule,
    NgbModule,
    NgbNavModule
  ],
  providers: [
    UserService,
    ErrorService,
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
