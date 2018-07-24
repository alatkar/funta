import { Feed } from 'src/app/models/feed';
import { FeedService } from './../services/feed.service';
import { Component, OnInit } from '@angular/core';
import { HttpResponse } from '@angular/common/http';

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
    this.feedService.getFeed().subscribe((data: Feed[]) => {
      console.log(data);
      const temp = data;
      this.feeds = data; // {...data.body};
    });
    this.feedService.newFeed.subscribe(
      (feed: Feed)  => {
        console.log('New Feed Created ' + feed);
        this.feedService.getFeed().subscribe((data: Feed[]) => this.feeds = data);
      }
    );
  }

  onFeedUpdated(event: Event) { // Just to show how events work
    console.log('FeedComponent received event for feed update ', event);
  }

  onNewFeed(event: Feed) {
    console.log('FeedComponent received new feed', event);
    this.feedService.addNewFeed(event).subscribe(
      (response: any) => {
        console.log(response);
      }
    );
  }
}
