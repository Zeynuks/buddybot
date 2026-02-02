export interface Authentication {
    Login: string;
    Password: string;
}

export interface Token {
    accessToken: string | null;
    refreshToken: string | null;
    errorMessage?: unknown;
}
