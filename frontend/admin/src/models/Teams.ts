export interface TeamList {
    id: number;
    name: string | null;
    departmentId: number;
    departmentName: string | null;
    memberCount: number;
    leaderId: number | null;
    leaderFirstName: string | null;
    leaderLastName: string | null;
}

export interface TeamDetail {
    id: number;
    name: string | null;
    departmentId: number;
    departmentName: string | null;
    leaderId: number | null;
}

export interface TeamLookup {
    id: number;
    name: string | null;
}
