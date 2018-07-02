import { Injectable, EventEmitter } from '@angular/core';
import { Feed } from 'src/app/models/feed';

@Injectable({
  providedIn: 'root'
})
export class FeedService {
  feeds: Feed[] = [
    // tslint:disable-next-line:max-line-length
    new Feed('1', 'admin', 'Look at my puppy', 'She is giving perfect pose!',
    'https://media.defense.gov/2014/Apr/09/2000860984/600/400/0/140409-F-IN811-001.JPG',
    'post', null, new Date(2018, 5, 11), new Date(2018, 6, 14) ),
    new Feed(
      '2',
      'admin',
      'Training camp on 05/15/2018',
      `We will provide free orientation abut how to train your dog. This will be especially useful for dogs having temperamental issues. 
       It can also help dogs which are not abedient`,
      'https://www.maxpixel.net/static/photo/1x/Sweet-Grass-Nice-Dog-Brown-Bitch-Dogs-Sun-2679538.jpg',
      'event', null, new Date(2018, 5, 12), new Date(2018, 6, 15) ),
    new Feed('3', 'admin', 'Summer is here German Shepherd owners', 'Please provide adequate water and apply cream to weed off fleece',
    'https://c.pxhere.com/photos/98/dd/dogs_iceland_dog_turbulent_play_pack_pet_rudel-1282435.jpg!d',
    'tip', null, new Date(2018, 5, 16), new Date(2018, 6, 17) ),
    new Feed('4', 'admin', 'Does anybody know what happens if dog eats blackberry plant?', 'My dog just ate tone of blackberry bush...',
    'https://www.maxpixel.net/static/photo/1x/Spitz-Puppies-Dogs-Puppies-2739234.jpg',
    'question', null, new Date(2018, 5, 11), new Date(2018, 6, 14) ),
    new Feed('5', 'admin', 'Dogs having tummy upset?', 'OTC medicine, venta works well!',
    'https://c.pxhere.com/photos/e4/0c/dogs_fur_animal_pet_portrait_cute_hybrid_dog_hybrid-535989.jpg!d',
    'answer', null, new Date(2018, 5, 12), new Date(2018, 6, 15) ),
    new Feed('6', 'admin', 'Dog walker recommendation..', 'David from Rose Hill district is perfect guy',
    'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScnGNGBjiVgI90-oGM1TbEFb10GTM7jGsr9TlMW5TVIRdZxAHQpA',
    'recommendation', null, new Date(2018, 5, 16), new Date(2018, 6, 17) ),
  ];

  newFeed = new EventEmitter<Feed>();
  constructor() { }

  getFeed() {
    return this.feeds.slice();
  }

  addNewFeed(feed: Feed) {
    this.feeds.unshift(feed);
    this.newFeed.emit(feed);
  }
}
