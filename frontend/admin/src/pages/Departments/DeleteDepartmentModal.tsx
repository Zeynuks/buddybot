import Modal from "react-bootstrap/Modal";
import type { DepartmentList } from "../../models/Department";

type Props = {
    open: boolean;
    department: DepartmentList | null;
    onClose: () => void;
    onDelete: () => void;
};

function pluralize(n: number) {
    if (n % 10 === 1 && n % 100 !== 11) return "команда";
    if ([2, 3, 4].includes(n % 10) && ![12, 13, 14].includes(n % 100)) return "команды";
    return "команд";
}

export default function DeleteDepartmentModal({ open, department, onClose, onDelete }: Props) {
    if (!department) return null;

    const teamCount = department.teamCount ?? 0;
    const hasTeams = teamCount > 0;

    return (
        <Modal show={open} onHide={onClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Удалить отдел?</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <p className="fw-semibold text-center mb-3">
                    Вы уверены, что хотите удалить отдел «{department.name}»?
                    <br />
                    <span className="text-secondary small">Это действие нельзя отменить.</span>
                </p>

                <div className="alert alert-light border d-flex align-items-center gap-2 small">
                    <i className="bi bi-people-fill fs-5 text-secondary"></i>В этом отделе: <strong>{teamCount}</strong>{" "}
                    {pluralize(teamCount)}.
                </div>

                {hasTeams && (
                    <div className="alert alert-warning small d-flex align-items-start gap-2 mt-2">
                        <i className="bi bi-exclamation-triangle-fill mt-1"></i>
                        Удаление невозможно, пока в отделе есть хотя бы одна команда.
                        <br />
                        Пожалуйста, сначала удалите или переместите все команды.
                    </div>
                )}

                <div className="d-flex gap-2 mt-4">
                    <button
                        className="btn btn-danger fw-semibold flex-fill py-2"
                        onClick={onDelete}
                        disabled={hasTeams}
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
