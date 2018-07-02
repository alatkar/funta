import { Address } from 'src/app/models/address';
import { Profile } from 'src/app/models/profile';

export class User {
    public id: string;
    public userName: string;
    public firstName: string;
    public lastName: string;
    public profileImage: string;
    public address: Address;
    public profiles: Profile[];
    public dateRegistereded: Date;
    // public dateLastUpdated: Date;

    constructor(id: string,
        userName: string,
        firstName: string,
        lastName: string,
        profileImage: string,
        address: Address,
        profiles: Profile[],
        dateRegistereded: Date) {
            this.id = id;
            this.userName = userName;
            this.firstName = firstName;
            this.lastName = lastName;
            this.profileImage = profileImage;
            this.address = address;
            this.profiles = profiles;
            this.dateRegistereded = dateRegistereded;
            // this.dateLastUpdated = dateLastUpdated;
    }
}
