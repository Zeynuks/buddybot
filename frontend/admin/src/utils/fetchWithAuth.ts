import { AuthenticationService } from "../services/authService";

export async function fetchWithAuth(input: RequestInfo, init?: RequestInit, retry = true): Promise<Response> {
    const authService = new AuthenticationService();
    let accessToken = authService.getAccessToken();

    const headers: HeadersInit = {
        ...(init?.headers || {}),
        ...(accessToken ? { Authorization: `Bearer ${accessToken}` } : {}),
    };

    let response = await fetch(input, { ...init, headers });

    if (response.status === 401 && retry && authService.getRefreshToken()) {
        const refreshResult = await authService.refreshToken();
        if (refreshResult.accessToken) {
            accessToken = refreshResult.accessToken;
            const headersWithNewToken: HeadersInit = {
                ...(init?.headers || {}),
                Authorization: `Bearer ${accessToken}`,
            };
            response = await fetch(input, { ...init, headers: headersWithNewToken });
        } else {
            authService.logout();
        }
    }

    return response;
}
