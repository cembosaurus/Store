import { Component, OnInit } from '@angular/core';
import { ItemsService } from '../../../../_services/items.service';
import { CartService } from '../../../../_services/cart.service';
import { CatalogueItem } from '../../../../_models/catalogueItem';
import { APIServiceResult } from '../../../../_models/APIServiceResult';
import { environment } from '../../../../environments/environment';
import { AuthService } from '../../../../_services/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { AddCatalogueItemPopUpComponent } from 'src/app/API_Services/Ordering/Cart/add-catalogue-item-pop-up/add-catalogue-item-pop-up.component';
import { CartItemUpdateDTO } from 'src/app/_models/CartItemUpdateDTO';



@Component({
  selector: 'app-items-album',
  templateUrl: './catalogue-items-album.component.html',
  styleUrls: ['./catalogue-items-album.component.css']
})
export class ItemsAlbumComponent implements OnInit {

  _photosURL = environment.ApiGatewayUrl + 'photos/';
  _catalogueItems: CatalogueItem[] | undefined;
  _selectedItems: CartItemUpdateDTO[] = new Array<CartItemUpdateDTO>;

  constructor(private itemsService: ItemsService, private cartService: CartService, private authService: AuthService, private addItemPopUpDialog: MatDialog) { }


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

        if(amount > 0)
        {
         this.addItemToList(itemId, amount);
        }
        else{
          this.removeItemFromList(itemId);
        }
      }
    );
    
  }



  removeItemFromList(itemId: number)
  {
    var index = this._selectedItems.findIndex(i => i.itemId === itemId)

    if (index > -1) {
      this._selectedItems.splice(index, 1);
    }

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







// ------------------- To Do: load items on cart from API to add or delete --------------------------





  addToCart()
  {
    console.log("--------- ADD to cart -------------> ITEM ID: ", this._selectedItems);

    this.cartService.addItemsToCart(this._selectedItems)
    .subscribe(data => {
      console.log("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX API RESPONSE XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", data);
    });    
  }


  removeFromCart(itemId: number)
  {
    console.log("---------- REMOVE from cart ------------> ITEM ID: ", itemId);
  }


  getPhoto(id: string)
  {
    this.itemsService.getPhoto(this._photosURL + id);
  }

}
