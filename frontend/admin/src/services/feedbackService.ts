import type { Feedback } from "../models/Feedbacks";
import { API_URL } from "../constants/apiUrl";
import { fetchWithAuth } from "../utils/fetchWithAuth";
import { handleApiError } from "../utils/handleApiError";
import { PAGE_SIZE } from "../constants/pagination";
import type { PagedResult } from "../models/PagedResult";

export async function getFeedbacks(
    searchTerm?: string,
    processKind?: string,
    rating?: number,
    pageNumber: number = 1,
    pageSize: number = PAGE_SIZE
): Promise<PagedResult<Feedback>> {
    const url = new URL(`${API_URL}/feedbacks`);
    if (searchTerm) url.searchParams.append("searchTerm", searchTerm);
    if (processKind) url.searchParams.append("StageType", processKind);
    if (rating !== undefined) url.searchParams.append("Rating", rating.toString());
    url.searchParams.append("pageNumber", pageNumber.toString());
    url.searchParams.append("pageSize", pageSize.toString());

    const response = await fetchWithAuth(url.toString(), { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось загрузить отзывы");
    }
    return response.json();
}

export async function deleteFeedback(id: number): Promise<void> {
    const response = await fetchWithAuth(`${API_URL}/feedbacks/${id}`, { method: "DELETE" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось удалить отзыв");
    }
}

export async function exportFeedbacks(): Promise<Blob> {
    const response = await fetchWithAuth(`${API_URL}/feedbacks/export`, { method: "GET" });

    if (!response.ok) {
        throw await handleApiError(response, "Не удалось экспортировать отзывы");
    }

    return await response.blob();
}
