import { FeedDetailComponent } from './../feed/feed-detail/feed-detail.component';
export class Feed {
    public id: string;
    public userName: string;
    public heading: string;
    public detail: string;
    public imageUrl: string;
    public type: string;
    public followups: Feed;
    public dateCreated: Date;
    public dateLastUpdated: Date;

    constructor(id: string,
         userName: string,
         heading: string,
         detail: string,
         imageUrl: string,
         type: string,
         followups: Feed,
         dateCreated: Date,
         dateLastUpdated: Date) {
             this.id = id;
             this.userName = userName;
             this.heading = heading;
             this.detail = detail;
             this.imageUrl = imageUrl;
             this.type = type;
             this.followups = followups;
             this.dateCreated = dateCreated;
             this.dateLastUpdated = dateLastUpdated;
    }
}
