import { Link, useSearchParams } from "react-router-dom";
import { ROUTES } from "../../constants/routes";
import TeamCard from "./TeamCard";
import SearchInput from "../../components/SearchInput/SearchInput";
import EmptyList from "../../components/EmptyList/EmptyList";
import { useEffect, useState } from "react";
import type { TeamList } from "../../models/Teams";
import { getTeams } from "../../services/teamService";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import { useToastFromLocation } from "../../hooks/useToastFromLocation";
import { Toast, ToastContainer } from "react-bootstrap";
import { getDepartmentsLookup } from "../../services/departmentService";
import DeleteTeamModal from "./DeleteTeamModal";
import { deleteTeam } from "../../services/teamService";
import useAuth from "../../hooks/useAuth";
import { PermissionName } from "../../constants/permissions";
import { hasPermission } from "../../utils/hasPermission";

export default function Teams() {
    const { permissions } = useAuth();
    const [searchParams, setSearchParams] = useSearchParams();
    const [teams, setTeams] = useState<TeamList[]>([]);
    const [departments, setDepartments] = useState<{ id: number; name: string | null }[]>([]);

    const departmentId = searchParams.get("departmentId") || "";
    const search = searchParams.get("search") || "";
    const page = Number(searchParams.get("page") || 1);
    const [totalCount, setTotalCount] = useState(0);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [deletingTeam, setDeletingTeam] = useState<TeamList | null>(null);

    const [showToast, setShowToast] = useState(false);
    const [toastMessage, setToastMessage] = useState("");
    useToastFromLocation(setToastMessage, setShowToast);

    useEffect(() => {
        getDepartmentsLookup()
            .then(setDepartments)
            .catch(() => setDepartments([]));
    }, []);

    useEffect(() => {
        setLoading(true);
        setError(null);

        const pagesToLoad = Array.from({ length: page }, (_, i) => i + 1);

        Promise.all(pagesToLoad.map((p) => getTeams(departmentId !== "" ? Number(departmentId) : undefined, search, p)))
            .then((results) => {
                const allItems = results.flatMap((res) => res.items);
                const lastTotalCount = results.length > 0 ? results[results.length - 1].totalCount : 0;
                setTeams(allItems);
                setTotalCount(lastTotalCount);
            })
            .catch((error) => setError(error instanceof Error ? error.message : "Не удалось загрузить команды"))
            .finally(() => setLoading(false));
    }, [departmentId, search, page]);

    useEffect(() => {
        setTeams([]);
        setTotalCount(0);
    }, [departmentId, search]);

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

    const handleDepartmentChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const value = e.target.value;
        setSearchParams(
            (params) => {
                if (value === "") params.delete("departmentId");
                else params.set("departmentId", value);
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

    const noMore = teams.length >= totalCount;

    const handleDelete = async () => {
        if (!deletingTeam) return;
        try {
            await deleteTeam(deletingTeam.id);
            setTeams((prev) => prev.filter((t) => t.id !== deletingTeam.id));
            setTotalCount((prev) => prev - 1);
            setToastMessage("Команда успешно удалена");
            setShowToast(true);
        } catch {
            alert("Не удалось удалить команду");
        } finally {
            setDeletingTeam(null);
        }
    };

    return (
        <div className="mx-4">
            <h5 className="text-center fw-semibold mb-3 mt-2">Команды</h5>
            <div className="mb-3">
                {hasPermission(permissions, PermissionName.TeamCreate) && (
                    <Link to={ROUTES.TEAMS.CREATE} className="btn btn-primary w-100 fw-medium py-2 mb-3 rounded-pill">
                        + Добавить команду
                    </Link>
                )}

                <select className="form-select rounded-3 mb-2" value={departmentId} onChange={handleDepartmentChange}>
                    <option value="">Все отделы</option>
                    {departments.map((dep) => (
                        <option key={dep.id} value={dep.id}>
                            {dep.name}
                        </option>
                    ))}
                </select>
                <SearchInput placeholder="Поиск команды…" value={search} onChange={handleSearch} />
            </div>
            {loading && teams.length === 0 ? (
                <LoadingSpinner />
            ) : error ? (
                <div className="alert alert-danger">{error}</div>
            ) : (
                <div className="d-flex flex-column gap-3">
                    {teams.length > 0 ? (
                        <>
                            {teams.map((team) => (
                                <TeamCard key={team.id} team={team} onDelete={setDeletingTeam} />
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
                            subtitle="Нажмите «Добавить команду», чтобы создать новую команду."
                        />
                    )}
                </div>
            )}
            <DeleteTeamModal
                open={!!deletingTeam}
                team={deletingTeam}
                onClose={() => setDeletingTeam(null)}
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
