import { Component, OnInit } from '@angular/core';
import { ItemsService } from '../_services/items.service';
import { Item } from '../_models/item';
import { map, take } from 'rxjs';
import { CatalogueItem } from '../_models/catalogueItem';
import { APIServiceResult } from '../_models/APIServiceResult';
import { environment } from '../environments/environment';
import { AuthService } from '../_services/auth.service';



@Component({
  selector: 'app-items-album',
  templateUrl: './items-album.component.html',
  styleUrls: ['./items-album.component.css']
})
export class ItemsAlbumComponent implements OnInit {

  _photosURL = environment.gatewayUrl + 'photos/';
  _catalogueItems: CatalogueItem[] | undefined;

  constructor(private itemsService: ItemsService, authService: AuthService) { }


  ngOnInit(): void {
    this.getItems();
  }


  getItems()
  {
    this.itemsService.getItems()
      .subscribe((result: APIServiceResult) => {
        this._catalogueItems = result.data;
      });
  }


  addToCart(itemId: number)
  {
    console.log("----------------------> ITEM ID: ", itemId);
  }



  getPhoto(id: string)
  {
    this.itemsService.getPhoto(this._photosURL + id);
  }

}
