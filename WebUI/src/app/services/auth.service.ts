import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import * as firebase from 'firebase';
import { Login } from '../models/login';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  token: string;
  user: User[] = [];
  constructor(private router: Router, private service: HttpClient) {
    this.token = null;
   }

  login(login: Login) {
    this.service.post<Login>('https://localhost:44344/api/account/LoginAsync', login).toPromise()
    .then(
    resp => {
      console.log('AuthService:signIn success' + resp);
      this.router.navigate(['/']);
      this.token = login.userName;
   })
  .catch(error => console.log('AuthService:signIn failed ' + error));
  }

  signUp(email: string, password: string) {
    firebase.auth().createUserWithEmailAndPassword(email, password)
    .then(resp => console.log('AuthService:signUp success ' + resp))
    .catch(error => console.log('AuthService:signUp failed ' + error));
  }

  signIn(email: string, password: string) {
    firebase.auth().signInWithEmailAndPassword(email, password)
    .then(
      resp => {
        console.log('AuthService:signIn success' + resp);
        this.router.navigate(['/']);
        firebase.auth().currentUser.getIdToken()
          .then ((token: string) => this.token = token);
     })
    .catch(error => console.log('AuthService:signIn failed ' + error));
  }

  signOut() {
    firebase.auth().signOut();
    this.token = null;
    this.router.navigate(['/signin']);
  }

  getToken() {
    //firebase.auth().currentUser.getIdToken()
    //.then ((token: string) => this.token = token);
    return this.token;
  }

  isAuthenticated() {
    return this.token != null && this.token.length > 2;
  }

  getUserId() {
    return firebase.auth().currentUser.uid;
  }

  private getHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders().set('Content-Type', 'application/json')
    .set('authorization', 'Bearer ' + token);
  }
}
