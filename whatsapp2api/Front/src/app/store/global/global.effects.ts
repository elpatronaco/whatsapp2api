import {Injectable} from "@angular/core";
import {Actions, createEffect, ofType} from "@ngrx/effects";
import {mergeMap, map, tap, catchError, concatMap} from "rxjs/operators";
import {AuthService} from "../../services/auth.service";
import {
  LoginFailure,
  LoginRequest,
  LoginSuccess,
  SetChatRecipient, ValidateUserFailure,
  ValidateUserRequest,
  ValidateUserSuccess
} from "./global.actions";
import {EMPTY, of} from "rxjs";
import {MessageService} from "src/app/services/message.service";
import decodeJwt from 'jwt-decode';
import {IAccessTokenPayload} from "../../models/Dto/Auth/IAccessTokenPayload";
import {Router} from "@angular/router";


@Injectable()
export class GlobalEffects {
  login$ = createEffect(() => this.actions$.pipe(
      ofType(LoginRequest),
      mergeMap(action => this.authService.login(action)
        .pipe(
          tap(async (tokens) => {
            this.authService.saveTokens(tokens);

            await this.messageService.init(tokens);

            await this.router.navigate(["chat"]);
          }),
          concatMap(async (tokens) => {
            const accessTokenPayload = decodeJwt<IAccessTokenPayload>(tokens.accessToken);

            return LoginSuccess(accessTokenPayload);
          }),
          catchError(() => of(LoginFailure()))
        )
      )
    )
  );

  validate$ = createEffect(() => this.actions$.pipe(
      ofType(ValidateUserRequest),
      concatMap(async () => {
        const tokens = this.authService.getTokens();

        if (!tokens) {
          await this.router.navigate([""]);

          return ValidateUserFailure();
        }

        await this.messageService.init(tokens);

        const accessTokenPayload = decodeJwt<IAccessTokenPayload>(tokens.accessToken);

        return ValidateUserSuccess(accessTokenPayload);
      })
    )
  );

  constructor(private actions$: Actions, private authService: AuthService,
              private messageService: MessageService, private router: Router) {
  }
}
