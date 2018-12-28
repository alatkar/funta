import { Subscription ,  Subscribable } from 'rxjs';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { ActivatedRoute, Params } from '@angular/router';
import { Component, OnInit, Input } from '@angular/core';
import { Profile } from 'src/app/models/profile';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnDestroy {

  // @Input('profile') profile: Profile;
  profile: Profile = null;
  sub: Subscription;

  // profile: {id: number, name: string};
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.profile = new Profile('1', 'Rocky', 'Labrador',
    'https://upload.wikimedia.org/wikipedia/commons/f/f5/Howlsnow.jpg',
    '2', new Date('04/05/2018'));
    this.profile.id =  this.route.snapshot.params['id'];
    this.profile.profileName = 'Atul..load runtime';
    // For loading profile from profile
    this.sub = this.route.params.subscribe(
      (params: Params) => {
        this.profile.id = params['id'];
        this.profile.profileName = 'Atul...load from service or somewhere';
      }
    );
  }

  ngOnDestroy(): void {
    // this.sub.unsubscribe();
  }
}
