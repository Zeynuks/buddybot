import Modal from "react-bootstrap/Modal";
import type { UserDetail } from "../../models/User";

type Props = {
    open: boolean;
    member: UserDetail | null;
    onClose: () => void;
    onDelete: () => void;
};

export default function DeleteMemberModal({ open, member, onClose, onDelete }: Props) {
    if (!member) return null;

    return (
        <Modal show={open} onHide={onClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Удалить участника?</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <p className="fw-semibold text-center mb-3">
                    Вы уверены, что хотите удалить участника <br />
                    <strong>
                        «{member.firstName} {member.lastName}»
                    </strong>
                    ?
                    <br />
                    <span className="text-secondary small">Это действие нельзя отменить.</span>
                </p>

                <div className="alert alert-light border d-flex align-items-center gap-2 small">
                    <i className="bi bi-exclamation-circle fs-5 text-secondary"></i>
                    Этот участник не активировал свой Telegram-аккаунт.
                </div>

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
