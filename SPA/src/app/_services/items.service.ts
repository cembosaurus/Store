import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from '../environments/environment';
import { Observable, map } from 'rxjs';
import { APIServiceResult } from '../_models/APIServiceResult';





@Injectable({
  providedIn: 'root'
})
export class ItemsService {

  _APIServiceResult: APIServiceResult | undefined;
  _url = environment.apiUrl + 'CatalogueItem/all/';

  constructor(private http: HttpClient) { }




  getItems(): Observable<APIServiceResult>
  {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })

    return this.http.get<APIServiceResult>(this._url, { headers: headers });
  }


  

  getPhoto(photoUrl: string): Observable<any>
  {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    })

    console.log("---> PHOTO: ", photoUrl);

    return this.http.get(photoUrl, { headers: headers, responseType: 'Blob' as 'json' })
  }


}
