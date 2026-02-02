import MemberCard from "./MemberCard";
import SearchInput from "../../components/SearchInput/SearchInput";
import EmptyList from "../../components/EmptyList/EmptyList";
import { useEffect, useState } from "react";
import type { DepartmentLookup } from "../../models/Department";
import type { UserDetail } from "../../models/User";
import type { TeamLookup } from "../../models/Teams";
import { getDepartmentsLookup } from "../../services/departmentService";
import { getTeamsLookup } from "../../services/teamService";
import AddMemberModal from "./AddMemberModal";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import { Toast, ToastContainer } from "react-bootstrap";
import { useToastFromLocation } from "../../hooks/useToastFromLocation";
import { getUsers } from "../../services/userService";
import { PAGE_SIZE } from "../../constants/pagination";
import { useSearchParams } from "react-router-dom";
import DeleteMemberModal from "./DeleteMemberModal";
import { deleteUser } from "../../services/userService";

export default function Members() {
    const [searchParams, setSearchParams] = useSearchParams();
    const [members, setMembers] = useState<UserDetail[]>([]);
    const [departments, setDepartments] = useState<DepartmentLookup[]>([]);
    const [teams, setTeams] = useState<TeamLookup[]>([]);

    const departmentId = searchParams.get("departmentId") || "";
    const teamId = searchParams.get("teamId") || "";
    const role = searchParams.get("role") || "";
    const search = searchParams.get("search") || "";
    const page = Number(searchParams.get("page") || 1);
    const [totalCount, setTotalCount] = useState(0);

    const [initialLoading, setInitialLoading] = useState(true);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [showAddModal, setShowAddModal] = useState(false);
    const [deletingMember, setDeletingMember] = useState<UserDetail | null>(null);

    const [showToast, setShowToast] = useState(false);
    const [toastMessage, setToastMessage] = useState("");
    useToastFromLocation(setToastMessage, setShowToast);

    useEffect(() => {
        setInitialLoading(true);
        setError(null);
        getDepartmentsLookup()
            .then(setDepartments)
            .catch(() => setError("Не удалось загрузить справочники отделов"))
            .finally(() => setInitialLoading(false));
    }, []);

    useEffect(() => {
        if (departmentId) {
            getTeamsLookup(Number(departmentId))
                .then(setTeams)
                .catch(() => setTeams([]));
        } else {
            setTeams([]);
        }
        setSearchParams(
            (params) => {
                if (!departmentId) {
                    params.delete("teamId");
                }
                return params;
            },
            { replace: true }
        );
    }, [departmentId, setSearchParams]);

    useEffect(() => {
        setLoading(true);
        setError(null);

        const pagesToLoad = Array.from({ length: page }, (_, i) => i + 1);

        Promise.all(
            pagesToLoad.map((p) =>
                getUsers(
                    search,
                    p,
                    PAGE_SIZE,
                    role ? [role] : ["HR", "Mentor"],
                    departmentId !== "" ? Number(departmentId) : undefined,
                    teamId !== "" ? Number(teamId) : undefined
                )
            )
        )
            .then((results) => {
                const allItems = results.flatMap((res) => res.items);
                const lastTotalCount = results.length > 0 ? results[results.length - 1].totalCount : 0;
                setMembers(allItems);
                setTotalCount(lastTotalCount);
            })
            .catch(() => setError("Не удалось загрузить участников"))
            .finally(() => setLoading(false));
    }, [search, page, departmentId, teamId, role]);

    useEffect(() => {
        setMembers([]);
        setTotalCount(0);
    }, [search, departmentId, teamId]);

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
                if (value === "") {
                    params.delete("departmentId");
                    params.delete("teamId");
                } else {
                    params.set("departmentId", value);
                    params.delete("teamId");
                }
                params.set("page", "1");
                return params;
            },
            { replace: true }
        );
    };

    const handleTeamChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const value = e.target.value;
        setSearchParams(
            (params) => {
                if (value === "") {
                    params.delete("teamId");
                } else {
                    params.set("teamId", value);
                }
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

    const noMore = members.length >= totalCount;

    const handleDeleteMember = async () => {
        if (!deletingMember) return;
        try {
            await deleteUser(deletingMember.id);
            setMembers((prev) => prev.filter((m) => m.id !== deletingMember.id));
            setTotalCount((prev) => prev - 1);
            setToastMessage("Участник успешно удалён");
            setShowToast(true);
        } catch {
            alert("Не удалось удалить участника");
        } finally {
            setDeletingMember(null);
        }
    };

    return (
        <div className="mx-4">
            <h5 className="text-center fw-semibold mb-3 mt-2">Участники</h5>
            <div className="mb-3">
                <button
                    className="btn btn-primary w-100 fw-medium py-2 mb-3 rounded-pill"
                    onClick={() => setShowAddModal(true)}
                >
                    + Добавить участника
                </button>
                <AddMemberModal open={showAddModal} onClose={() => setShowAddModal(false)} />
                <select className="form-select rounded-3 mb-2" value={role} onChange={handleRoleChange}>
                    <option value="">Роль</option>
                    <option value="Mentor">Наставник</option>
                    <option value="HR">HR</option>
                </select>
                <select className="form-select rounded-3 mb-2" value={departmentId} onChange={handleDepartmentChange}>
                    <option value="">Отдел</option>
                    {departments.map((dep) => (
                        <option key={dep.id} value={dep.id}>
                            {dep.name}
                        </option>
                    ))}
                </select>
                <select
                    className="form-select rounded-3 mb-2"
                    value={teamId}
                    onChange={handleTeamChange}
                    disabled={!departmentId}
                >
                    <option value="">Команда</option>
                    {teams.map((team) => (
                        <option key={team.id} value={team.id}>
                            {team.name}
                        </option>
                    ))}
                </select>
                <SearchInput placeholder="Поиск участника..." value={search} onChange={handleSearch} />
            </div>
            {initialLoading ? (
                <LoadingSpinner />
            ) : loading && members.length === 0 ? (
                <div className="py-5">
                    <div className="d-flex justify-content-center">
                        <LoadingSpinner />
                    </div>
                </div>
            ) : error ? (
                <div className="alert alert-danger">{error}</div>
            ) : (
                <div className="d-flex flex-column gap-3">
                    {members.length > 0 ? (
                        <>
                            {members.map((member) => (
                                <MemberCard key={member.id} member={member} onDelete={setDeletingMember} />
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
                            subtitle="Нажмите «Добавить участника», чтобы добавить нового участника."
                        />
                    )}
                </div>
            )}
            <DeleteMemberModal
                open={!!deletingMember}
                member={deletingMember}
                onClose={() => setDeletingMember(null)}
                onDelete={handleDeleteMember}
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
