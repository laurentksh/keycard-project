export interface LoginDto {
    email: string;
    password: string;
}

export interface AuthGrantViewModel {
    id: string;
    token: string;
    device: string;
    type: AuthGrantType;
    issuedToId: string;
    creationDate: string;
    expirationDate: string;
}

export enum AuthGrantType {
    unknown = 0,
    jwt = 1,
    physical = 2
}