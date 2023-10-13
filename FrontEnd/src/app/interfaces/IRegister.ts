import { IAddress } from "./IAddress";

export interface IRegister {
    id: number,
    email: string,
    name: string,
    surname: string,
    password: string,
    userType: string,
    telephone: string,
    address: IAddress
}
