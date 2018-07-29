import { Component, OnInit, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';
import { Feed } from 'src/app/models/feed';
import { NgForm } from '@angular/forms';
import { FeedService } from 'src/app/services/feed.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-feed-create',
  templateUrl: './feed-create.component.html',
  styleUrls: ['./feed-create.component.css']
})
export class FeedCreateComponent implements OnInit {
  @Output('newFeed') newFeed = new EventEmitter<Feed>();

  constructor(private feedService: FeedService, private authService: AuthService) { }

  ngOnInit() {
  }

  onSubmit(form: NgForm) {
    console.log(form.value.detail);
    console.log(form.value);
    const newFeed =  new Feed(null, this.authService.getUserId(), form.value.heading, form.value.detail, form.value.imageUrl,
    form.value.type, null, new Date(), new Date());
    this.feedService.addNewFeed(newFeed)
      .subscribe((resp: Feed ) =>  {console.log('received post respone ', resp); }
  );
    form.reset();
    // Hack to validate form. This should happne in Template
    /*
    if (this.heading.nativeElement.value !== '' &&
        this.detail.nativeElement.value !== '' &&
        this.type.nativeElement.value !== '') {
          this.newFeed.emit(new Feed(null, 'Amin', this.heading.nativeElement.value,
              this.detail.nativeElement.value, this.imageUrl.nativeElement.value, 
              this.type.nativeElement.value, null, new Date(), new Date()));
    }
    */
  }
}
