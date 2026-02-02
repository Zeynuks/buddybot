import { useEffect, useState } from "react";
import OnboardingAccessRequestCard from "./OnboardingAccessRequestCard";
import SearchInput from "../../components/SearchInput/SearchInput";
import EmptyList from "../../components/EmptyList/EmptyList";
import type { OnboardingAccessRequest, OnboardingAccessRequestStatus } from "../../models/OnboardingAccessRequests";
import ConfirmRequestModal from "./ConfirmRequestModal";
import RejectRequestModal from "./RejectRequestModal";
import {
    confirmOnboardingAccessRequest,
    deleteOnboardingAccessRequest,
    getOnboardingAccessRequests,
    rejectOnboardingAccessRequest,
    restoreOnboardingAccessRequest,
} from "../../services/onboardingAccessRequestService";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import { Toast, ToastContainer } from "react-bootstrap";
import { useToastFromLocation } from "../../hooks/useToastFromLocation";
import { PAGE_SIZE } from "../../constants/pagination";
import { useSearchParams } from "react-router-dom";
import DeleteOnboardingRequestModal from "./DeleteOnboardingRequestModal";
import RestoreOnboardingRequestModal from "./RestoreOnboardingRequestModal";

const STATUS_OPTIONS: { id: "" | OnboardingAccessRequestStatus; label: string }[] = [
    { id: "", label: "Статус заявки" },
    { id: "Pending", label: "В ожидании" },
    { id: "Scheduled", label: "Запланировано" },
    { id: "Granted", label: "Принят" },
    { id: "Denied", label: "Отклонён" },
];

export default function OnboardingAccessRequests() {
    const [searchParams, setSearchParams] = useSearchParams();
    const [requests, setRequests] = useState<OnboardingAccessRequest[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const search = searchParams.get("search") || "";
    const selectedStatus = (searchParams.get("status") as OnboardingAccessRequestStatus | "") || "";
    const page = Number(searchParams.get("page") || 1);
    const [totalCount, setTotalCount] = useState(0);

    const [confirmingRequest, setConfirmingRequest] = useState<OnboardingAccessRequest | null>(null);
    const [rejectingRequest, setRejectingRequest] = useState<OnboardingAccessRequest | null>(null);
    const [deletingRequest, setDeletingRequest] = useState<OnboardingAccessRequest | null>(null);
    const [restoringRequest, setRestoringRequest] = useState<OnboardingAccessRequest | null>(null);

    const [showToast, setShowToast] = useState(false);
    const [toastMessage, setToastMessage] = useState("");
    useToastFromLocation(setToastMessage, setShowToast);

    useEffect(() => {
        setLoading(true);
        setError(null);

        const pagesToLoad = Array.from({ length: page }, (_, i) => i + 1);

        Promise.all(
            pagesToLoad.map((p) => getOnboardingAccessRequests(search, selectedStatus || undefined, p, PAGE_SIZE))
        )
            .then((results) => {
                const allItems = results.flatMap((res) => res.items);
                const lastTotalCount = results.length > 0 ? results[results.length - 1].totalCount : 0;
                setRequests(allItems);
                setTotalCount(lastTotalCount);
            })
            .catch(() => setError("Не удалось загрузить заявки"))
            .finally(() => setLoading(false));
    }, [search, selectedStatus, page]);

    useEffect(() => {
        setRequests([]);
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

    const handleShowMore = () => {
        setSearchParams((params) => {
            const nextPage = Number(params.get("page") || 1) + 1;
            params.set("page", nextPage.toString());
            return params;
        });
    };

    const noMore = requests.length >= totalCount;

    const handleConfirm = async (values: { onboardingAccessTimeUtc?: string; teamId: number; mentorIds: number[] }) => {
        if (!confirmingRequest) return;
        try {
            await confirmOnboardingAccessRequest(confirmingRequest.candidateId, values);
            setConfirmingRequest(null);
            setToastMessage("Заявка успешно подтверждена");
            setShowToast(true);
            reloadRequests();
        } catch {
            setToastMessage("Ошибка при подтверждении заявки");
            setShowToast(true);
        }
    };

    const handleReject = async () => {
        if (!rejectingRequest) return;
        try {
            await rejectOnboardingAccessRequest(rejectingRequest.candidateId);
            setRejectingRequest(null);
            setToastMessage("Заявка успешно отклонена");
            setTotalCount((prev) => prev - 1);
            setShowToast(true);
            reloadRequests();
        } catch {
            setToastMessage("Ошибка при отклонении заявки");
            setShowToast(true);
        }
    };

    const handleRestore = async () => {
        if (!restoringRequest) return;
        try {
            await restoreOnboardingAccessRequest(restoringRequest.candidateId);
            setRestoringRequest(null);
            setToastMessage("Заявка успешно восстановлена");
            setShowToast(true);
            reloadRequests();
        } catch {
            setToastMessage("Ошибка при восстановлении заявки");
            setShowToast(true);
        }
    };

    const reloadRequests = () => {
        setLoading(true);
        setError(null);

        const pagesToLoad = Array.from({ length: page }, (_, i) => i + 1);

        Promise.all(
            pagesToLoad.map((p) => getOnboardingAccessRequests(search, selectedStatus || undefined, p, PAGE_SIZE))
        )
            .then((results) => {
                const allItems = results.flatMap((res) => res.items);
                const lastTotalCount = results.length > 0 ? results[results.length - 1].totalCount : 0;
                setRequests(allItems);
                setTotalCount(lastTotalCount);
            })
            .catch(() => setError("Не удалось загрузить заявки"))
            .finally(() => setLoading(false));
    };

    const handleDelete = async () => {
        if (!deletingRequest) return;
        try {
            await deleteOnboardingAccessRequest(deletingRequest.candidateId);
            setToastMessage("Заявка успешно удалена");
            setShowToast(true);
            reloadRequests();
        } catch {
            setToastMessage("Ошибка при удалении заявки");
            setShowToast(true);
        } finally {
            setDeletingRequest(null);
        }
    };

    return (
        <div className="mx-4">
            <h5 className="text-center fw-semibold mb-3 mt-2">Заявки на онбординг</h5>
            <div className="mb-3">
                <select className="form-select rounded-3 mb-2" value={selectedStatus} onChange={handleStatusChange}>
                    {STATUS_OPTIONS.map((opt) => (
                        <option key={opt.id} value={opt.id}>
                            {opt.label}
                        </option>
                    ))}
                </select>
                <SearchInput placeholder="Поиск по имени..." value={search} onChange={handleSearch} />
            </div>
            {loading && requests.length === 0 ? (
                <LoadingSpinner />
            ) : error ? (
                <div className="alert alert-danger">{error}</div>
            ) : (
                <div className="d-flex flex-column gap-3">
                    {requests.length > 0 ? (
                        <>
                            {requests.map((req) => (
                                <OnboardingAccessRequestCard
                                    key={req.candidateId}
                                    request={req}
                                    onConfirm={() => setConfirmingRequest(req)}
                                    onReject={() => setRejectingRequest(req)}
                                    onDelete={() => setDeletingRequest(req)}
                                    onRestore={() => setRestoringRequest(req)}
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
                        <EmptyList title="Здесь пока пусто" subtitle="Ожидайте первой заявки на онбординг." />
                    )}
                </div>
            )}
            {confirmingRequest && (
                <ConfirmRequestModal
                    open={!!confirmingRequest}
                    request={confirmingRequest}
                    onClose={() => setConfirmingRequest(null)}
                    onConfirm={handleConfirm}
                />
            )}
            {rejectingRequest && (
                <RejectRequestModal
                    open={!!rejectingRequest}
                    request={rejectingRequest}
                    onClose={() => setRejectingRequest(null)}
                    onConfirm={handleReject}
                />
            )}
            {deletingRequest && (
                <DeleteOnboardingRequestModal
                    open={!!deletingRequest}
                    request={deletingRequest}
                    onClose={() => setDeletingRequest(null)}
                    onDelete={handleDelete}
                />
            )}
            {restoringRequest && (
                <RestoreOnboardingRequestModal
                    open={!!restoringRequest}
                    request={restoringRequest}
                    onClose={() => setRestoringRequest(null)}
                    onConfirm={handleRestore}
                />
            )}

            <ToastContainer className="position-fixed top-0 start-50 translate-middle-x p-3" style={{ zIndex: 1080 }}>
                <Toast show={showToast} onClose={() => setShowToast(false)} bg="success" delay={2000} autohide>
                    <Toast.Body className="text-white d-flex align-items-center gap-2">
                        <i className="bi bi-check-circle-fill fs-5"></i>
                        <span>{toastMessage}</span>
                    </Toast.Body>
                </Toast>
            </ToastContainer>
        </div>
    );
}
