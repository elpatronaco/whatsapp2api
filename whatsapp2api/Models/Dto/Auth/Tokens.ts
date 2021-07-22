export interface ITokens {
    IdToken: string
    AccessToken: string
}

export interface AccessTokenPayload {
    Phone: string
    Username: string
}

export interface IdTokenPayload {
    Id: string
}