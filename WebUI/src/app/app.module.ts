import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AngularFontAwesomeModule } from 'angular-font-awesome';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { FeedComponent } from './feed/feed.component';
import { FeedCreateComponent } from './feed/feed-create/feed-create.component';
import { FeedDetailComponent } from './feed/feed-detail/feed-detail.component';

import { UserComponent } from './user/user.component';
import { ProfileComponent } from './user/profile/profile.component';
import { NotFoundComponent } from './not-found/not-found.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FeedComponent,
    FeedCreateComponent,
    FeedDetailComponent,
    ProfileComponent,
    UserComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    AngularFontAwesomeModule,
    RouterModule.forRoot([
      // Routes should be from most specific to general
      // Open URLs
      { path: '', component: FeedComponent /*HomeComponent*/ },
      { path: 'home', component: FeedComponent /*HomeComponent*/ },
      { path: 'products', component: FeedComponent },
      { path: 'user', component: UserComponent },
      { path: 'user/:id', component: UserComponent },
      { path: 'profile', component: ProfileComponent },
      { path: 'profile/:id', component: ProfileComponent },
      { path: 'profile/:userName', component: ProfileComponent },
      { path: 'login', component: FeedComponent },
      { path: 'not-found', component: NotFoundComponent },
      { path: '**', redirectTo: 'not-found' },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
