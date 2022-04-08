export interface PunchInterface {
    id: string;
    creationDate: string;
    userId: string;
    source: PunchSource;
}

export enum PunchSource {
    Unknown = 0,
    WebPortal = 1,
    Physical = 2
}