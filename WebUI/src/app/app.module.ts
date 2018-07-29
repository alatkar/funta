import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
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
import { RegisterUserComponent } from './user/register-user/register-user.component';
import { RegisterProfileComponent } from './user/profile/register-profile/register-profile.component';
import { SignupComponent } from './auth/signup/signup.component';
import { SigninComponent } from './auth/signin/signin.component';
import { AuthGuard } from './services/auth-guard.service';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FeedComponent,
    FeedCreateComponent,
    FeedDetailComponent,
    ProfileComponent,
    UserComponent,
    NotFoundComponent,
    RegisterUserComponent,
    RegisterProfileComponent,
    SignupComponent,
    SigninComponent
  ],
  imports: [
    FormsModule,
    HttpClientModule,
    BrowserModule,
    AngularFontAwesomeModule,
    RouterModule.forRoot([
      // Routes should be from most specific to general
      // Open URLs
      { path: '', component: FeedComponent /*HomeComponent*/ , canActivate: [AuthGuard]},
      { path: 'home', component: FeedComponent /*HomeComponent*/, canActivate: [AuthGuard] },
      { path: 'post', component: FeedCreateComponent /*HomeComponent*/ , canActivate: [AuthGuard]},
      { path: 'products', component: FeedComponent , canActivate: [AuthGuard]},
      { path: 'user', component: UserComponent , canActivate: [AuthGuard]},
      { path: 'user/:id', component: UserComponent , canActivate: [AuthGuard]},
      { path: 'profile', component: ProfileComponent , canActivate: [AuthGuard]},
      { path: 'profile/:id', component: ProfileComponent , canActivate: [AuthGuard]},
      { path: 'profile/:userName', component: ProfileComponent , canActivate: [AuthGuard]},
      { path: 'signup', component: SignupComponent },
      { path: 'signin', component: SigninComponent },
      { path: 'not-found', component: NotFoundComponent },
      { path: '**', redirectTo: 'not-found' },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
