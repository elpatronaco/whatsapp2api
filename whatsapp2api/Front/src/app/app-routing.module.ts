import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LandingComponent} from "./landing/landing.component";
import {ChatComponent} from "./chat/chat.component";

const routes: Routes = [
  {
    path: "",
    pathMatch: "full",
    component: LandingComponent
  },
  {
    path: "chat",
    pathMatch: "full",
    component: ChatComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
