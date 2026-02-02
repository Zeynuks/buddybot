import { useParams, useNavigate } from "react-router-dom";
import { useEffect, useState, useCallback } from "react";
import { deleteInvitation, getInvitationByToken, revokeInvitation } from "../../services/invitationService";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import type { InvitationDetail } from "../../models/Invitation";
import InvitationDetails from "./InvitationDetails";
import { Toast, ToastContainer } from "react-bootstrap";
import { useToastFromLocation } from "../../hooks/useToastFromLocation";
import DeleteInvitationModal from "./DeleteInvitationModal";
import RevokeInvitationModal from "./RevokeInvitationModal";

export default function InvitationDetailsPage() {
    const { token } = useParams<{ token: string }>();
    const navigate = useNavigate();

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [invitation, setInvitation] = useState<InvitationDetail | null>(null);

    const [showToast, setShowToast] = useState(false);
    const [toastMessage, setToastMessage] = useState("");
    useToastFromLocation(setToastMessage, setShowToast);

    const [revokingInvitation, setRevokingInvitation] = useState<InvitationDetail | null>(null);
    const [deletingInvitation, setDeletingInvitation] = useState<InvitationDetail | null>(null);

    const loadInvitation = useCallback(async () => {
        if (!token) return;
        setLoading(true);
        setError(null);
        try {
            const data = await getInvitationByToken(token);
            setInvitation(data);
        } catch {
            setError("Не удалось загрузить приглашение");
        } finally {
            setLoading(false);
        }
    }, [token]);

    useEffect(() => {
        loadInvitation();
    }, [loadInvitation]);

    const handleEdit = () => {
        navigate(`/invitations/${token}/edit`);
    };

    const handleRevoke = async () => {
        if (!revokingInvitation || !token) return;
        setLoading(true);
        try {
            await revokeInvitation(token);
            await loadInvitation();
            setRevokingInvitation(null);
        } catch {
            setError("Не удалось отозвать приглашение");
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async () => {
        if (!token) return;
        setLoading(true);
        try {
            await deleteInvitation(token);
            navigate("/invitations");
        } catch {
            setError("Не удалось удалить приглашение");
        } finally {
            setLoading(false);
        }
    };

    if (loading) return <LoadingSpinner />;
    if (error || !invitation)
        return (
            <div className="alert alert-danger mt-4 text-center">
                {error || "Не удалось загрузить приглашение"}
                <button className="btn btn-link mt-2" onClick={() => navigate(-1)}>
                    Назад
                </button>
            </div>
        );

    return (
        <>
            <InvitationDetails
                invitation={invitation}
                onEdit={handleEdit}
                onRevoke={setRevokingInvitation}
                onDelete={setDeletingInvitation}
                onBack={() => navigate(-1)}
            />
            <RevokeInvitationModal
                open={!!revokingInvitation}
                invitation={revokingInvitation}
                onClose={() => setRevokingInvitation(null)}
                onRevoke={handleRevoke}
            />

            <DeleteInvitationModal
                open={!!deletingInvitation}
                invitation={deletingInvitation}
                onClose={() => setDeletingInvitation(null)}
                onDelete={handleDelete}
            />

            <ToastContainer className="position-fixed top-0 start-50 translate-middle-x p-3" style={{ zIndex: 1080 }}>
                <Toast show={showToast} onClose={() => setShowToast(false)} bg="success" delay={2200} autohide>
                    <Toast.Body className="text-white d-flex align-items-center gap-2">
                        <i className="bi bi-check-circle-fill fs-5"></i>
                        <span>{toastMessage}</span>
                    </Toast.Body>
                </Toast>
            </ToastContainer>
        </>
    );
}
