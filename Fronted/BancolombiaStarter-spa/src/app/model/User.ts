export class User {
    userName: string;
    fullName: string;
    rols: string;
    token?: string;
    pictureUrl:string;
}


export class AuthenticateResponse {
    result: Result;
    targetUrl: string;
    success: string;
    error: string;
}

export class Result {
    accessToken: string;
    encryptedAccessToken: string;
    expireInSeconds: string;
    userId: string;
}