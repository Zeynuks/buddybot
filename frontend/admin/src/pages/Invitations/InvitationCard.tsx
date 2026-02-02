import { Link } from "react-router-dom";
import { ROUTES } from "../../constants/routes";
import type { InvitationList } from "../../models/Invitation";
import dayjs from "dayjs";
import "dayjs/locale/ru";
import useAuth from "../../hooks/useAuth";
import { hasPermission } from "../../utils/hasPermission";
import { PermissionName } from "../../constants/permissions";

const STATUS_LABELS: Record<InvitationList["status"], { label: string; className: string }> = {
    Issued: { label: "Выдан", className: "bg-primary bg-opacity-10 text-primary" },
    Activated: { label: "Активирован", className: "bg-success bg-opacity-25 text-success" },
    Revoked: { label: "Отозван", className: "bg-danger bg-opacity-25 text-danger" },
    Expired: { label: "Истёк", className: "bg-secondary bg-opacity-10 text-secondary" },
};

const ROLE_LABELS: Record<InvitationList["userRole"], { label: string; className: string }> = {
    hr: { label: "HR", className: "bg-primary bg-opacity-10 text-primary me-2" },
    candidate: { label: "Кандидат", className: "bg-info bg-opacity-10 text-info me-2" },
    unknown: { label: "Unknown", className: "bg-secondary bg-opacity-10 text-secondary me-2" },
};

function formatDate(dateStr?: string | null) {
    if (!dateStr) return "-";
    return dayjs(dateStr).format("D MMMM YYYY, HH:mm");
}

export default function InvitationCard({
    invitation,
    onDelete,
}: {
    invitation: InvitationList;
    onDelete: (invitation: InvitationList) => void;
}) {
    const { permissions } = useAuth();
    const status = STATUS_LABELS[invitation.status];
    const userRole = ROLE_LABELS[invitation.userRole];
    const userFullName = `${invitation.userFirstName ?? ""} ${invitation.userLastName ?? ""}`.trim();
    const hr =
        invitation.userRole === "candidate" && invitation.hRs && invitation.hRs.length > 0
            ? `${invitation.hRs[0].firstName ?? ""} ${invitation.hRs[0].lastName ?? ""}`.trim()
            : null;

    const canEdit = invitation.status === "Issued";
    const canDelete =
        ["Activated", "Revoked", "Expired"].includes(invitation.status) &&
        hasPermission(permissions, PermissionName.AccountCreationTokenDelete);

    return (
        <div className="card border-0 shadow-sm rounded-4 px-3 py-3 position-relative">
            {canEdit && (
                <div className="position-absolute top-0 end-0 m-2">
                    <Link
                        to={ROUTES.INVITATIONS.EDIT(invitation.tokenValue)}
                        className="btn btn-link text-primary p-0"
                        title="Редактировать"
                    >
                        <i className="bi bi-pencil-square fs-5"></i>
                    </Link>
                </div>
            )}
            {canDelete && (
                <div className="position-absolute top-0 end-0 m-2">
                    <button
                        className="btn btn-link text-danger p-0"
                        title="Удалить приглашение"
                        onClick={() => onDelete(invitation)}
                    >
                        <i className="bi bi-trash fs-5"></i>
                    </button>
                </div>
            )}
            <div className="d-flex align-items-center mb-1">
                <span className="fw-semibold">{userFullName}</span>
                <span className={`badge fw-normal ms-2 ${userRole.className}`}>{userRole.label}</span>
            </div>
            <div className="d-flex justify-content-between align-items-center mb-2">
                <span className="small text-secondary">Статус:</span>
                <span className={`badge fw-normal ${status.className}`}>{status.label}</span>
            </div>
            <div className="mb-2">
                {invitation.status === "Issued" ? (
                    <>
                        <div className="d-flex justify-content-between align-items-center">
                            <span className="small text-secondary">Создан:</span>
                            <span className="small text-secondary text-end">{formatDate(invitation.issuedAt)}</span>
                        </div>
                    </>
                ) : invitation.status === "Activated" ? (
                    <>
                        <div className="d-flex justify-content-between align-items-center">
                            <span className="small text-secondary">Активирован:</span>
                            <span className="small text-secondary text-end">{formatDate(invitation.activatedAt)}</span>
                        </div>
                    </>
                ) : null}
                <div className="d-flex justify-content-between align-items-center">
                    <span className="small text-secondary">Истекает:</span>
                    <span className="small text-secondary text-end">
                        {invitation.expireDate ? formatDate(invitation.expireDate) : "-"}
                    </span>
                </div>
                {hr && (
                    <div className="d-flex justify-content-between align-items-center">
                        <span className="small text-secondary">Закреплённый HR:</span>
                        <span className="small text-secondary text-end">{hr}</span>
                    </div>
                )}
            </div>
            {invitation.status === "Revoked" && <div className="small text-secondary mt-1">Досрочно отозван HR</div>}
            {invitation.status === "Expired" && (
                <div className="small text-secondary mt-1">Срок действия токена истёк</div>
            )}
            <div className="text-end mt-2">
                <Link
                    to={ROUTES.INVITATIONS.DETAIL(invitation.tokenValue)}
                    className="small text-primary fw-semibold text-decoration-none"
                >
                    Подробнее <i className="bi bi-chevron-right small align-middle"></i>
                </Link>
            </div>
        </div>
    );
}
