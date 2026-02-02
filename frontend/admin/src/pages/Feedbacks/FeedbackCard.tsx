import type { Feedback, ProcessKind } from "../../models/Feedbacks";
import dayjs from "dayjs";
import "dayjs/locale/ru";
import useAuth from "../../hooks/useAuth";
import { PermissionName } from "../../constants/permissions";
import { hasPermission } from "../../utils/hasPermission";
dayjs.locale("ru");

const STAGE_BADGE: Record<ProcessKind, string> = {
    Onboarding: "badge bg-success bg-opacity-25 text-success fw-normal",
    Preboarding: "badge bg-warning bg-opacity-25 text-warning fw-normal",
};

const STAGE_LABEL: Record<ProcessKind, string> = {
    Onboarding: "Онбординг",
    Preboarding: "Пребординг",
};

type FeedbackCardProps = {
    feedback: Feedback;
    onDelete: () => void;
};

export default function FeedbackCard({ feedback, onDelete }: FeedbackCardProps) {
    const { permissions } = useAuth();

    const stars = Array.from({ length: 5 }, (_, i) => (
        <span key={i} className={i < feedback.rating ? "text-warning" : "text-secondary"}>
            ★
        </span>
    ));

    const hrName = feedback.hrNames?.[0] || "—";
    const mentorName = feedback.mentorNames?.[0] || "—";

    return (
        <div className="card border-0 shadow-sm rounded-4 px-3 py-3 position-relative">
            {hasPermission(permissions, PermissionName.FeedbackDelete) && (
                <button
                    className="btn btn-link text-danger p-0 position-absolute top-0 end-0 m-2"
                    title="Удалить"
                    tabIndex={0}
                    onClick={onDelete}
                >
                    <i className="bi bi-trash fs-5"></i>
                </button>
            )}
            <div className="fw-semibold mb-1">
                {feedback.firstName} {feedback.lastName}
            </div>
            <div className="small text-secondary mb-1">
                <span className="me-2">
                    <i className="bi bi-diagram-3 me-1"></i>
                    {feedback.departmentName || "—"}
                </span>
                <span className="me-2">
                    <i className="bi bi-people me-1"></i>
                    {feedback.teamName || "—"}
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
            <div className="mb-2 fs-5">{stars}</div>
            <div className="mb-2" style={{ whiteSpace: "pre-wrap", wordBreak: "break-word" }}>
                {feedback.comment && feedback.comment.trim() ? (
                    feedback.comment
                ) : (
                    <span className="text-secondary fst-italic">Комментарий не указан</span>
                )}
            </div>
            <div className="d-flex justify-content-between align-items-center mt-2">
                <span className={STAGE_BADGE[feedback.processKind]}>{STAGE_LABEL[feedback.processKind]}</span>
                <div className="position-absolute end-0 bottom-0 mb-2 me-3 small text-secondary d-flex align-items-center gap-1">
                    <i className="bi bi-calendar2 me-1"></i>
                    Создан: {dayjs(feedback.createdAt).format("D MMMM YYYY")}
                </div>
            </div>
        </div>
    );
}
