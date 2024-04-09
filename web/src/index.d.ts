export interface ChatChannel extends Entity<string> {
    order: number;
    type: string;
    name: string;
    address: string;
    responseTime: string | null;
    key: string;
    models: string[];
    other: string;
    disable: boolean;
    extension: { [key: string]: string; };
}

export interface Token extends Entity<number> {
    key: string;
    name: string;
    usedQuota: number;
    unlimitedQuota: boolean;
    remainQuota: number;
    accessedTime: string | null;
    expiredTime: string | null;
    unlimitedExpired: boolean;
    disabled: boolean;
    isDelete: boolean;
    deletedAt: string | null;
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

export interface SoftDeletion {
    isDelete: boolean;
    deletedAt: string | null;
}
export interface ChatLogger extends Entity<number> {
    type: ChatLoggerType;
    content: string;
    promptTokens: number;
    completionTokens: number;
    quota: number;
    modelName: string;
    tokenName: string;
    userName: string | null;
    userId: string | null;
}