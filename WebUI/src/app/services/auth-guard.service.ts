import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router/src/interfaces';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import { RouterStateSnapshot } from '@angular/router/src/router_state';

@Injectable()
export class AuthGuard  implements CanActivate{

  constructor(private auth: AuthService, private router: Router) { }

  canActivate(route, state: RouterStateSnapshot) {
    return this.auth.user$.map(user => {
      if (user) {
        return true;
      }

      this.router.navigate(['/login'], {queryParams: {returnUrl: state.url}});
      return false;
    });
  }

}
