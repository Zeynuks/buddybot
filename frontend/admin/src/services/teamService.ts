import type { TeamList, TeamDetail, TeamLookup } from "../models/Teams";
import { API_URL } from "../constants/apiUrl";
import { fetchWithAuth } from "../utils/fetchWithAuth";
import { handleApiError } from "../utils/handleApiError";
import { PAGE_SIZE } from "../constants/pagination";
import type { PagedResult } from "../models/PagedResult";

export async function getTeams(
    departmentId?: number,
    searchTerm?: string,
    pageNumber: number = 1,
    pageSize: number = PAGE_SIZE
): Promise<PagedResult<TeamList>> {
    const url = new URL(`${API_URL}/teams`);
    if (typeof departmentId === "number") {
        url.searchParams.append("departmentId", departmentId.toString());
    }
    if (searchTerm) url.searchParams.append("searchTerm", searchTerm);
    url.searchParams.append("pageNumber", pageNumber.toString());
    url.searchParams.append("pageSize", pageSize.toString());

    const response = await fetchWithAuth(url.toString(), { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить команды");
    }
    return response.json();
}

export async function getTeam(id: number): Promise<TeamDetail> {
    const response = await fetchWithAuth(`${API_URL}/teams/${id}`, { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить команду");
    }
    return response.json();
}

export async function getTeamsLookup(departmentId?: number): Promise<TeamLookup[]> {
    let url = `${API_URL}/teams/lookup`;
    if (departmentId) {
        url += `?DepartmentId=${departmentId}`;
    }
    const response = await fetchWithAuth(url, { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить список команд");
    }
    return response.json();
}

export async function createTeam(data: { name: string; departmentId: number; leaderId?: number }): Promise<TeamDetail> {
    const response = await fetchWithAuth(`${API_URL}/teams`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
        credentials: "include",
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось создать команду");
    }
    return response.json();
}

export async function updateTeam(
    id: number,
    data: { name: string; departmentId: number; leaderId?: number }
): Promise<TeamDetail> {
    const response = await fetchWithAuth(`${API_URL}/teams/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
        credentials: "include",
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось обновить команду");
    }
    return response.json();
}

export async function deleteTeam(id: number): Promise<void> {
    const response = await fetchWithAuth(`${API_URL}/teams/${id}`, { method: "DELETE" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось удалить команду");
    }
}
