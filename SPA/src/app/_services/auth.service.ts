import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../_models/user';
import { environment } from '../environments/environment';
import { APIServiceResult } from '../_models/APIServiceResult';
import { Observable } from 'rxjs';



@Injectable({
  providedIn: 'root'
})
export class AuthService {

  url = environment.ApiGatewayUrl + 'identity/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  user: User = { id: 0, name: "", password: ""};



  constructor(private http: HttpClient) {}


  getJWTToken()
  {
    return localStorage.getItem('token') || "";
  }


  initialize()
  {
    this.decodedToken = this.decodedToken = this.jwtHelper.decodeToken(localStorage.getItem('token') || "");
    this.user.name = this.decodedToken.name;
  }


  login(userToLogin: any): Observable<APIServiceResult> {

    return this.http.post<APIServiceResult>(this.url + 'login', userToLogin)
    .pipe(
      map((response: APIServiceResult) => {
          if (response.status) {

            localStorage.setItem('token', response.data);   
            this.initialize();
           
          }

          return response;
        })
    );
  }


  register(user: User): Observable<APIServiceResult>  {
    return this.http.post(this.url + 'register', user)
    .pipe(
      map((response: any) => {
          return response;
        })
    );
  }

  isLoggedIn() {
    return !this.jwtHelper.isTokenExpired(localStorage.getItem('token') || "");
  }


  getCurentUser()
  {
    this.initialize();
    return this.user;
  }
}
