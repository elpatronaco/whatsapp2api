import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ITokens} from "../models/Dto/Auth/ITokens";
import {IUserAuthenticate} from "../models/Dto/User/IUserAuthenticate";


@Injectable({providedIn: "root"})
export class AuthService {
  constructor(private http: HttpClient) {
  }

  saveTokens({accessToken, idToken}: ITokens) {
    sessionStorage.setItem("idToken", idToken);
    sessionStorage.setItem("accessToken", accessToken);
  }

  getTokens(): ITokens | null {
    const idToken = sessionStorage.getItem("idToken");
    const accessToken = sessionStorage.getItem("accessToken");

    return idToken && accessToken ? {accessToken, idToken} : null;
  }

  login(payload: IUserAuthenticate): Observable<ITokens> {
    return this.http.post<ITokens>("api/auth/login", payload);
  }
}
