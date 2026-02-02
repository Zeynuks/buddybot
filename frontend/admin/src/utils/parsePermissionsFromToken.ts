export function parsePermissionsFromToken(token: string | null): string[] {
    if (!token) return [];
    try {
        const payloadBase64 = token.split(".")[1];
        const payload = JSON.parse(atob(payloadBase64));
        if (Array.isArray(payload.Permission)) {
            return payload.Permission;
        }
        return [];
    } catch {
        return [];
    }
}
