import { useEffect, useState } from "react";
import CandidateCard from "./CandidateCard";
import SearchInput from "../../components/SearchInput/SearchInput";
import EmptyList from "../../components/EmptyList/EmptyList";
import DeleteCandidateModal from "./DeleteCandidateModal";
import type { UserDetail } from "../../models/User";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import { Toast, ToastContainer } from "react-bootstrap";
import { useToastFromLocation } from "../../hooks/useToastFromLocation";
import { getUsers } from "../../services/userService";
import { useSearchParams } from "react-router-dom";
import { deleteUser } from "../../services/userService";

const PROCESS_KIND_OPTIONS = [
    { value: "", label: "Все этапы" },
    { value: "Preboarding", label: "Пребординг" },
    { value: "Onboarding", label: "Онбординг" },
    { value: "PersonalArea", label: "Личный кабинет" },
];

export default function Candidates() {
    const [searchParams, setSearchParams] = useSearchParams();
    const [candidates, setCandidates] = useState<UserDetail[]>([]);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [deletingCandidate, setDeletingCandidate] = useState<UserDetail | null>(null);

    const search = searchParams.get("search") || "";
    const processKind = searchParams.get("processKind") || "";
    const page = Number(searchParams.get("page") || 1);
    const [totalCount, setTotalCount] = useState(0);

    const [showToast, setShowToast] = useState(false);
    const [toastMessage, setToastMessage] = useState("");
    useToastFromLocation(setToastMessage, setShowToast);

    useEffect(() => {
        setLoading(true);
        setError(null);

        const pagesToLoad = Array.from({ length: page }, (_, i) => i + 1);

        Promise.all(
            pagesToLoad.map((p) =>
                getUsers(search, p, undefined, ["Candidate"], undefined, undefined, processKind || undefined)
            )
        )
            .then((results) => {
                const allItems = results.flatMap((res) => res.items);
                const lastTotalCount = results.length > 0 ? results[results.length - 1].totalCount : 0;
                setCandidates(allItems);
                setTotalCount(lastTotalCount);
            })
            .catch(() => setError("Не удалось загрузить кандидатов"))
            .finally(() => setLoading(false));
    }, [search, page, processKind]);

    useEffect(() => {
        setCandidates([]);
        setTotalCount(0);
    }, [search, processKind]);

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

    const handleProcessKindChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const value = e.target.value;
        setSearchParams(
            (params) => {
                if (value) params.set("processKind", value);
                else params.delete("processKind");
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

    const noMore = candidates.length >= totalCount;

    const handleDelete = async () => {
        if (!deletingCandidate) return;
        try {
            await deleteUser(deletingCandidate.id);
            setCandidates((prev) => prev.filter((c) => c.id !== deletingCandidate.id));
            setTotalCount((prev) => prev - 1);
            setToastMessage("Кандидат успешно удалён");
            setShowToast(true);
        } catch {
            alert("Не удалось удалить кандидата");
        } finally {
            setDeletingCandidate(null);
        }
    };

    return (
        <div className="mx-4">
            <h5 className="text-center fw-semibold mb-3 mt-2">Кандидаты</h5>
            <select className="form-select rounded-3 mb-2" value={processKind} onChange={handleProcessKindChange}>
                {PROCESS_KIND_OPTIONS.map((option) => (
                    <option key={option.value} value={option.value}>
                        {option.label}
                    </option>
                ))}
            </select>
            <div className="mb-3">
                <SearchInput placeholder="Поиск кандидатов…" value={search} onChange={handleSearch} />
            </div>

            {loading && candidates.length === 0 ? (
                <LoadingSpinner />
            ) : error ? (
                <div className="alert alert-danger">{error}</div>
            ) : (
                <div className="d-flex flex-column gap-3">
                    {candidates.length > 0 ? (
                        <>
                            {candidates.map((candidate) => (
                                <CandidateCard
                                    key={candidate.id}
                                    candidate={candidate}
                                    onDelete={() => setDeletingCandidate(candidate)}
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
                        <EmptyList title="Здесь пока пусто" subtitle="Добавьте кандидатов через приглашение в бота." />
                    )}
                </div>
            )}
            {deletingCandidate && (
                <DeleteCandidateModal
                    open={!!deletingCandidate}
                    candidate={deletingCandidate}
                    onClose={() => setDeletingCandidate(null)}
                    onDelete={handleDelete}
                />
            )}
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
