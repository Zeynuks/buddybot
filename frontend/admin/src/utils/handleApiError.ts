export async function handleApiError(response: Response, fallback = "Неизвестная ошибка"): Promise<Error> {
    let message = fallback;
    try {
        const data = await response.json();
        if (data?.errors && typeof data.errors === "object") {
            const allErrors = Object.values(data.errors).flat();
            if (allErrors.length) {
                const first = allErrors[0];
                if (typeof first === "string") {
                    message = first;
                }
            }
        } else if (typeof data?.message === "string" && data.message.trim() !== "") {
            message = data.message;
        }
    } catch {
        // empty, usr fallback
    }
    return new Error(message);
}
