import Modal from "react-bootstrap/Modal";
import type { TeamList } from "../../models/Teams";

type Props = {
    open: boolean;
    team: TeamList | null;
    onClose: () => void;
    onDelete: () => void;
};

export default function DeleteTeamModal({ open, team, onClose, onDelete }: Props) {
    if (!team) return null;

    const memberCount = team.memberCount ?? 0;
    const hasMembers = memberCount > 0;

    return (
        <Modal show={open} onHide={onClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Удалить команду?</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <p className="fw-semibold text-center mb-3">
                    Вы уверены, что хотите удалить команду «{team.name}»?
                    <br />
                    <span className="text-secondary small">Это действие нельзя отменить.</span>
                </p>

                <div className="alert alert-light border d-flex align-items-center gap-2 small">
                    <i className="bi bi-people-fill fs-5 text-secondary"></i>В этой команде:{" "}
                    <strong>{memberCount}</strong> участник{memberCount === 1 ? "" : "а"}.
                </div>

                {hasMembers && (
                    <div className="alert alert-warning small d-flex align-items-start gap-2 mt-2">
                        <i className="bi bi-exclamation-triangle-fill mt-1"></i>
                        Удаление невозможно, пока в команде есть участники.
                        <br />
                        Пожалуйста, сначала переместите всех участников.
                    </div>
                )}

                <div className="d-flex gap-2 mt-4">
                    <button
                        className="btn btn-danger fw-semibold flex-fill py-2"
                        onClick={onDelete}
                        disabled={hasMembers}
                    >
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
