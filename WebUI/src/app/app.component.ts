import * as firebase from 'firebase';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';

  ngOnInit(): void {
    firebase.initializeApp({
      apiKey: 'ReplaceFirebaseKey_From_Azure_Portal',
      authDomain: 'funtadb.firebaseapp.com',
    });
  }
}
