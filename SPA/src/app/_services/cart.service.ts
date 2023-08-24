import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from '../environments/environment';
import { Observable, map } from 'rxjs';
import { APIServiceResult } from '../_models/APIServiceResult';
import { CartItemUpdateDTO } from '../_models/CartItemUpdateDTO';




@Injectable({
  providedIn: 'root'
})
export class CartService {

  _APIServiceResult: APIServiceResult | undefined;
  _orderingUrl = environment.OrderingServiceUrl;


  constructor(private http: HttpClient) { }




  addItemsToCart(items: CartItemUpdateDTO[]): Observable<APIServiceResult>
  {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })

    return this.http.post<APIServiceResult>(this._orderingUrl + "cart/items", {Items: items},{ headers: headers });

  }
}

