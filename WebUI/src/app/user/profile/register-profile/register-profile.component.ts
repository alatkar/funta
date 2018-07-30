import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-register-profile',
  templateUrl: './register-profile.component.html',
  styleUrls: ['./register-profile.component.css']
})
export class RegisterProfileComponent implements OnInit {
  @ViewChild('registerProfileForm') form: NgForm;
  profileTypes = ['Dog Owner', 'NGO', 'Dog Service', 'Dog Product'];
  defaultProfileType: string = this.profileTypes[0];
  constructor() { }

  ngOnInit() {
  }

  onSubmit(form: NgForm) {
    this.form = form;
    console.log(form.value);
  }
}
