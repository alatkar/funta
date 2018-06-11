export interface Feed {
    $id: string;
    userName: string;
    data: string;
    type: string;
    imageUrl: string;
    dateCreated: Date;
    dateLastUpdated: Date;
    followups : Feed[];
}