import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  constructor(private userService: UserService, private auth: AuthService, router: Router){
    auth.user$.subscribe(user => { 
      // No need of unsubscribe as this is root component.
      // TODO: Implement unsubscribe
      // Need condition as user can log out and this will be null
      //TODO: In future, save only when needed. 
      if(!user) return;

      userService.save(user);
      let returnUrl = localStorage.getItem('returnUrl');

      if(!returnUrl) return;
     
      localStorage.removeItem('returnUrl');
      router.navigateByUrl(returnUrl);
    })
  }
}
