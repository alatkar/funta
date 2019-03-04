import { Address } from 'src/app/models/address';
import { Profile } from 'src/app/models/profile';

export class Login {
    public password: string;
    public userName: string;

    constructor(userName: string, password: string) {
            this.password = password;
            this.userName = userName;
    }
}
