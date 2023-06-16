import { Component, OnInit } from '@angular/core';
import { ItemsService } from '../_services/items.service';
import { Item } from '../_models/item';
import { map, take } from 'rxjs';
import { CatalogueItem } from '../_models/catalogueItem';
import { APIServiceResult } from '../_models/APIServiceResult';

@Component({
  selector: 'app-items-album',
  templateUrl: './items-album.component.html',
  styleUrls: ['./items-album.component.css']
})
export class ItemsAlbumComponent implements OnInit {

  _catalogueItems: CatalogueItem[] | undefined;

  constructor(private itemsService: ItemsService) { }


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


  getPhoto(photoUrl: string)
  {
    return this.itemsService.getPhoto(photoUrl);
  }


  addToCart(itemId: number)
  {
    console.log("----------------------> ITEM ID: ", itemId);
  }

}
