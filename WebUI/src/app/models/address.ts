export class Address {
    public line1: string;
    public line2: string;
    public city: string;
    public country: string;
    public zipCode: string;

    constructor(line1: string, line2: string, city: string, country: string, zipCode: string) {
        this.line1 = line1;
        this.line2 = line2;
        this.city = city;
        this.country = country;
        this.zipCode = zipCode;
    }
}
