export function parseTelegramUsername(input: string): string {
    if (!input) return "";

    let value = input.trim().replace(/^\/+|\/+$/g, "");

    value = value
        .replace(/^@/, "")
        .replace(/^(https?:\/\/)?t\.me\//, "")
        .replace(/^t\.me\//, "");

    value = value.replace(/\s/g, "");
    value = value.replace(/\//g, "");

    return value;
}
