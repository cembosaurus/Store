import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../_models/user';
import { environment } from '../environments/environment';



@Injectable({
  providedIn: 'root'
})
export class AuthService {

  url = environment.apiUrl + 'identity/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User | undefined;



  constructor(private http: HttpClient) { }


  login(userDTO: any) {

    return this.http.post(this.url + 'login', userDTO).pipe(
      map((response: any) => {

          const response_from_API = response;

          if (response_from_API) {

            console.log("---> Response from Auth API: ", response_from_API);
            localStorage.setItem('token', response_from_API.data.token);
            localStorage.setItem('user', JSON.stringify(response_from_API.data.Name));
            this.decodedToken = this.jwtHelper.decodeToken(response_from_API.data.token);
            this.currentUser = response_from_API.data.Name;

            console.log("---> Decdoded token from Auth API: ", this.decodedToken);

          }
        })
    );
  }


  register(userDTO: User) {
    return this.http.post(this.url + 'register', userDTO);
  }

  isLoggedIn() {
    const jwt = localStorage.getItem('token');

    console.log("---> Decoded TOKEN from Auth API: ", jwt);

    return !this.jwtHelper.isTokenExpired(jwt);
  }

}
