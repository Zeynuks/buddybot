export interface City {
    id: number;
    name: string | null;
    countryId: number;
    countryName: string | null;
    candidateCount: number;
}

export interface CityLookup {
    id: number;
    name: string | null;
}
