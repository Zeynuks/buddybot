import Modal from "react-bootstrap/Modal";
import type { OnboardingAccessRequest } from "../../models/OnboardingAccessRequests";

type Props = {
    open: boolean;
    request: OnboardingAccessRequest | null;
    onClose: () => void;
    onDelete: () => void;
};

export default function DeleteOnboardingRequestModal({ open, request, onClose, onDelete }: Props) {
    if (!request) return null;

    return (
        <Modal show={open} onHide={onClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Удалить заявку?</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <p className="fw-semibold text-center mb-3">
                    Вы уверены, что хотите удалить заявку «{request.firstName} {request.lastName}»?
                    <br />
                    <span className="text-secondary small">Это действие нельзя отменить.</span>
                </p>
                <div className="d-flex gap-2 mt-4">
                    <button className="btn btn-danger flex-fill" onClick={onDelete}>
                        Удалить
                    </button>
                    <button className="btn btn-light flex-fill" onClick={onClose}>
                        Отмена
                    </button>
                </div>
            </Modal.Body>
        </Modal>
    );
}
