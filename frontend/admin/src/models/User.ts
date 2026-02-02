import type { TeamDetail } from "./Teams";

export interface UserLookup {
    id: number;
    firstName: string | null;
    lastName: string | null;
    roles: string[];
}

export interface UserDetail {
    id: number;
    firstName: string | null;
    lastName: string | null;
    login?: string | null;
    roles: string[];
    telegramId: number | null;
    telegramContact: string | null;
    photoUrl: string | null;
    videoUrl: string | null;
    microsoftTeamsUrl: string | null;
    cityId: number | null;
    cityName: string | null;
    hRs: UserLookup[];
    mentors: UserLookup[];
    team: TeamDetail | null;
    createdAt: string;
    isTeamLeader: boolean;
    activeProcessKind?: string | null;
    isActivated: boolean;
    isOnboardingAccessGranted: boolean;
}
