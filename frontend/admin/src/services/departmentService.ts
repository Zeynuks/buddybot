import type { DepartmentList, DepartmentDetail, DepartmentLookup } from "../models/Department";
import { API_URL } from "../constants/apiUrl";
import { fetchWithAuth } from "../utils/fetchWithAuth";
import { handleApiError } from "../utils/handleApiError";
import type { PagedResult } from "../models/PagedResult";
import { PAGE_SIZE } from "../constants/pagination";

export async function getDepartments(
    searchTerm?: string,
    pageNumber: number = 1,
    pageSize: number = PAGE_SIZE
): Promise<PagedResult<DepartmentList>> {
    const url = new URL(`${API_URL}/departments`);
    if (searchTerm) url.searchParams.append("searchTerm", searchTerm);
    url.searchParams.append("pageNumber", pageNumber.toString());
    url.searchParams.append("pageSize", pageSize.toString());
    const response = await fetchWithAuth(url.toString(), { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить отделы");
    }
    return response.json();
}

export async function getDepartment(id: number): Promise<DepartmentDetail> {
    const response = await fetchWithAuth(`${API_URL}/departments/${id}`, { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить данные отдела");
    }
    return response.json();
}

export async function getDepartmentsLookup(): Promise<DepartmentLookup[]> {
    const response = await fetchWithAuth(`${API_URL}/departments/lookup`, { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить lookup отделов");
    }
    return response.json();
}

export async function createDepartment(data: {
    name: string;
    headFirstName: string;
    headLastName: string;
    headVideoUrl?: string;
    headMicrosoftTeamsUrl?: string;
}): Promise<DepartmentDetail> {
    const response = await fetchWithAuth(`${API_URL}/departments`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
        credentials: "include",
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось создать отдел");
    }
    return response.json();
}

export async function updateDepartment(
    id: number,
    data: {
        name: string;
        headFirstName: string;
        headLastName: string;
        headMicrosoftTeamsUrl: string;
        headVideoUrl?: string;
    }
): Promise<DepartmentDetail> {
    const response = await fetchWithAuth(`${API_URL}/departments/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
        credentials: "include",
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось обновить данные отдела");
    }
    return response.json();
}

export async function deleteDepartment(id: number): Promise<void> {
    const response = await fetchWithAuth(`${API_URL}/departments/${id}`, { method: "DELETE" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось удалить отдел");
    }
}
