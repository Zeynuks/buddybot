import { useState } from "react";
import type { InvitationDetail } from "../../models/Invitation";
import dayjs from "dayjs";
import { Toast, ToastContainer } from "react-bootstrap";
import "dayjs/locale/ru";
import { Link } from "react-router-dom";
import { ROUTES } from "../../constants/routes";
import useAuth from "../../hooks/useAuth";
import { PermissionName } from "../../constants/permissions";
import { hasPermission } from "../../utils/hasPermission";

type Props = {
    invitation: InvitationDetail;
    onEdit: () => void;
    onRevoke: (invitation: InvitationDetail) => void;
    onDelete: (invitation: InvitationDetail) => void;
    onBack: () => void;
};
function StatusBadge({ status }: { status: InvitationDetail["status"] }) {
    const STATUS_MAP = {
        Issued: { text: "Выдан", className: "text-primary" },
        Activated: { text: "Активирован", className: "text-success" },
        Revoked: { text: "Отозван", className: "text-danger" },
        Expired: { text: "Истёк", className: "text-secondary" },
    };
    const info = STATUS_MAP[status];
    return <span className={`${info.className} fw-semibold`}>{info.text}</span>;
}

function formatDate(dateStr?: string | null) {
    if (!dateStr) return "-";
    return dayjs(dateStr).format("D MMMM YYYY, HH:mm");
}

export default function InvitationDetails({ invitation, onEdit, onRevoke, onDelete, onBack }: Props) {
    const { permissions } = useAuth();
    const isCandidate = invitation.userRole === "candidate";
    const isHR = invitation.userRole === "hr";
    const [showToastLink, setShowToastLink] = useState(false);
    const [showToastQR, setShowToastQR] = useState(false);

    const handleCopyLink = () => {
        navigator.clipboard.writeText(invitation.inviteLink);
        setShowToastLink(true);
        setTimeout(() => setShowToastLink(false), 2500);
    };

    const handleCopyQR = async () => {
        const qrUrl = invitation.qrCodeBase64.startsWith("data:")
            ? invitation.qrCodeBase64
            : `data:image/png;base64,${invitation.qrCodeBase64}`;
        const data = await fetch(qrUrl);
        const blob = await data.blob();
        await navigator.clipboard.write([new window.ClipboardItem({ [blob.type]: blob })]);
        setShowToastQR(true);
        setTimeout(() => setShowToastQR(false), 2500);
    };

    return (
        <div className="mx-4">
            <div className="card border-0 shadow-sm rounded-4 p-4 mb-4 position-relative mx-auto">
                <button
                    className="btn btn-link text-primary px-0 mb-2 d-flex align-items-center text-decoration-none"
                    type="button"
                    onClick={onBack}
                >
                    <i className="bi bi-chevron-left small align-middle"></i>
                    <span className="ms-1">Назад</span>
                </button>
                <div className="text-center fs-4 mb-4 mt-2">Приглашение в бота</div>

                {invitation.status === "Issued" && (
                    <>
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1">Ссылка-приглашение</label>
                            <div className="input-group">
                                <input
                                    type="text"
                                    className="form-control rounded-3"
                                    value={invitation.inviteLink}
                                    disabled
                                    readOnly
                                />
                                <button
                                    className="btn btn-outline-secondary rounded-3 ms-2"
                                    type="button"
                                    title="Скопировать"
                                    onClick={handleCopyLink}
                                >
                                    <i className="bi bi-clipboard"></i>
                                </button>
                            </div>
                        </div>
                        <div className="d-flex flex-column align-items-center mb-3">
                            <div className="position-relative d-inline-block mb-3">
                                <div className="bg-white rounded-4 p-2 border d-inline-block">
                                    <img
                                        src={
                                            invitation.qrCodeBase64.startsWith("data:")
                                                ? invitation.qrCodeBase64
                                                : `data:image/png;base64,${invitation.qrCodeBase64}`
                                        }
                                        alt="QR Code"
                                        width={180}
                                        height={180}
                                    />
                                </div>
                                <button
                                    className="btn btn-outline-secondary rounded-3 position-absolute top-50 start-100 translate-middle-y ms-2"
                                    type="button"
                                    title="Скопировать QR"
                                    onClick={handleCopyQR}
                                >
                                    <i className="bi bi-clipboard"></i>
                                </button>
                            </div>
                        </div>
                    </>
                )}

                <div className="d-flex justify-content-between mb-4 flex-wrap">
                    <div className="small text-start">
                        <div>Статус:</div>
                        <div>Создан:</div>
                        {invitation.status === "Activated" && <div>Активирован:</div>}
                        {invitation.status === "Issued" && <div>Истекает:</div>}
                        {invitation.status === "Expired" && <div>Истёк:</div>}
                        <div>Создал:</div>
                        <div>
                            <span className="fw-semibold">
                                {isCandidate ? "Кандидат:" : isHR ? "HR:" : "Пользователь:"}
                            </span>
                        </div>
                        {isCandidate && <div>Закреплённый HR:</div>}
                        {invitation.department && <div>Отдел:</div>}
                        {invitation.team && <div>Команда:</div>}
                        {isCandidate && <div>Наставник:</div>}
                    </div>
                    <div className="small text-end">
                        <StatusBadge status={invitation.status} />
                        <div>{formatDate(invitation.issuedAt)}</div>
                        {invitation.status === "Activated" && <div>{formatDate(invitation.activatedAt)}</div>}
                        {invitation.status === "Issued" && <div>{formatDate(invitation.expireDate)}</div>}
                        {invitation.status === "Expired" && <div>{formatDate(invitation.expireDate)}</div>}
                        <div>
                            {invitation.creator.firstName} {invitation.creator.lastName}
                        </div>
                        <div>
                            <span className="fw-semibold">
                                {invitation.user.firstName} {invitation.user.lastName}
                            </span>
                        </div>
                        {isCandidate && (
                            <div>{invitation.hRs.map((hr) => `${hr.firstName} ${hr.lastName}`).join(", ") || "-"}</div>
                        )}
                        {invitation.department && <div>{invitation.department.name}</div>}
                        {invitation.team && <div>{invitation.team.name}</div>}
                        {isCandidate && (
                            <div>{invitation.mentors.map((m) => `${m.firstName} ${m.lastName}`).join(", ") || "-"}</div>
                        )}
                    </div>
                </div>

                <div className="d-flex flex-column gap-2 mt-3">
                    {invitation.status === "Issued" && (
                        <>
                            <button className="btn btn-primary w-100 fw-semibold py-2" type="button" onClick={onEdit}>
                                Редактировать приглашение
                            </button>
                            <button
                                className="btn btn-danger w-100 fw-semibold py-2"
                                type="button"
                                onClick={() => onRevoke(invitation)}
                            >
                                Отозвать
                            </button>
                        </>
                    )}
                    {invitation.status === "Activated" && (
                        <>
                            {isCandidate ? (
                                <Link
                                    to={ROUTES.CANDIDATES.EDIT(invitation.user.id)}
                                    className="btn btn-primary w-100 fw-semibold py-2 mb-2"
                                >
                                    Редактировать кандидата
                                </Link>
                            ) : isHR ? (
                                <Link
                                    to={ROUTES.MEMBERS.EDIT(invitation.user.id)}
                                    className="btn btn-primary w-100 fw-semibold py-2 mb-2"
                                >
                                    Редактировать HR
                                </Link>
                            ) : (
                                <button className="btn btn-primary w-100 fw-semibold py-2 mb-2" type="button" disabled>
                                    Редактировать
                                </button>
                            )}
                            {hasPermission(permissions, PermissionName.AccountCreationTokenDelete) && (
                                <button
                                    className="btn btn-danger w-100 fw-semibold py-2"
                                    type="button"
                                    onClick={() => onDelete(invitation)}
                                >
                                    Удалить
                                </button>
                            )}
                        </>
                    )}
                    {(invitation.status === "Revoked" || invitation.status === "Expired") &&
                        hasPermission(permissions, PermissionName.AccountCreationTokenDelete) && (
                            <button
                                className="btn btn-danger w-100 fw-semibold py-2"
                                type="button"
                                onClick={() => onDelete(invitation)}
                            >
                                Удалить
                            </button>
                        )}
                </div>
            </div>
            <ToastContainer className="position-fixed top-0 start-50 translate-middle-x p-3">
                <Toast show={showToastLink} onClose={() => setShowToastLink(false)} bg="success" delay={1500} autohide>
                    <Toast.Body className="text-white d-flex align-items-center gap-2">
                        <i className="bi bi-check-circle-fill fs-5"></i>
                        <span>Ссылка скопирована</span>
                    </Toast.Body>
                </Toast>
                <Toast show={showToastQR} onClose={() => setShowToastQR(false)} bg="success" delay={1500} autohide>
                    <Toast.Body className="text-white d-flex align-items-center gap-2">
                        <i className="bi bi-check-circle-fill fs-5"></i>
                        <span>QR-код скопирован</span>
                    </Toast.Body>
                </Toast>
            </ToastContainer>
        </div>
    );
}
