import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from '../environments/environment';
import { Observable, map } from 'rxjs';
import { APIServiceResult } from '../_models/APIServiceResult';
import { AuthService } from './auth.service';





@Injectable({
  providedIn: 'root'
})
export class ItemsService {

  _APIServiceResult: APIServiceResult | undefined;
  _itemsURL = environment.ApiGatewayUrl + 'CatalogueItem/all/';
  

  constructor(private http: HttpClient, private authService: AuthService) { }




  getItems(): Observable<APIServiceResult>
  {
    return this.http.get<APIServiceResult>(this._itemsURL);
  }


  getPhoto(url: string)
  {
    console.log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> IMAGE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>", url);

    return this.http.get(url, {
      responseType: 'blob'
    });
  }

}
