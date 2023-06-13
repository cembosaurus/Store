import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import {map} from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User | undefined;

  constructor(private http: HttpClient) { }

  login(userDTO: any) {
    return this.http.post(this.baseUrl + 'login', userDTO).pipe(
      map((response: any) => {
          const response_from_API = response;
          if (response_from_API) {
            console.log(response_from_API);
            localStorage.setItem('token', response_from_API.token);
            localStorage.setItem('user', JSON.stringify(response_from_API.userFromApi));
            this.decodedToken = this.jwtHelper.decodeToken(response_from_API.token);
            this.currentUser = response_from_API.userFromApi;
            console.log(this.decodedToken);
          }
        })
    );
  }

  register(userDTO: User) {
    return this.http.post(this.baseUrl + 'register', userDTO);
  }

  isLoggedIn() {
    const jwt = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(jwt);
  }
}