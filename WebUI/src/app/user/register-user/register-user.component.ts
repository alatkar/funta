import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent implements OnInit {
  @ViewChild('registerUserForm') form: NgForm;

  constructor() { }

  ngOnInit() {
  }

  public onSubmit(form: NgForm) {
    this.form = form;
    console.log(form.value);
  }
}
