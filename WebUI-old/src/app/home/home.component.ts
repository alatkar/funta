import { Component, OnInit } from '@angular/core';
import {FeedService } from '../services/feed.service'
import { Feed } from '../models/feed';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  feed = [
    {type: 'post', message: 'Dog sleeping'},
    {type: 'product', message: 'Dog uses food'},
    {type: 'service', message: 'Dog uses grooming'},
    {type: 'service', message: 'Dog walker'}
  ];
  
  feeds : Feed[];

  constructor(private feedService: FeedService) { }

  ngOnInit() {
    this.feedService.getFeed()
    .subscribe ( fd => {this.feeds = fd; } );
  }

  getFeed() {
    
  }

}
