import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = 'http://localhost:5000/api/identity/';

  constructor(private http: HttpClient) { }

  login(userDTO: any) {
    return this.http.post(this.baseUrl + 'login', userDTO)
      .pipe(
        map((response: any) => {
          const userResponse = response;
          if (userResponse) {
            localStorage.setItem('token', userResponse.data.token);
          }
        })
      );
  }

  register(userDTO: any) {
    return this.http.post(this.baseUrl + 'register', userDTO);
  }

}
