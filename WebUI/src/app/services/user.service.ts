import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { Profile } from './../models/profile';
import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { Address } from '../models/address';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor() { }

  getUserById(id: string) {
    const profiles: Profile[] = [];
    profiles.unshift(this.getProfileById('1'));
    profiles.unshift(this.getProfileByProfileName('1'));

    return new User('1', 'admin', 'Admin', 'Funta',
    'https://www.maxpixel.net/static/photo/1x/Sweet-Grass-Nice-Dog-Brown-Bitch-Dogs-Sun-2679538.jpg',
      new Address(), profiles, new Date('04/04/2018'));
  }

  getUserByUserName(userName: string) {
    const profiles: Profile[] = [];
    profiles.unshift(this.getProfileById('1'));
    profiles.unshift(this.getProfileByProfileName('1'));

    return new User('1', 'admin', 'Admin', 'Funta',
      'https://www.maxpixel.net/static/photo/1x/Sweet-Grass-Nice-Dog-Brown-Bitch-Dogs-Sun-2679538.jpg',
      new Address(), profiles, new Date('04/04/2018'));
  }

  getProfileById(id: string) {
    return new Profile('1', 'Rocky', 'Labrador',
    'https://upload.wikimedia.org/wikipedia/commons/f/f5/Howlsnow.jpg',
    '2', new Date('04/05/2018'));
  }

  getProfileByProfileName(id: string) {
    return new Profile('2', 'Rocky', 'Labrador',
    'https://upload.wikimedia.org/wikipedia/commons/8/8a/Too-cute-doggone-it-video-playlist.jpg',
    '2', new Date('04/05/2018'));
  }
}
