import Modal from "react-bootstrap/Modal";
import type { OnboardingAccessRequest } from "../../models/OnboardingAccessRequests";
import dayjs from "dayjs";
import "dayjs/locale/ru";

type RejectRequestModalProps = {
    open: boolean;
    onClose: () => void;
    request: OnboardingAccessRequest | null;
    onConfirm: () => void;
};

export default function RejectRequestModal({ open, onClose, request, onConfirm }: RejectRequestModalProps) {
    if (!request) return null;

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

    return (
        <Modal show={open} onHide={onClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Отклонить заявку?</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <div className="fw-semibold text-center mb-3">
                    Вы уверены, что хотите отклонить заявку? <br />
                    <span className="text-secondary small">Это действие нельзя отменить.</span>
                </div>
                <div className="fw-semibold mb-1 d-flex align-items-center">
                    {request.firstName} {request.lastName}
                </div>
                <div className="small text-secondary mb-1">
                    <i className="bi bi-diagram-3 me-1"></i>
                    {departmentName}
                    <i className="bi bi-people ms-2 me-1"></i>
                    {teamName}
                </div>
                <div className="small text-secondary mb-1">
                    <i className="bi bi-person-badge me-1"></i>
                    HR: {hrName}
                </div>
                <div className="small text-secondary mb-1">
                    <i className="bi bi-person-check me-1"></i>
                    Наставник: {mentorName}
                </div>
                <div className="small text-secondary mb-2">
                    <i className="bi bi-calendar me-1"></i>
                    Создана: {dayjs(request.createdAt).format("D MMMM YYYY, HH:mm")}
                </div>
                <div className="d-flex gap-2 mt-4">
                    <button className="btn btn-danger fw-semibold flex-fill py-2" type="button" onClick={onConfirm}>
                        Отклонить
                    </button>
                    <button className="btn btn-light fw-semibold flex-fill py-2" type="button" onClick={onClose}>
                        Отмена
                    </button>
                </div>
            </Modal.Body>
        </Modal>
    );
}
