import Modal from "react-bootstrap/Modal";
import { ROUTES } from "../../constants/routes";
import { useNavigate } from "react-router-dom";
import useAuth from "../../hooks/useAuth";
import { PermissionName } from "../../constants/permissions";
import { hasPermission } from "../../utils/hasPermission";

type AddInvitationModalProps = {
    open: boolean;
    onClose: () => void;
};

export default function AddMemberModal({ open, onClose }: AddInvitationModalProps) {
    const navigate = useNavigate();
    const { permissions } = useAuth();

    return (
        <Modal show={open} onHide={onClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Кого пригласить?</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <div className="d-flex flex-column gap-3">
                    <button
                        className="btn btn-primary py-2 fw-semibold rounded-3"
                        type="button"
                        onClick={() => {
                            onClose();
                            navigate(ROUTES.INVITATIONS.CREATE_CANDIDATE);
                        }}
                    >
                        Кандидата
                    </button>
                    {hasPermission(permissions, PermissionName.AccountCreationTokenCreateHr) && (
                        <button
                            className="btn btn-outline-primary py-2 fw-semibold rounded-3"
                            type="button"
                            onClick={() => {
                                onClose();
                                navigate(ROUTES.INVITATIONS.CREATE_HR);
                            }}
                        >
                            HR
                        </button>
                    )}
                </div>
            </Modal.Body>
        </Modal>
    );
}
