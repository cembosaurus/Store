import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { AuthService } from '../_services/auth.service';





@Injectable()
export class HttpHeaderInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) {}

  
  intercept(req: HttpRequest<any>, next: HttpHandler) {

    const authToken = this.authService.getJWTToken();

    if(this.authService.isLoggedIn())
    {
    // cloned headers, updated with the authorization.
    req = req.clone({
      setHeaders: {
        'Content-Type': 'application/json; charset=utf-8',
        Accept: 'application/json',
        Authorization: `Bearer ${this.authService.getJWTToken()}`
      }
    });
    }
    else{
    req = req.clone({
      setHeaders: {
        'Content-Type': 'application/json; charset=utf-8',
        Accept: 'application/json'
      }
    });
    }


    return next.handle(req);
  }
}