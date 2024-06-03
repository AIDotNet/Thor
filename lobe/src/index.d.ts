export interface User extends Entity<string> {
    userName: string;
    email: string;
    password: string;
    passwordHas: string;
    avatar: string | null;
    role: string;
    isDisabled: boolean;
    isDelete: boolean;
    deletedAt: string | null;
    consumeToken: number;
    requestCount: number;
    residualCredit: number;
}

export interface Entity<TKey> {
    id: TKey;
    updatedAt: string | null;
    modifier: string | null;
    createdAt: string;
    creator: string | null;
}

export interface Creatable {
    createdAt: string;
    creator: string | null;
}

export interface Updatable {
    updatedAt: string | null;
    modifier: string | null;
}

export interface ResultDto {
    message: string;
    success: boolean;
    data: any | null;
}

export interface ResultDto<T> {
    message: string;
    success: boolean;
    data: T | null;
}