import Modal from "react-bootstrap/Modal";
import type { InvitationDetail, InvitationList } from "../../models/Invitation";

type Props = {
    open: boolean;
    invitation: InvitationList | InvitationDetail | null;
    onClose: () => void;
    onDelete: () => void;
};

export default function DeleteInvitationModal({ open, invitation, onClose, onDelete }: Props) {
    if (!invitation) return null;

    return (
        <Modal show={open} onHide={onClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Удалить приглашение?</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <p className="fw-semibold text-center mb-3">
                    Вы уверены, что хотите удалить приглашение для «{invitation.userFirstName} {invitation.userLastName}
                    » ?
                    <br />
                    <span className="text-secondary small">Это действие нельзя отменить.</span>
                </p>
                <div className="d-flex gap-2 mt-4">
                    <button className="btn btn-danger fw-semibold flex-fill py-2" onClick={onDelete}>
                        Удалить
                    </button>
                    <button className="btn btn-light fw-semibold flex-fill py-2" onClick={onClose}>
                        Отмена
                    </button>
                </div>
            </Modal.Body>
        </Modal>
    );
}
