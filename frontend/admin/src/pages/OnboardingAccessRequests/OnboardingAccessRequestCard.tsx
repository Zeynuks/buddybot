import type { OnboardingAccessRequest, OnboardingAccessRequestStatus } from "../../models/OnboardingAccessRequests";
import dayjs from "dayjs";
import "dayjs/locale/ru";
import { Link } from "react-router-dom";
import { ROUTES } from "../../constants/routes";

const STATUS_LABELS: Record<OnboardingAccessRequestStatus, { label: string; className: string }> = {
    Pending: { label: "В ожидании", className: "bg-primary bg-opacity-10 text-primary" },
    Scheduled: { label: "Запланировано", className: "bg-warning bg-opacity-25 text-warning" },
    Granted: { label: "Принят", className: "bg-success bg-opacity-25 text-success" },
    Denied: { label: "Отклонён", className: "bg-danger bg-opacity-10 text-danger" },
};

type OnboardingAccessRequestCardProps = {
    request: OnboardingAccessRequest;
    onConfirm: () => void;
    onReject: () => void;
    onDelete?: () => void;
    onRestore?: () => void;
};

export default function OnboardingAccessRequestCard({
    request,
    onConfirm,
    onReject,
    onDelete,
    onRestore,
}: OnboardingAccessRequestCardProps) {
    const status = STATUS_LABELS[request.requestOutcome];
    const hrName =
        Array.isArray(request.hRs) && request.hRs.length
            ? `${request.hRs[0].firstName ?? ""} ${request.hRs[0].lastName ?? ""}`.trim()
            : "—";
    const mentorName =
        Array.isArray(request.mentors) && request.mentors.length
            ? `${request.mentors[0].firstName ?? ""} ${request.mentors[0].lastName ?? ""}`.trim()
            : "—";
    const departmentName = request.department?.name ?? "—";
    const teamName = request.team?.name ?? "—";

    let actions = null;
    if (request.requestOutcome === "Scheduled") {
        actions = (
            <div className="position-absolute top-0 end-0 m-2">
                <Link
                    to={ROUTES.ONBOARDING_ACCESS_REQUESTS.EDIT(request.candidateId)}
                    className="btn btn-link text-primary p-0"
                    title="Редактировать"
                    tabIndex={0}
                >
                    <i className="bi bi-pencil-square fs-5"></i>
                </Link>
            </div>
        );
    }
    if (request.requestOutcome === "Denied") {
        actions = (
            <div className="position-absolute top-0 end-0 m-2">
                <button className="btn btn-link text-danger p-0" title="Удалить" tabIndex={0} onClick={onDelete}>
                    <i className="bi bi-trash fs-5"></i>
                </button>
            </div>
        );
    }

    return (
        <div className="card border-0 shadow-sm rounded-4 px-3 py-3 position-relative">
            {actions}
            <div className="fw-semibold mb-1">
                {request.firstName} {request.lastName}
                <span className={`badge ms-2 fw-normal ${status.className}`}>{status.label}</span>
            </div>
            <div className="small text-secondary mb-1">
                <span className="me-2">
                    <i className="bi bi-diagram-3 me-1"></i>
                    {departmentName}
                </span>
                <span className="me-2">
                    <i className="bi bi-people me-1"></i>
                    {teamName}
                </span>
            </div>
            <div className="small text-secondary mb-1">
                <i className="bi bi-person-badge me-1"></i>
                HR: {hrName}
            </div>
            <div className="small text-secondary mb-2">
                <i className="bi bi-person-check me-1"></i>
                Наставник: {mentorName}
            </div>
            {request.requestOutcome === "Pending" && (
                <>
                    <div className="small text-secondary mb-2">
                        <i className="bi bi-calendar me-1"></i>
                        Создана: {dayjs(request.createdAt).format("D MMM YYYY HH:mm")}
                    </div>
                    <div className="d-flex gap-3 mb-2">
                        <button className="btn btn-success w-100 fw-semibold" onClick={onConfirm}>
                            Принять
                        </button>
                        <button className="btn btn-danger w-100 fw-semibold" onClick={onReject}>
                            Отклонить
                        </button>
                    </div>
                </>
            )}
            {request.requestOutcome === "Scheduled" && (
                <div className="small text-secondary mb-1">
                    <i className="bi bi-calendar me-1"></i>
                    Доступ будет выдан:{" "}
                    {request.onboardingAccessTime
                        ? dayjs(request.onboardingAccessTime).format("D MMM YYYY HH:mm")
                        : "—"}
                </div>
            )}
            {request.requestOutcome === "Granted" && (
                <div className="small text-secondary mb-1">
                    <i className="bi bi-calendar me-1"></i>
                    Доступ выдан:{" "}
                    {request.onboardingAccessTime
                        ? dayjs(request.onboardingAccessTime).format("D MMM YYYY HH:mm")
                        : "—"}
                </div>
            )}
            {request.requestOutcome === "Denied" && (
                <>
                    <div className="small text-secondary mb-2">
                        <i className="bi bi-calendar me-1"></i>
                        Создана: {dayjs(request.createdAt).format("D MMM YYYY HH:mm")}
                    </div>
                    <div className="d-flex gap-3 mb-2">
                        <button className="btn btn-primary w-100 fw-semibold" onClick={onRestore}>
                            Восстановить заявку
                        </button>
                    </div>
                </>
            )}
        </div>
    );
}
