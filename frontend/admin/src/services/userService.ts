import type { UserDetail, UserLookup } from "../models/User";
import { API_URL } from "../constants/apiUrl";
import { fetchWithAuth } from "../utils/fetchWithAuth";
import { handleApiError } from "../utils/handleApiError";
import { PAGE_SIZE } from "../constants/pagination";
import type { PagedResult } from "../models/PagedResult";

export async function getCurrentUser(): Promise<UserLookup> {
    const response = await fetchWithAuth(`${API_URL}/users/me`);
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить пользователя");
    }
    return response.json();
}

export async function getUsersLookup(params?: { roles?: string[]; teamId?: number }): Promise<UserLookup[]> {
    const query = new URLSearchParams();
    if (params?.roles) {
        params.roles.forEach((r) => query.append("Roles", r));
    }
    if (params?.teamId) query.append("TeamId", params.teamId.toString());
    const response = await fetchWithAuth(`${API_URL}/users/lookup?${query.toString()}`, { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить пользователей");
    }
    return response.json();
}

export async function getUserById(id: number): Promise<UserDetail> {
    const response = await fetchWithAuth(`${API_URL}/users/${id}`, { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить данные пользователя");
    }
    return response.json();
}

export async function createMentor(data: {
    firstName: string;
    lastName: string;
    mentorPhotoUrl?: string;
    microsoftTeamsUrl?: string;
    teamId: number;
    isTeamLeader?: boolean;
}): Promise<UserDetail> {
    const response = await fetchWithAuth(`${API_URL}/users/create-mentor`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось создать наставника");
    }
    return response.json();
}

export async function updateCandidate(
    id: number,
    {
        firstName,
        lastName,
        teamId,
        hrIds,
        mentorIds,
        microsoftTeamsUrl,
        photoUrl,
        videoUrl,
        onboardingAccessTimeUtc,
    }: {
        firstName?: string;
        lastName?: string;
        teamId?: number;
        hrIds?: number[];
        mentorIds?: number[];
        microsoftTeamsUrl?: string;
        photoUrl?: string;
        videoUrl?: string;
        onboardingAccessTimeUtc?: string;
    }
): Promise<UserDetail> {
    const response = await fetchWithAuth(`${API_URL}/users/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            firstName,
            lastName,
            teamId,
            hrIds,
            mentorIds,
            microsoftTeamsUrl,
            photoUrl,
            videoUrl,
            onboardingAccessTimeUtc,
        }),
        credentials: "include",
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось обновить данные кандидата");
    }
    return response.json();
}

export async function updateMember(
    id: number,
    {
        teamId,
        firstName,
        lastName,
        microsoftTeamsUrl,
        photoUrl,
        telegramContact,
    }: {
        teamId?: number;
        firstName?: string;
        lastName?: string;
        microsoftTeamsUrl?: string;
        photoUrl?: string | null;
        telegramContact?: string;
    }
): Promise<UserDetail> {
    const response = await fetchWithAuth(`${API_URL}/users/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            teamId,
            firstName,
            lastName,
            microsoftTeamsUrl,
            photoUrl,
            telegramContact,
        }),
        credentials: "include",
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось обновить данные участника");
    }
    return response.json();
}

export async function getUsers(
    searchTerm?: string,
    pageNumber: number = 1,
    pageSize: number = PAGE_SIZE,
    roles?: string[],
    departmentId?: number,
    teamId?: number,
    processKind?: string
): Promise<PagedResult<UserDetail>> {
    const url = new URL(`${API_URL}/users`);
    if (searchTerm) url.searchParams.append("searchTerm", searchTerm);
    url.searchParams.append("pageNumber", pageNumber.toString());
    url.searchParams.append("pageSize", pageSize.toString());
    if (roles) roles.forEach((r) => url.searchParams.append("Roles", r));
    if (departmentId !== undefined) url.searchParams.append("DepartmentId", departmentId.toString());
    if (teamId !== undefined) url.searchParams.append("TeamId", teamId.toString());
    if (processKind) url.searchParams.append("ProcessKind", processKind);

    const response = await fetchWithAuth(url.toString(), { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить пользователей");
    }
    return response.json();
}

export async function deleteUser(id: number): Promise<void> {
    const response = await fetchWithAuth(`${API_URL}/users/${id}`, { method: "DELETE" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось удалить пользователя");
    }
}
