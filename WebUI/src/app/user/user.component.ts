import { Profile } from './../models/profile';
import { UserService } from './../services/user.service';
import { Component, OnInit } from '@angular/core';
import { User } from '../models/user';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  /*
  profiles = [
    {
      id: 1,
      name: 'Rocky',
      userName: 'rockyaa'
    },
    {
      id: 2,
      name: 'Aris',
      userName: 'arisaa'
    },
    {
      id: 3,
      name: 'Kalu',
      userName: 'kaluaa'
    }
  ];*/

  profiles: Profile[] = [];
  user: User = null;

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.user = this.userService.getUserById('1');
  }

}
