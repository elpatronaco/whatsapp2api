import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {IUser} from "../models/Dto/User/IUser";


@Injectable({providedIn: "root"})
export class UserService {
  constructor(private http: HttpClient) {
  }

  public getAllUsers(): Observable<IUser[]> {
    return this.http.get<IUser[]>("api/users/");
  }
}
