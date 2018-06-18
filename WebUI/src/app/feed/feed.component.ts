import { Component, OnInit } from '@angular/core';
import { Feed } from '../models/feed';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit {

  feeds: Feed[] = [
    // tslint:disable-next-line:max-line-length
    new Feed('1', 'admin', 'First Post', 'First post detail',
    'https://media.defense.gov/2014/Apr/09/2000860984/600/400/0/140409-F-IN811-001.JPG',
    'post', null, new Date(2018, 5, 11), new Date(2018, 6, 14) ),
    new Feed(
      '2',
      'admin',
      'Second Post',
      'Second post detail',
      'https://www.maxpixel.net/static/photo/1x/Sweet-Grass-Nice-Dog-Brown-Bitch-Dogs-Sun-2679538.jpg',
      'event', null, new Date(2018, 5, 12), new Date(2018, 6, 15) ),
    new Feed('3', 'admin', 'Third Post', 'Third post detail',
    'https://c.pxhere.com/photos/98/dd/dogs_iceland_dog_turbulent_play_pack_pet_rudel-1282435.jpg!d',
    'tip', null, new Date(2018, 5, 16), new Date(2018, 6, 17) ),
    new Feed('4', 'admin', 'Fourth Post', 'Fourth post detail',
    'https://www.maxpixel.net/static/photo/1x/Spitz-Puppies-Dogs-Puppies-2739234.jpg',
    'question', null, new Date(2018, 5, 11), new Date(2018, 6, 14) ),
    new Feed('5', 'admin', 'Fifth Post', 'Fifth post detail',
    'https://c.pxhere.com/photos/e4/0c/dogs_fur_animal_pet_portrait_cute_hybrid_dog_hybrid-535989.jpg!d',
    'answer', null, new Date(2018, 5, 12), new Date(2018, 6, 15) ),
    new Feed('6', 'admin', 'Sixth Post', 'Sixth post detail',
    null,
    'recommendation', null, new Date(2018, 5, 16), new Date(2018, 6, 17) ),
  ];

  constructor() { }

  ngOnInit() {
  }

  onFeedUpdated(event: Event) { // Just to show how events work
    console.log('FeedComponent received event for feed update ', event);
  }

}
