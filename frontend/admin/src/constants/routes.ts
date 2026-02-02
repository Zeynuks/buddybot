export const ROUTES = {
    AUTH: {
        LOGIN: "/login",
    },
    DASHBOARD: "/",
    DEPARTMENTS: {
        ROOT: "/departments",
        CREATE: "/departments/create",
        EDIT: (id: string | number) => `/departments/${id}/edit`,
    },
    TEAMS: {
        ROOT: "/teams",
        CREATE: "/teams/create",
        EDIT: (id: string | number) => `/teams/${id}/edit`,
    },
    MEMBERS: {
        ROOT: "/members",
        CREATE_MENTOR: "/members/create-mentor",
        EDIT: (id: string | number) => `/members/${id}/edit`,
    },
    COUNTRIES: {
        ROOT: "/countries",
        CREATE: "/countries/create",
        EDIT: (id: string | number) => `/countries/${id}/edit`,
    },
    CITIES: {
        BY_COUNTRY: (countryId: number | string) => `/countries/${countryId}/cities`,
        CREATE: (countryId: number | string) => `/countries/${countryId}/cities/create`,
        EDIT: (countryId: number | string, id: number | string) => `/countries/${countryId}/cities/${id}/edit`,
    },
    INVITATIONS: {
        ROOT: "/invitations",
        CREATE_CANDIDATE: "/invitations/create-candidate",
        CREATE_HR: "/invitations/create-hr",
        DETAIL: (token: string | number) => `/invitations/${token}`,
        EDIT: (token: string | number) => `/invitations/${token}/edit`,
    },
    CANDIDATES: {
        ROOT: "/candidates",
        EDIT: (id: string | number) => `/candidates/${id}/edit`,
    },
    ONBOARDING_ACCESS_REQUESTS: {
        ROOT: "/onboarding-access-requests",
        EDIT: (id: string | number) => `/onboarding-access-requests/${id}/edit`,
    },
    FEEDBACKS: {
        ROOT: "/feedbacks",
    },
    TEST: "/test",
};
