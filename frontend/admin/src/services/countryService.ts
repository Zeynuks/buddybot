import type { Country } from "../models/Countries";
import { API_URL } from "../constants/apiUrl";
import { fetchWithAuth } from "../utils/fetchWithAuth";
import { handleApiError } from "../utils/handleApiError";
import { PAGE_SIZE } from "../constants/pagination";
import type { PagedResult } from "../models/PagedResult";

export async function getCountries(
    searchTerm?: string,
    pageNumber: number = 1,
    pageSize: number = PAGE_SIZE
): Promise<PagedResult<Country>> {
    const url = new URL(`${API_URL}/countries`);
    if (searchTerm) url.searchParams.append("searchTerm", searchTerm);
    url.searchParams.append("pageNumber", pageNumber.toString());
    url.searchParams.append("pageSize", pageSize.toString());

    const response = await fetchWithAuth(url.toString(), { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить страны");
    }
    return response.json();
}

export async function getCountry(id: number): Promise<Country> {
    const response = await fetchWithAuth(`${API_URL}/countries/${id}`, { credentials: "include" });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось получить страну");
    }
    return response.json();
}

export async function createCountry(data: { name: string }): Promise<Country> {
    const response = await fetchWithAuth(`${API_URL}/countries`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
        credentials: "include",
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось создать страну");
    }
    return response.json();
}

export async function updateCountry(id: number, data: { name: string }): Promise<Country> {
    const response = await fetchWithAuth(`${API_URL}/countries/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
        credentials: "include",
    });
    if (!response.ok) {
        throw await handleApiError(response, "Не удалось обновить страну");
    }
    return response.json();
}
