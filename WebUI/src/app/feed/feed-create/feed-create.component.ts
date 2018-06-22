import { Component, OnInit, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';
import { Feed } from 'src/app/models/feed';

@Component({
  selector: 'app-feed-create',
  templateUrl: './feed-create.component.html',
  styleUrls: ['./feed-create.component.css']
})
export class FeedCreateComponent implements OnInit {
  @Output('newFeed') newFeed = new EventEmitter<Feed>();
  @ViewChild('heading') heading: ElementRef;
  @ViewChild('detail') detail: ElementRef;
  @ViewChild('imageUrl') imageUrl: ElementRef;
  @ViewChild('type') type: ElementRef;

  constructor() { }

  ngOnInit() {
  }

  onNewFeed() {
    // Hack to validate form. This should happne in Template
    if (this.heading.nativeElement.value !== '' &&
        this.detail.nativeElement.value !== '' &&
        this.type.nativeElement.value !== '') {
          this.newFeed.emit(new Feed(null, 'Amin', this.heading.nativeElement.value,
              this.detail.nativeElement.value, this.imageUrl.nativeElement.value, 
              this.type.nativeElement.value, null, new Date(), new Date()));
    }
  }
}
