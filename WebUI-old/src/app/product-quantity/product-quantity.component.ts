import { Component, OnInit, Input } from '@angular/core';
import { Product } from '../models/product';
import { ShoppingCart } from '../models/shopping-cart';
import { ShoppingCartService } from '../services/shopping-cart.service';

@Component({
  selector: 'product-quantity',
  templateUrl: './product-quantity.component.html',
  styleUrls: ['./product-quantity.component.css']
})
export class ProductQuantityComponent implements OnInit {
  @Input('product') product: Product;
  @Input('shopping-cart') shoppingCart: ShoppingCart;

  constructor(private shoppingCartService: ShoppingCartService) { }

  ngOnInit() {
  }

  addToCart() {
    this.shoppingCartService.addToCart(this.product);
  }

  removeFromCart() {
    this.shoppingCartService.removeFromCart(this.product);
  }

  getQuantity() {
    // tslint:disable-next-line:curly
    if (!this.shoppingCart) {
      console.log('ProductCardComponent:getQuantity=> ', 'getQuantity is zero');
      return 0;
    }

    const item = this.shoppingCart.items[this.product.$key];
    if (item) {
      console.log('ProductCardComponent:getQuantity=> ', item.quantity, item, this.shoppingCart, this.product.$key);
    }
    return item ? item.quantity : 0;
  }

}
