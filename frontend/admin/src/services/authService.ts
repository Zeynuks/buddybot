import type { Token, Authentication } from "../models/Auth";
import { Cookies } from "react-cookie";
import { API_URL } from "../constants/apiUrl";

export class AuthenticationService {
    private apiUrl: string;
    private cookies: Cookies;

    constructor(apiUrl: string = `${API_URL}/users`) {
        this.apiUrl = apiUrl;
        this.cookies = new Cookies();
    }

    getAccessToken(): string | null {
        return localStorage.getItem("AccessToken");
    }

    getRefreshToken(): string | null {
        return this.cookies.get("RefreshToken") || null;
    }

    setTokens(tokens: { accessToken?: string | null; refreshToken?: string | null }) {
        if (tokens.accessToken) localStorage.setItem("AccessToken", tokens.accessToken);
        if (tokens.refreshToken) this.cookies.set("RefreshToken", tokens.refreshToken, { path: "/" });
    }

    clearTokens() {
        localStorage.removeItem("AccessToken");
        this.cookies.remove("RefreshToken", { path: "/" });
    }

    async authentication(body: Authentication): Promise<Token> {
        try {
            const response = await fetch(`${this.apiUrl}/login`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(body),
            });
            if (!response.ok) {
                const err = await response.json().catch(() => ({}));
                throw new Error(err.message || "Ошибка авторизации");
            }
            const data: Token = await response.json();
            this.clearTokens();
            this.setTokens({ accessToken: data.accessToken, refreshToken: data.refreshToken });
            return data;
        } catch (error) {
            return {
                accessToken: null,
                refreshToken: null,
                errorMessage: error instanceof Error ? error.message : String(error),
            };
        }
    }

    async refreshToken(): Promise<Token> {
        try {
            const refreshToken = this.getRefreshToken();
            if (!refreshToken) {
                throw new Error("Отсутствует refreshToken");
            }
            const response = await fetch(`${this.apiUrl}/refresh-token`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                credentials: "include",
                body: JSON.stringify({ refreshToken }),
            });
            if (!response.ok) {
                throw new Error("Не удалось обновить токен");
            }
            const data: Token = await response.json();
            this.setTokens({ accessToken: data.accessToken, refreshToken: data.refreshToken });
            return data;
        } catch (error) {
            this.logout();
            return {
                accessToken: null,
                refreshToken: null,
                errorMessage: error instanceof Error ? error.message : String(error),
            };
        }
    }

    logout(): void {
        fetch(`${this.apiUrl}/logout`, {
            method: "POST",
            headers: {
                Authorization: `Bearer ${this.getAccessToken()}`,
                "Content-Type": "application/json",
            },
            credentials: "include",
        }).catch(() => {});
        this.clearTokens();
    }
}
