import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import * as firebase from 'firebase';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  token: string;
  constructor(private router: Router) { }

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
    firebase.auth().currentUser.getIdToken()
    .then ((token: string) => this.token = token);

    return this.token;
  }

  isAuthenticated() {
    return this.token != null;
  }

  getUserId() {
    return firebase.auth().currentUser.uid;
  }
}
