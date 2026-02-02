import type { InvitationDetail, InvitationList } from "../models/Invitation";
import { API_URL } from "../constants/apiUrl";
import { fetchWithAuth } from "../utils/fetchWithAuth";
import { handleApiError } from "../utils/handleApiError";
import { PAGE_SIZE } from "../constants/pagination";
import type { PagedResult } from "../models/PagedResult";

export async function getInvitations(
    searchTerm?: string,
    pageNumber: number = 1,
    pageSize: number = PAGE_SIZE,
    status?: string,
    roles?: string | string[]
): Promise<PagedResult<InvitationList>> {
    const url = new URL(`${API_URL}/account-creation-tokens`);
    if (searchTerm) url.searchParams.append("searchTerm", searchTerm);
    if (status) url.searchParams.append("status", status);
    url.searchParams.append("pageNumber", pageNumber.toString());
    url.searchParams.append("pageSize", pageSize.toString());
    if (roles) {
        if (Array.isArray(roles)) {
            roles.forEach((role) => url.searchParams.append("roles", role));
        } else {
            url.searchParams.append("roles", roles);
        }
    }

    const response = await fetchWithAuth(url.toString(), { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить приглашения");
    }
    return response.json();
}

export async function getInvitationByToken(tokenValue: string): Promise<InvitationDetail> {
    const response = await fetchWithAuth(`${API_URL}/account-creation-tokens/${tokenValue}`);
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить приглашение");
    }
    return response.json();
}

export async function createHRInvitation(data: {
    login: string;
    password: string;
    firstName: string;
    lastName: string;
    telegramContact?: string;
    microsoftTeamsUrl?: string;
    teamId: number;
    expirationDays: number;
    creatorId: number;
}): Promise<InvitationDetail> {
    const response = await fetchWithAuth(`${API_URL}/account-creation-tokens/hr`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось создать приглашение для HR");
    }
    return response.json();
}

export async function createCandidateInvitation(data: {
    firstName: string;
    lastName: string;
    teamId: number;
    expirationDays: number;
    mentorIds?: number[];
    hrIds?: number[];
    creatorId: number;
}): Promise<InvitationDetail> {
    const response = await fetchWithAuth(`${API_URL}/account-creation-tokens/candidate`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось создать приглашение для кандидата");
    }
    return response.json();
}

export async function updateInvitation(
    tokenValue: string,
    data: {
        firstName?: string;
        lastName?: string;
        teamId?: number;
        expireDate?: string;
        mentorIds?: number[];
        hrIds?: number[];
        telegramContact?: string;
    }
): Promise<InvitationDetail> {
    const response = await fetchWithAuth(`${API_URL}/account-creation-tokens/${tokenValue}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось обновить приглашение");
    }
    return response.json();
}

export async function revokeInvitation(tokenValue: string): Promise<InvitationDetail> {
    const response = await fetchWithAuth(`${API_URL}/account-creation-tokens/${tokenValue}/revoke`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось отозвать приглашение");
    }
    return response.json();
}

export async function deleteInvitation(tokenValue: string): Promise<void> {
    const response = await fetchWithAuth(`${API_URL}/account-creation-tokens/${tokenValue}`, { method: "DELETE" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось удалить приглашение");
    }
}
