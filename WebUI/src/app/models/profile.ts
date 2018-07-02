export class Profile {
    public id: string;
    public profileName: string;
    public breed: string;
    public profileImage: string;
    public age: string;
    public dateCreated: Date;
    // public dateLastUpdated: Date;

    constructor(id: string,
        profileName: string,
        breed: string,
        profileImage: string,
        age: string,
        dateCreated: Date) {
            this.id = id;
            this.profileName = profileName;
            this.breed = breed;
            this.profileImage = profileImage;
            this.age = age;
            this.dateCreated = dateCreated;
            // this.dateLastUpdated = dateLastUpdated;
    }
}
