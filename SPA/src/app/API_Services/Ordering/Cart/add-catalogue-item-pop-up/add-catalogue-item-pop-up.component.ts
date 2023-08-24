import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


@Component({
  selector: 'app-add-catalogue-item-pop-up',
  templateUrl: './add-catalogue-item-pop-up.component.html',
  styleUrls: ['./add-catalogue-item-pop-up.component.css']
})
export class AddCatalogueItemPopUpComponent implements OnInit {

  @Output() _itemAmountEmiter = new EventEmitter<any>();
  _amountOnCart: number;
  _itemId: number;






//--------------------------------------- To Do: input amount on cart to work with






  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private dialogRef: MatDialogRef<AddCatalogueItemPopUpComponent>) {
    this._itemId = data.itemId;
    this._amountOnCart = data.amount;
  }

  ngOnInit(): void { }

  ngOnDestroy(){

    console.log("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx ", this._amountOnCart);

    this.dialogRef.close(this._amountOnCart); 
  }




  increaseOnCart()
  {
    this._amountOnCart++;
  }


  decreaseOnCart()
  {
    if(this._amountOnCart > 0)
    this._amountOnCart--;
  }

}
