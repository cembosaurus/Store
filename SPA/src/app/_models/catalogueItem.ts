import { Item } from "./item";
import { ItemPrice } from "./itemPrice";

export interface CatalogueItem {

    itemId: number;
    description: string;
    item: Item;
    itemPrice: ItemPrice;
    instock: number;
}