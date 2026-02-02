import { API_URL } from "../constants/apiUrl";
import { handleApiError } from "../utils/handleApiError";

export function getMediaUrl(id: string): string {
    return `${API_URL}/media/${id}`;
}

export async function uploadMedia(file: File): Promise<string> {
    const formData = new FormData();
    formData.append("file", file);

    const response = await fetch(`${API_URL}/media/upload`, {
        method: "POST",
        body: formData,
    });
    if (!response.ok) {
        throw await handleApiError(response, "Ошибка загрузки файла");
    }
    return response.text();
}

export async function downloadMedia(id: string): Promise<Blob> {
    const response = await fetch(`${API_URL}/media/${id}`);
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось скачать файл");
    }
    return response.blob();
}
