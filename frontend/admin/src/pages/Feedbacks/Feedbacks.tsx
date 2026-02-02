import { useEffect, useState } from "react";
import FeedbackCard from "./FeedbackCard";
import SearchInput from "../../components/SearchInput/SearchInput";
import EmptyList from "../../components/EmptyList/EmptyList";
import type { Feedback, ProcessKind } from "../../models/Feedbacks";
import DeleteFeedbackModal from "./DeleteFeedbackModal";
import { deleteFeedback, exportFeedbacks, getFeedbacks } from "../../services/feedbackService";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import { Toast, ToastContainer } from "react-bootstrap";
import { PAGE_SIZE } from "../../constants/pagination";
import { useSearchParams } from "react-router-dom";

const PROCESS_KIND_OPTIONS: { id: "" | ProcessKind; label: string }[] = [
    { id: "", label: "Тип этапа" },
    { id: "Onboarding", label: "Онбординг" },
    { id: "Preboarding", label: "Пребординг" },
];

const RATING_OPTIONS: { id: "" | number; label: string }[] = [
    { id: "", label: "Все оценки" },
    { id: 5, label: "⭐⭐⭐⭐⭐" },
    { id: 4, label: "⭐⭐⭐⭐☆" },
    { id: 3, label: "⭐⭐⭐☆☆" },
    { id: 2, label: "⭐⭐☆☆☆" },
    { id: 1, label: "⭐☆☆☆☆" },
];

export default function Feedbacks() {
    const [searchParams, setSearchParams] = useSearchParams();
    const [showToast, setShowToast] = useState(false);
    const [toastMessage, setToastMessage] = useState("");

    const [feedbacks, setFeedbacks] = useState<Feedback[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [deletingFeedback, setDeletingFeedback] = useState<Feedback | null>(null);

    const search = searchParams.get("search") || "";
    const processKind = (searchParams.get("processKind") as ProcessKind) || "";
    const rating = searchParams.get("rating") || "";
    const page = Number(searchParams.get("page") || 1);
    const [totalCount, setTotalCount] = useState(0);

    useEffect(() => {
        setLoading(true);
        setError(null);

        const pagesToLoad = Array.from({ length: page }, (_, i) => i + 1);
        Promise.all(
            pagesToLoad.map((p) =>
                getFeedbacks(search, processKind || undefined, rating !== "" ? Number(rating) : undefined, p, PAGE_SIZE)
            )
        )
            .then((results) => {
                const allItems = results.flatMap((r) => r.items);
                const lastTotalCount = results.length > 0 ? results[results.length - 1].totalCount : 0;
                setFeedbacks(allItems);
                setTotalCount(lastTotalCount);
            })
            .catch(() => setError("Не удалось загрузить отзывы"))
            .finally(() => setLoading(false));
    }, [search, processKind, rating, page]);

    useEffect(() => {
        setFeedbacks([]);
        setTotalCount(0);
    }, [search, processKind, rating]);

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

    const handleKindChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setSearchParams(
            (params) => {
                if (e.target.value) params.set("processKind", e.target.value);
                else params.delete("processKind");
                params.set("page", "1");
                return params;
            },
            { replace: true }
        );
    };

    const handleRatingChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setSearchParams(
            (params) => {
                if (e.target.value) params.set("rating", e.target.value);
                else params.delete("rating");
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

    const noMore = feedbacks.length >= totalCount;

    const handleDelete = async () => {
        if (!deletingFeedback) return;
        try {
            await deleteFeedback(deletingFeedback.id);
            setFeedbacks((prev) => prev.filter((f) => f.id !== deletingFeedback.id));
            setToastMessage("Отзыв успешно удалён");
            setTotalCount((prev) => prev - 1);
            setShowToast(true);
        } catch {
            alert("Не удалось удалить отзыв");
        } finally {
            setDeletingFeedback(null);
        }
    };

    const handleExport = async () => {
        try {
            const blob = await exportFeedbacks();
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = `feedbacks_${new Date().toISOString().slice(0, 10)}.csv`;
            document.body.appendChild(a);
            a.click();
            a.remove();
            setToastMessage("Экспорт выполнен успешно");
            setShowToast(true);
        } catch {
            setError("Не удалось экспортировать отзывы");
            setShowToast(true);
        }
    };

    return (
        <div className="mx-4">
            <h5 className="text-center fw-semibold mb-3 mt-2">Отзывы</h5>
            <div className="mb-3">
                <select className="form-select rounded-3 mb-2" value={processKind} onChange={handleKindChange}>
                    {PROCESS_KIND_OPTIONS.map((opt) => (
                        <option key={opt.id} value={opt.id}>
                            {opt.label}
                        </option>
                    ))}
                </select>
                <select className="form-select rounded-3 mb-2" value={rating} onChange={handleRatingChange}>
                    {RATING_OPTIONS.map((opt) => (
                        <option key={opt.id} value={opt.id}>
                            {opt.label}
                        </option>
                    ))}
                </select>
                <div className="mb-3 d-flex gap-2 align-items-center">
                    <div className="flex-grow-1">
                        <SearchInput placeholder="Поиск по имени…" value={search} onChange={handleSearch} />
                    </div>
                    <button
                        className="btn btn-success fw-medium px-4 rounded-3 d-flex align-items-center gap-2"
                        type="button"
                        onClick={handleExport}
                    >
                        <i className="bi bi-download"></i>
                        Экспорт
                    </button>
                </div>
            </div>
            {loading && feedbacks.length === 0 ? (
                <LoadingSpinner />
            ) : error ? (
                <div className="alert alert-danger">{error}</div>
            ) : (
                <div className="d-flex flex-column gap-3">
                    {feedbacks.length > 0 ? (
                        <>
                            {feedbacks.map((feedback) => (
                                <FeedbackCard
                                    key={feedback.id}
                                    feedback={feedback}
                                    onDelete={() => setDeletingFeedback(feedback)}
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
                        <EmptyList title="Здесь пока пусто" subtitle="Пока никто не оставил отзыв." />
                    )}
                </div>
            )}
            {deletingFeedback && (
                <DeleteFeedbackModal
                    open={!!deletingFeedback}
                    feedback={deletingFeedback}
                    onClose={() => setDeletingFeedback(null)}
                    onDelete={handleDelete}
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
