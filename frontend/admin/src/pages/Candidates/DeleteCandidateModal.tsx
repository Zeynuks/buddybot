import Modal from "react-bootstrap/Modal";
import type { UserDetail } from "../../models/User";

type DeleteCandidateModalProps = {
    open: boolean;
    onClose: () => void;
    candidate: UserDetail | null;
    onDelete: () => void;
};

const PROCESS_KIND_LABELS: Record<
    NonNullable<UserDetail["activeProcessKind"]>,
    { label: string; className: string }
> = {
    Preboarding: { label: "Пребординг", className: "badge bg-warning bg-opacity-25 text-warning fw-normal" },
    Onboarding: { label: "Онбординг", className: "badge bg-success bg-opacity-25 text-success fw-normal" },
    PersonalArea: { label: "Личный кабинет", className: "badge bg-info bg-opacity-25 text-info fw-normal" },
};

export default function DeleteCandidateModal({ open, onClose, candidate, onDelete }: DeleteCandidateModalProps) {
    if (!candidate) return null;

    const kind = candidate.activeProcessKind;
    const kindData = kind && PROCESS_KIND_LABELS[kind];

    return (
        <Modal show={open} onHide={onClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Удалить кандидата?</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <div className="fw-semibold text-center mb-3">
                    Вы уверены что хотите удалить этого кандидата? <br />
                    <span className="text-secondary small">Это действие нельзя отменить.</span>
                </div>
                <div className="fw-semibold mb-1 d-flex align-items-center">
                    {candidate.firstName} {candidate.lastName}
                    {kindData && <span className={`${kindData.className} ms-2`}>{kindData.label}</span>}
                </div>
                <div className="small text-secondary mb-1">
                    <i className="bi bi-diagram-3 me-1"></i>
                    {candidate.team?.departmentName || "—"}
                    <i className="bi bi-people ms-2 me-1"></i>
                    {candidate.team?.name || "—"}
                </div>
                <div className="small text-secondary mb-1">
                    <i className="bi bi-person-badge me-1"></i>
                    HR: {candidate.hRs?.[0] ? `${candidate.hRs[0].firstName} ${candidate.hRs[0].lastName}` : "—"}
                </div>
                <div className="small text-secondary mb-1">
                    <i className="bi bi-person-check me-1"></i>
                    Наставник:{" "}
                    {candidate.mentors?.[0]
                        ? `${candidate.mentors[0].firstName} ${candidate.mentors[0].lastName}`
                        : "—"}
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
