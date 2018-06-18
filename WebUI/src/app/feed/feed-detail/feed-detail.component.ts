import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Feed } from '../../models/feed';

@Component({
  selector: 'app-feed-detail',
  templateUrl: './feed-detail.component.html',
  styleUrls: ['./feed-detail.component.css']
})
export class FeedDetailComponent implements OnInit {
  @Input('feed') feed: Feed;
  @Output('feedUpdated') feedUpdated = new EventEmitter<Feed>();

  constructor() { }

  onFeedUpdate() {
    this.feedUpdated.emit(this.feed);  // Tell parent that feed was updated
  }

  ngOnInit() {
  }

}
