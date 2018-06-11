import { Injectable } from '@angular/core';
import { AngularFireAuth } from 'angularfire2/auth';
import * as firebase from 'firebase';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute } from '@angular/router';
import { AppUser } from '../models/app-user';
import { UserService } from './user.service';
import 'rxjs/add/operator/switchMap';

@Injectable()
export class AuthService {

  //TODO: Add our own User object for not exposing firebase implementation
  user$: Observable<firebase.User>;

  constructor(private userService: UserService, private afAuth: AngularFireAuth, private route: ActivatedRoute) { 
    //Use pipe to unsubscribe from this observable. Other way to do it is implement OnDestroy interface
    this.user$ = afAuth.authState;
  }

  login(){
    let returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || '/';
    localStorage.setItem('returnUrl', returnUrl);
    this.afAuth.auth.signInWithRedirect(new firebase.auth.GoogleAuthProvider());
  }

  logout(){
    this.afAuth.auth.signOut();
  }

  get appUser$() : Observable<AppUser>{
    return this.user$
    //.switchMap(user => this.userService.get(user.uid)); //This caused error "Cannot read property 'uid' of null"
    .switchMap(user => {
      if (user) return this.userService.get(user.uid);
      else return Observable.of(null);
    });
  }

}
