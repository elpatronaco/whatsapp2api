import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {LandingComponent} from "./landing/landing.component";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {HttpClientInterceptor} from "./services/httpInterceptor.service";
import {ChatModule} from "./chat/chat.module";
import {StateModule} from "./store/state.module";

@NgModule({
  declarations: [AppComponent, LandingComponent],
  imports: [
    BrowserModule, AppRoutingModule, FormsModule, HttpClientModule, ChatModule,
    StateModule
  ],
  providers: [{provide: HTTP_INTERCEPTORS, multi: true, useClass: HttpClientInterceptor}],
  bootstrap: [AppComponent],
})
export class AppModule {
}
