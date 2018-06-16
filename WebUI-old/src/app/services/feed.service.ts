import { Injectable } from '@angular/core';
import { Feed } from '../models/Feed';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class FeedService {

  constructor(private http: HttpClient) { }

  private heroesUrl = 'http://localhost:5000/api/feed/';

  getFeed() : Observable<Feed[]> {
    return this.http.get<Feed[]>(this.heroesUrl);
  }
}
