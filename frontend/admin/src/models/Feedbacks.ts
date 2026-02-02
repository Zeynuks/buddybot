export type ProcessKind = "Preboarding" | "Onboarding";

export interface Feedback {
    id: number;
    firstName: string | null;
    lastName: string | null;
    processKind: ProcessKind;
    rating: number;
    comment: string | null;
    departmentName: string | null;
    teamName: string | null;
    hrNames: string[];
    mentorNames: string[];
    createdAt: string;
}
