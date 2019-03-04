import { NgForm } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Login } from 'src/app/models/login';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  onSignIn(f: NgForm) {
    console.log('Reeived Form' + f);
    this.authService.login(new Login(f.value.userName, f.value.password));
    //this.authService.signIn(f.value.email, f.value.password);
  }
}
