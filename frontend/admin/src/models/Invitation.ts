import type { DepartmentLookup } from "./Department";
import type { TeamLookup } from "./Teams";
import type { UserDetail, UserLookup } from "./User";

type InvitationStatus = "Issued" | "Activated" | "Revoked" | "Expired";
type UserRole = "candidate" | "hr" | "unknown";

export type InvitationList = {
    tokenValue: string;
    userFirstName: string;
    userLastName: string;
    status: InvitationStatus;
    issuedAt: string;
    expireDate?: string | null;
    activatedAt?: string | null;
    userRole: UserRole;
    hRs: UserLookup[];
};

export type InvitationDetail = {
    tokenValue: string;
    userFirstName: string;
    userLastName: string;
    inviteLink: string;
    qrCodeBase64: string;
    status: InvitationStatus;
    issuedAt: string;
    expireDate?: string | null;
    activatedAt?: string | null;
    creator: UserLookup;
    userRole: UserRole;
    user: UserDetail;
    department?: DepartmentLookup;
    team?: TeamLookup;
    hRs: UserLookup[];
    mentors: UserLookup[];
};
