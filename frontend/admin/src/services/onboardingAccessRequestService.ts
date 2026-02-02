import type { OnboardingAccessRequest } from "../models/OnboardingAccessRequests";
import { API_URL } from "../constants/apiUrl";
import { fetchWithAuth } from "../utils/fetchWithAuth";
import { handleApiError } from "../utils/handleApiError";
import { PAGE_SIZE } from "../constants/pagination";
import type { PagedResult } from "../models/PagedResult";

export async function getOnboardingAccessRequests(
    searchTerm?: string,
    requestOutcome?: string,
    pageNumber: number = 1,
    pageSize: number = PAGE_SIZE
): Promise<PagedResult<OnboardingAccessRequest>> {
    const url = new URL(`${API_URL}/onboarding-access-requests`);
    if (searchTerm) url.searchParams.append("searchTerm", searchTerm);
    if (requestOutcome) url.searchParams.append("RequestOutcome", requestOutcome);
    url.searchParams.append("pageNumber", pageNumber.toString());
    url.searchParams.append("pageSize", pageSize.toString());

    const response = await fetchWithAuth(url.toString(), { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось загрузить заявки на онбординг");
    }
    return response.json();
}

export async function getOnboardingAccessRequestById(id: string): Promise<OnboardingAccessRequest> {
    const response = await fetchWithAuth(`${API_URL}/onboarding-access-requests/${id}`);
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось загрузить заявку");
    }
    return response.json();
}

export async function updateOnboardingAccessRequest(
    id: string,
    data: { onboardingAccessTimeUtc?: string; teamId?: number; mentorIds?: number[] }
): Promise<OnboardingAccessRequest> {
    const response = await fetchWithAuth(`${API_URL}/onboarding-access-requests/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось обновить заявку");
    }
    return response.json();
}

export async function confirmOnboardingAccessRequest(
    candidateId: number,
    data: { onboardingAccessTimeUtc?: string; teamId?: number; mentorIds?: number[] }
): Promise<OnboardingAccessRequest> {
    const response = await fetchWithAuth(`${API_URL}/onboarding-access-requests/${candidateId}/confirm`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось подтвердить заявку");
    }
    return response.json();
}

export async function rejectOnboardingAccessRequest(candidateId: number): Promise<OnboardingAccessRequest> {
    const response = await fetchWithAuth(`${API_URL}/onboarding-access-requests/${candidateId}/reject`, {
        method: "PUT",
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось отклонить заявку");
    }
    return response.json();
}

export async function deleteOnboardingAccessRequest(candidateId: number): Promise<void> {
    const response = await fetchWithAuth(`${API_URL}/onboarding-access-requests/${candidateId}`, { method: "DELETE" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось удалить заявку");
    }
}

export async function restoreOnboardingAccessRequest(candidateId: number): Promise<OnboardingAccessRequest> {
    const response = await fetchWithAuth(`${API_URL}/onboarding-access-requests/${candidateId}/restore`, {
        method: "PUT",
    });

    if (!response.ok) {
        throw await handleApiError(response, "Не удалось восстановить заявку");
    }

    return response.json();
}

export async function createOnboardingAccessRequest(
    candidateId: number,
    outcome?: "Scheduled"
): Promise<OnboardingAccessRequest> {
    const response = await fetchWithAuth(`${API_URL}/onboarding-access-requests`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ candidateId, outcome }),
    });

    if (!response.ok) {
        throw await handleApiError(response, "Не удалось создать заявку на онбординг");
    }

    return response.json();
}
