import Modal from "react-bootstrap/Modal";
import type { InvitationDetail } from "../../models/Invitation";

type Props = {
    open: boolean;
    invitation: InvitationDetail | null;
    onClose: () => void;
    onRevoke: () => void;
};

export default function RevokeInvitationModal({ open, invitation, onClose, onRevoke }: Props) {
    if (!invitation) return null;

    return (
        <Modal show={open} onHide={onClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Отозвать приглашение?</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <p className="fw-semibold text-center mb-3">
                    Вы уверены, что хотите отозвать приглашение для «{invitation.user.firstName}{" "}
                    {invitation.user.lastName}»?
                    <br />
                    <span className="text-secondary small">Это действие нельзя отменить.</span>
                </p>
                <div className="alert alert-light border d-flex align-items-center gap-2 small">
                    <i className="bi bi-exclamation-circle fs-5 text-secondary"></i>
                    После отзыва, приглашение больше нельзя будет использовать.
                </div>
                <div className="d-flex gap-2 mt-4">
                    <button className="btn btn-danger fw-semibold flex-fill py-2" onClick={onRevoke}>
                        Отозвать
                    </button>
                    <button className="btn btn-light fw-semibold flex-fill py-2" onClick={onClose}>
                        Отмена
                    </button>
                </div>
            </Modal.Body>
        </Modal>
    );
}
