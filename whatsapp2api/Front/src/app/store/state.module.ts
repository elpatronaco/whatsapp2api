import {NgModule} from "@angular/core";
import {StoreModule} from "@ngrx/store";
import * as fromGlobal from './global/global.reducer';
import {EffectsModule} from "@ngrx/effects";
import {StoreDevtoolsModule} from '@ngrx/store-devtools';
import {GlobalEffects} from "./global/global.effects";
import {environment} from "../../environments/environment";

export interface IState {
  global: fromGlobal.IGlobalState
}

@NgModule({
  imports: [
    StoreModule.forRoot<IState>({global: fromGlobal.reducer}),
    EffectsModule.forRoot([GlobalEffects]),
    StoreDevtoolsModule.instrument({name: "Whatsapp 2 App", logOnly: !environment.production})
  ]
})
export class StateModule {
}
