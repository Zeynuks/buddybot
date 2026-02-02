import { Link } from "react-router-dom";
import { ROUTES } from "../../constants/routes";
import type { UserDetail } from "../../models/User";
import dayjs from "dayjs";
import "dayjs/locale/ru";
dayjs.locale("ru");

type CandidateCardProps = {
    candidate: UserDetail;
    onDelete: () => void;
};

const PROCESS_KIND_LABELS: {
    [key: string]: { label: string; className: string };
} = {
    Preboarding: { label: "Пребординг", className: "bg-warning bg-opacity-25 text-warning" },
    Onboarding: { label: "Онбординг", className: "bg-success bg-opacity-10 text-success" },
    PersonalArea: { label: "Личный кабинет", className: "bg-info bg-opacity-10 text-info" },
};

export default function CandidateCard({ candidate, onDelete }: CandidateCardProps) {
    const kind = candidate.activeProcessKind;
    const processBadge = kind && PROCESS_KIND_LABELS[kind];

    return (
        <div className="card border-0 shadow-sm rounded-4 px-3 py-3 position-relative">
            <div className="position-absolute top-0 end-0 m-2 d-flex align-items-center gap-4">
                <Link
                    to={ROUTES.CANDIDATES.EDIT(candidate.id)}
                    className="btn btn-link text-primary p-0"
                    title="Редактировать"
                    tabIndex={0}
                >
                    <i className="bi bi-pencil-square fs-5"></i>
                </Link>
                <button className="btn btn-link text-danger p-0" title="Удалить" tabIndex={0} onClick={onDelete}>
                    <i className="bi bi-trash fs-5"></i>
                </button>
            </div>
            <div className="fw-semibold mb-1">
                {candidate.firstName} {candidate.lastName}
            </div>
            <div className="small text-secondary mb-1">
                <span className="me-2">
                    <i className="bi bi-diagram-3 me-1"></i>
                    {candidate.team?.departmentName || "—"}
                </span>
                <span className="me-2">
                    <i className="bi bi-people me-1"></i>
                    {candidate.team?.name || "—"}
                </span>
            </div>
            <div className="small text-secondary mb-1">
                <i className="bi bi-person-badge me-1"></i>
                HR:{" "}
                {candidate.hRs && candidate.hRs.length > 0
                    ? [candidate.hRs[0].firstName, candidate.hRs[0].lastName].filter(Boolean).join(" ") || "—"
                    : "—"}
            </div>
            <div className="small text-secondary">
                <i className="bi bi-person-check me-1"></i>
                Наставник:{" "}
                {candidate.mentors && candidate.mentors.length > 0
                    ? [candidate.mentors[0].firstName, candidate.mentors[0].lastName].filter(Boolean).join(" ") || "—"
                    : "—"}
            </div>
            <div>
                {processBadge && (
                    <span className={`badge fw-normal mb-1 ${processBadge.className}`}>{processBadge.label}</span>
                )}
            </div>
            <div className="position-absolute end-0 bottom-0 mb-2 me-3 small text-secondary d-flex align-items-center gap-1">
                <i className="bi bi-calendar2 me-1"></i>
                Создан: {dayjs(candidate.createdAt).format("D MMMM YYYY")}
            </div>
        </div>
    );
}
