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
  _selectedItems: {itemId: number, amount: number}[] = new Array;

  constructor(private itemsService: ItemsService, authService: AuthService, private addItemPopUpDialog: MatDialog) { }


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



  openAddItemDialog(itemId: number) {

    var item = this.getItemFromList(itemId);

    this.addItemPopUpDialog
    .open(AddCatalogueItemPopUpComponent, { data: item ?? {itemId: itemId, amount: 0} })
    .afterClosed().subscribe(
      (amount: number)=>{

        this.addItemToList(itemId, amount);

      }
    );

  }



  addItemToList(itemId: number, amountToAdd: number)
  {
    var index = this._selectedItems.findIndex(i => i.itemId === itemId);

    if(index < 0)
    {
      this._selectedItems?.push({itemId: itemId, amount: amountToAdd});
    }
    else{
      this._selectedItems.forEach(item => {
        if(item.itemId === itemId)
        {
          this._selectedItems[index].amount = amountToAdd;
        }
      });
    }
  }


  getItemFromList(itemId: number): any
  {
    var result = undefined;

    var index = this._selectedItems.findIndex(i => i.itemId === itemId);

    if(index > -1)
    {
      this._selectedItems.forEach(item => {
        if(item.itemId === itemId)
        {
          result = item;
        }
      });
    }

    return result;
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
