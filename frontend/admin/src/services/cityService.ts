import type { City } from "../models/Cities";
import type { PagedResult } from "../models/PagedResult";
import { API_URL } from "../constants/apiUrl";
import { fetchWithAuth } from "../utils/fetchWithAuth";
import { handleApiError } from "../utils/handleApiError";
import { PAGE_SIZE } from "../constants/pagination";

export async function getCities(
    countryId: number,
    searchTerm?: string,
    pageNumber: number = 1,
    pageSize: number = PAGE_SIZE
): Promise<PagedResult<City>> {
    const url = new URL(`${API_URL}/cities`);
    url.searchParams.append("countryId", countryId.toString());
    if (searchTerm) url.searchParams.append("searchTerm", searchTerm);
    url.searchParams.append("pageNumber", pageNumber.toString());
    url.searchParams.append("pageSize", pageSize.toString());

    const response = await fetchWithAuth(url.toString(), { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить города");
    }
    return response.json();
}

export async function getCity(id: number): Promise<City> {
    const response = await fetchWithAuth(`${API_URL}/cities/${id}`, { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить город");
    }
    return response.json();
}

export async function createCity(data: { name: string; countryId: number }): Promise<City> {
    const response = await fetchWithAuth(`${API_URL}/cities`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
        credentials: "include",
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось создать город");
    }
    return response.json();
}

export async function updateCity(id: number, data: { name: string; countryId: number }): Promise<City> {
    const response = await fetchWithAuth(`${API_URL}/cities/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
        credentials: "include",
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось обновить город");
    }
    return response.json();
}
