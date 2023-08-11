import { Component, OnInit } from '@angular/core';
import { ItemsService } from '../../../../_services/items.service';
import { Item } from '../../../../_models/item';
import { map, take } from 'rxjs';
import { CatalogueItem } from '../../../../_models/catalogueItem';
import { APIServiceResult } from '../../../../_models/APIServiceResult';
import { environment } from '../../../../environments/environment';
import { AuthService } from '../../../../_services/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { AddCatalogueItemPopUpComponent } from 'src/app/API_Services/Ordering/Cart/add-catalogue-item-pop-up/add-catalogue-item-pop-up.component';
import { Observable } from 'rxjs';



@Component({
  selector: 'app-items-album',
  templateUrl: './catalogue-items-album.component.html',
  styleUrls: ['./catalogue-items-album.component.css']
})
export class ItemsAlbumComponent implements OnInit {

  _photosURL = environment.gatewayUrl + 'photos/';
  _catalogueItems: CatalogueItem[] | undefined;
  _inCart: [itemId: number, amount: number] = [0, 0];

  constructor(private itemsService: ItemsService, authService: AuthService, private addItemPopUpDialog: MatDialog) { }


  ngOnInit(): void {
    this.getItems();
  }



  // ............................................................. To Do: _inCart should be populated by response from Dialog pop up ..........................




  getItems()
  {
    this.itemsService.getItems()
      .subscribe((result: APIServiceResult) => {
        this._catalogueItems = result.data;
      });
  }



  openAddItemDialog(itemId: number) {
    this.addItemPopUpDialog
    .open(AddCatalogueItemPopUpComponent, { data: itemId })
    .afterClosed().subscribe(
      (res)=>{
        console.log("--------------------------->" + res);
      }
    );
  }



  addToCart(itemId: number)
  {
    console.log("----------------------> ITEM ID: ", itemId);
  }


  removeFromCart(itemId: number)
  {
    console.log("----------------------> ITEM ID: ", itemId);
  }


  getPhoto(id: string)
  {
    this.itemsService.getPhoto(this._photosURL + id);
  }

}
