export interface PunchViewModel {
    id: string;
    creationDate: string;
    userId: string;
    source: PunchSource;
}

export enum PunchSource {
    unknown = 0,
    webPortal = 1,
    physical = 2
}

export interface PunchFilterDto
{
    date?: string;
}

export interface PunchEditDto
{
    newDate: string;
}