import { Feed } from 'src/app/models/feed';
import { FeedService } from './../services/feed.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit {

  feeds: Feed[] = [];

  constructor(private feedService: FeedService) {
   }

  ngOnInit() {
    this.feeds = this.feedService.getFeed();
    this.feedService.newFeed.subscribe(
      (feed: Feed)  => {
        console.log('New Feed Created ' + feed);
        this.feeds = this.feedService.getFeed();
      }
    );
  }

  onFeedUpdated(event: Event) { // Just to show how events work
    console.log('FeedComponent received event for feed update ', event);
  }

  onNewFeed(event: Feed) {
    console.log('FeedComponent received new feed', event);
    this.feedService.addNewFeed(event);
  }
}
