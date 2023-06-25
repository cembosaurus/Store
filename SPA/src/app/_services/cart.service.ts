import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from '../environments/environment';
import { Observable, map } from 'rxjs';
import { APIServiceResult } from '../_models/APIServiceResult';




@Injectable({
  providedIn: 'root'
})
export class CartService {

  _APIServiceResult: APIServiceResult | undefined;
  _url = environment.apiUrl;

  constructor(private http: HttpClient) { }




  addItemToCart(): Observable<APIServiceResult>
  {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })

    return this.http.post<APIServiceResult>(this._url, { headers: headers });// To DO: add body //////!





}


// "{UserId}/items"