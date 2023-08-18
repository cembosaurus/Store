import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


@Component({
  selector: 'app-add-catalogue-item-pop-up',
  templateUrl: './add-catalogue-item-pop-up.component.html',
  styleUrls: ['./add-catalogue-item-pop-up.component.css']
})
export class AddCatalogueItemPopUpComponent implements OnInit {

  @Output() _itemAmountEmiter = new EventEmitter<any>();
  _amountOnCart: number = 0;
  _itemId: number = 0;






//--------------------------------------- To Do: input amount on cart to work with






  constructor(@Inject(MAT_DIALOG_DATA) public data: number, private dialogRef: MatDialogRef<AddCatalogueItemPopUpComponent>) {
    this._itemId = data;
  }

  ngOnInit(): void { }

  ngOnDestroy(){
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
