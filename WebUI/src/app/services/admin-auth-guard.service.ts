import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AuthService } from './auth.service';
import { UserService } from './user.service';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';
import { AngularFireObject, AngularFireDatabase } from 'angularfire2/database';

@Injectable()
export class AdminAuthGuard implements CanActivate{
  
    constructor(private auth: AuthService, private userService: UserService) { }
  
    canActivate(): Observable<boolean> {
      return this.auth.appUser$
      //.switchMap(user => this.userService.get(user.uid)) // moved to auth service
      .map(appUser => appUser.isAdmin);
    }
  } 