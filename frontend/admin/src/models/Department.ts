import type { TeamDetail } from "./Teams";

export interface DepartmentList {
    id: number;
    name: string | null;
    headFirstName: string | null;
    headLastName: string | null;
    isVideoGreetingUploaded: boolean;
    teamCount: number;
}

export interface DepartmentDetail {
    id: number;
    name: string | null;
    headFirstName: string | null;
    headLastName: string | null;
    headVideoUrl: string | null;
    headMicrosoftTeamsUrl: string | null;
    teams: TeamDetail[];
}

export interface DepartmentLookup {
    id: number;
    name: string | null;
}
