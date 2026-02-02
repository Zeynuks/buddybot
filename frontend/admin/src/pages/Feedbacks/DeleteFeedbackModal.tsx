import Modal from "react-bootstrap/Modal";
import type { Feedback } from "../../models/Feedbacks";
import dayjs from "dayjs";
import "dayjs/locale/ru";
dayjs.locale("ru");

const STAGE_BADGE: Record<Feedback["processKind"], string> = {
    Onboarding: "badge bg-success bg-opacity-25 text-success fw-normal",
    Preboarding: "badge bg-warning bg-opacity-25 text-warning fw-normal",
};

const STAGE_LABEL: Record<Feedback["processKind"], string> = {
    Onboarding: "Онбординг",
    Preboarding: "Пребординг",
};

type DeleteFeedbackModalProps = {
    open: boolean;
    feedback: Feedback | null;
    onClose: () => void;
    onDelete: () => void;
};

export default function DeleteFeedbackModal({ open, onClose, feedback, onDelete }: DeleteFeedbackModalProps) {
    if (!feedback) return null;

    const hr = feedback.hrNames && feedback.hrNames.length > 0 ? feedback.hrNames[0] : "—";
    const mentor = feedback.mentorNames && feedback.mentorNames.length > 0 ? feedback.mentorNames[0] : "—";

    return (
        <Modal show={open} onHide={onClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Удалить отзыв?</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <div className="fw-semibold text-center mb-3">
                    Вы уверены, что хотите удалить этот отзыв? <br />
                    <span className="text-secondary small">Это действие нельзя отменить.</span>
                </div>
                <div className="fw-semibold mb-1 d-flex align-items-center">
                    {feedback.firstName} {feedback.lastName}
                    <span className={STAGE_BADGE[feedback.processKind] + " ms-2"}>
                        {STAGE_LABEL[feedback.processKind]}
                    </span>
                </div>
                <div className="small text-secondary mb-1">
                    <i className="bi bi-diagram-3 me-1"></i>
                    {feedback.departmentName || "—"}
                    <i className="bi bi-people ms-2 me-1"></i>
                    {feedback.teamName || "—"}
                </div>
                <div className="small text-secondary mb-1">
                    <i className="bi bi-person-badge me-1"></i>
                    HR: {hr}
                </div>
                <div className="small text-secondary mb-1">
                    <i className="bi bi-person-check me-1"></i>
                    Наставник: {mentor}
                </div>
                <div className="small text-secondary mb-1">
                    <i className="bi bi-calendar me-1"></i>
                    Создан: {dayjs(feedback.createdAt).format("D MMMM YYYY, HH:mm")}
                </div>
                <div className="mb-4"></div>
                <div className="mb-2">
                    <span className="fw-semibold">Оценка:</span>{" "}
                    {Array.from({ length: 5 }, (_, i) => (
                        <span key={i} className={i < feedback.rating ? "text-warning" : "text-secondary"}>
                            ★
                        </span>
                    ))}
                </div>
                <div>
                    <span className="fw-semibold">Комментарий:</span>{" "}
                    {feedback.comment && feedback.comment.trim() ? (
                        feedback.comment
                    ) : (
                        <span className="text-secondary fst-italic">Комментарий не указан</span>
                    )}
                </div>
                <div className="d-flex gap-2 mt-4">
                    <button className="btn btn-danger fw-semibold flex-fill py-2" type="button" onClick={onDelete}>
                        Удалить
                    </button>
                    <button className="btn btn-light fw-semibold flex-fill py-2" type="button" onClick={onClose}>
                        Отмена
                    </button>
                </div>
            </Modal.Body>
        </Modal>
    );
}
