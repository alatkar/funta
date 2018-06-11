import { Component, OnInit } from '@angular/core';
import { FirebaseApp } from 'angularfire2';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private auth: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.auth.login();
  }

}
