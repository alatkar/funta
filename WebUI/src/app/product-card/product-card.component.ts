import { Product } from './../models/product';
import { Component, OnInit, Input } from '@angular/core';
import { ShoppingCartService } from '../services/shopping-cart.service';
import { ShoppingCart } from '../models/shopping-cart';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.css']
})
export class ProductCardComponent implements OnInit {

  @Input('product') product: Product;
  // tslint:disable-next-line:no-input-rename
  @Input('show-actions') showActions = true;
  // tslint:disable-next-line:no-input-rename
  @Input('shopping-cart') shoppingCart: ShoppingCart;

  constructor(private shoppingCartService: ShoppingCartService) { }

  ngOnInit() {
  }

  addToCart() {
    this.shoppingCartService.addToCart(this.product);
  }
/*
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
  */
}
