import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AngularFontAwesomeModule } from 'angular-font-awesome';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { FeedComponent } from './feed/feed.component';
import { FeedCreateComponent } from './feed/feed-create/feed-create.component';
import { FeedDetailComponent } from './feed/feed-detail/feed-detail.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FeedComponent,
    FeedCreateComponent,
    FeedDetailComponent
  ],
  imports: [
    BrowserModule,
    AngularFontAwesomeModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
