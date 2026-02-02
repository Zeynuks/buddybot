import type { DepartmentLookup } from "./Department";
import type { TeamLookup } from "./Teams";
import type { UserLookup } from "./User";

export type OnboardingAccessRequestStatus = "Pending" | "Scheduled" | "Granted" | "Denied";

export interface OnboardingAccessRequest {
    candidateId: number;
    firstName: string;
    lastName: string;
    department: DepartmentLookup | null;
    team: TeamLookup | null;
    hRs: UserLookup[];
    mentors: UserLookup[];
    requestOutcome: OnboardingAccessRequestStatus;
    createdAt: string;
    onboardingAccessTime?: string | null;
}
