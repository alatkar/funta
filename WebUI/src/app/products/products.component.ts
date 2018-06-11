import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { ShoppingCartService } from './../services/shopping-cart.service';
import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../models/product';
import 'rxjs/add/operator/switchMap';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit, OnDestroy {
  products: Product[] = [];
  filteredProducts: Product[];
  category: string;
  cart: any;
  subscription: Subscription;

  constructor(route: ActivatedRoute, productService: ProductService, private shoppingCartService: ShoppingCartService) {
    productService
    .getAll()
    .switchMap(products => {
      this.products = products;
      return route.queryParamMap;
    })
    .subscribe(params => {
      this.category = params.get('category');
      this.filteredProducts = (this.category) ? this.products.filter(p => p.category === this.category) :  this.products;
    });
  }

  async ngOnInit() {  // Async can not be in ctor so it is implemented
    this. subscription = (await this.shoppingCartService.getCart()).subscribe(cart => this.cart = cart);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
