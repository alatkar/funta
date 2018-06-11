import { Injectable } from '@angular/core';
import { AngularFireDatabase } from 'angularfire2/database';
import { FirebaseListObservable } from 'angularfire2/database-deprecated';

@Injectable()
export class CategoryService {
  constructor(private db: AngularFireDatabase) { }

  /*
  getCategories() {
    return this.db.list('/categories', ref=> {
        let q = ref.orderByChild('name');
        return q;
    });
  }*/

  public getAll() {
    return this.db
      .list('/categories', ref => ref.orderByChild('name'))
      .snapshotChanges().map(action => {
        // This changed from lecture due to version change
        return action.map(
          item => {
            const $key = item.payload.key;
            const data = { $key, ...item.payload.val() };
            return data;
        });
      });
  }
}
