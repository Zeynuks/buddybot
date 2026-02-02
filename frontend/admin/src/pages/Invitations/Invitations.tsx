import type { InvitationList } from "../../models/Invitation";
import { useState, useEffect } from "react";
import { deleteInvitation, getInvitations } from "../../services/invitationService";
import { PAGE_SIZE } from "../../constants/pagination";
import SearchInput from "../../components/SearchInput/SearchInput";
import InvitationCard from "./InvitationCard";
import EmptyList from "../../components/EmptyList/EmptyList";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import AddInvitationModal from "./AddInvitationModal";
import { useSearchParams } from "react-router-dom";
import DeleteInvitationModal from "./DeleteInvitationModal";
import { Toast, ToastContainer } from "react-bootstrap";

const STATUS_LABELS = [
    { id: "Issued", name: "Выдан" },
    { id: "Activated", name: "Активирован" },
    { id: "Revoked", name: "Отозван" },
    { id: "Expired", name: "Истёк" },
];

export default function Invitations() {
    const [searchParams, setSearchParams] = useSearchParams();
    const [invitations, setInvitations] = useState<InvitationList[]>([]);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [showAddModal, setShowAddModal] = useState(false);
    const [showToast, setShowToast] = useState(false);
    const [toastMessage, setToastMessage] = useState("");
    const [deletingInvitation, setDeletingInvitation] = useState<InvitationList | null>(null);

    const search = searchParams.get("search") || "";
    const selectedStatus = searchParams.get("status") || "";
    const role = searchParams.get("role") || "";
    const page = Number(searchParams.get("page") || 1);
    const [totalCount, setTotalCount] = useState(0);

    useEffect(() => {
        setLoading(true);
        setError(null);

        const pagesToLoad = Array.from({ length: page }, (_, i) => i + 1);

        Promise.all(
            pagesToLoad.map((p) => getInvitations(search, p, PAGE_SIZE, selectedStatus || undefined, role || undefined))
        )
            .then((results) => {
                const allItems = results.flatMap((res) => res.items);
                const lastTotalCount = results.length > 0 ? results[results.length - 1].totalCount : 0;
                setInvitations(allItems);
                setTotalCount(lastTotalCount);
            })
            .catch(() => setError("Не удалось загрузить приглашения"))
            .finally(() => setLoading(false));
    }, [search, page, selectedStatus, role]);

    useEffect(() => {
        setInvitations([]);
        setTotalCount(0);
    }, [search, selectedStatus]);

    const handleSearch = (val: string) => {
        setSearchParams(
            (params) => {
                if (val) params.set("search", val);
                else params.delete("search");
                params.set("page", "1");
                return params;
            },
            { replace: true }
        );
    };

    const handleStatusChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const value = e.target.value;
        setSearchParams(
            (params) => {
                if (value) params.set("status", value);
                else params.delete("status");
                params.set("page", "1");
                return params;
            },
            { replace: true }
        );
    };

    const handleRoleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const value = e.target.value;
        setSearchParams(
            (params) => {
                if (value) params.set("role", value);
                else params.delete("role");
                params.set("page", "1");
                return params;
            },
            { replace: true }
        );
    };

    const handleShowMore = () => {
        setSearchParams((params) => {
            const nextPage = Number(params.get("page") || 1) + 1;
            params.set("page", nextPage.toString());
            return params;
        });
    };

    const noMore = invitations.length >= totalCount;

    const handleDelete = async () => {
        if (!deletingInvitation) return;
        try {
            await deleteInvitation(deletingInvitation.tokenValue);
            setInvitations((prev) => prev.filter((inv) => inv.tokenValue !== deletingInvitation.tokenValue));
            setTotalCount((prev) => prev - 1);
            setToastMessage("Приглашение успешно удалено");
            setShowToast(true);
        } catch (err) {
            if (err instanceof Error) {
                setToastMessage(err.message);
                setShowToast(true);
            } else {
                alert("Не удалось удалить приглашение");
            }
        } finally {
            setDeletingInvitation(null);
        }
    };

    return (
        <div className="mx-4">
            <h5 className="text-center fw-semibold mb-3 mt-2">Приглашения в бота</h5>
            <div className="mb-3">
                <button
                    className="btn btn-primary w-100 fw-medium py-2 mb-3 rounded-pill"
                    onClick={() => setShowAddModal(true)}
                >
                    + Создать ссылку
                </button>
                <AddInvitationModal open={showAddModal} onClose={() => setShowAddModal(false)} />
                <select className="form-select rounded-3 mb-2" value={role} onChange={handleRoleChange}>
                    <option value="">Роль</option>
                    <option value="candidate">Кандидат</option>
                    <option value="hr">HR</option>
                </select>

                <select className="form-select rounded-3 mb-2" value={selectedStatus} onChange={handleStatusChange}>
                    <option value="">Статус</option>
                    {STATUS_LABELS.map((status) => (
                        <option key={status.id} value={status.id}>
                            {status.name}
                        </option>
                    ))}
                </select>
                <SearchInput placeholder="Поиск по имени..." value={search} onChange={handleSearch} />
            </div>
            {loading && invitations.length === 0 ? (
                <div
                    className="d-flex justify-content-center align-items-center mx-auto"
                    style={{
                        maxWidth: 340,
                        minHeight: 240,
                    }}
                >
                    <LoadingSpinner />
                </div>
            ) : error ? (
                <div className="alert alert-danger">{error}</div>
            ) : (
                <div className="d-flex flex-column gap-3">
                    {invitations.length > 0 ? (
                        <>
                            {invitations.map((inv) => (
                                <InvitationCard
                                    key={inv.tokenValue}
                                    invitation={inv}
                                    onDelete={setDeletingInvitation}
                                />
                            ))}
                            {!noMore && (
                                <button
                                    className="btn btn-outline-primary mt-3 mb-2 align-self-center px-5"
                                    disabled={loading}
                                    onClick={handleShowMore}
                                >
                                    {loading ? "Загрузка..." : "Показать ещё"}
                                </button>
                            )}
                        </>
                    ) : (
                        <EmptyList
                            title="Здесь пока пусто"
                            subtitle="Нажмите «Создать ссылку», чтобы выдать новое приглашение."
                        />
                    )}
                </div>
            )}
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
        </div>
    );
}
