import { FeedDetailComponent } from './../feed/feed-detail/feed-detail.component';
import { FeedTypeEnum } from './feedTypeEnum';
export class Feed {
    public id: string;
    public userName: string;
    public heading: string;
    public data: string;
    public imageUrl: string;
    public type: FeedTypeEnum;
    public followups: Feed;
    public dateCreated: Date;
    public dateLastUpdated: Date;

    constructor(id: string,
         userName: string,
         heading: string,
         data: string,
         imageUrl: string,
         type: FeedTypeEnum,
         followups: Feed,
         dateCreated: Date,
         dateLastUpdated: Date) {
             this.id = id;
             this.userName = userName;
             this.heading = heading;
             this.data = data;
             this.imageUrl = imageUrl;
             this.type = type;
             this.followups = followups;
             this.dateCreated = dateCreated;
             this.dateLastUpdated = dateLastUpdated;
    }
}
