import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import type { DepartmentLookup } from "../../models/Department";
import type { UserLookup } from "../../models/User";
import { getTeam, updateTeam } from "../../services/teamService";
import { getDepartmentsLookup } from "../../services/departmentService";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import { ROUTES } from "../../constants/routes";
import { getUsersLookup } from "../../services/userService";

export default function EditTeam() {
    const { id } = useParams<{ id: string }>();
    const [name, setName] = useState("");
    const [departmentId, setDepartmentId] = useState<number | "">("");
    const [leaderId, setLeaderId] = useState<number | "">("");
    const [departments, setDepartments] = useState<DepartmentLookup[]>([]);
    const [leaders, setLeaders] = useState<UserLookup[]>([]);
    const [initialLoading, setInitialLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        let mounted = true;
        setInitialLoading(true);
        setError(null);

        if (!id) return setInitialLoading(false);

        Promise.all([
            getTeam(Number(id)),
            getDepartmentsLookup(),
            getUsersLookup({ roles: ["Mentor", "HR"], teamId: Number(id) }),
        ])
            .then(([team, deps, users]) => {
                if (!mounted) return;
                setName(team.name ?? "");
                setDepartmentId(team.departmentId);
                setLeaderId(team.leaderId ?? "");
                setDepartments(deps);
                setLeaders(users);
            })
            .catch(() => {
                if (mounted) setError("Не удалось загрузить данные");
            })
            .finally(() => {
                if (mounted) setInitialLoading(false);
            });

        return () => {
            mounted = false;
        };
    }, [id]);

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        if (!id) return;
        setSaving(true);
        setError(null);
        try {
            await updateTeam(Number(id), {
                name,
                departmentId: Number(departmentId),
                leaderId: leaderId ? Number(leaderId) : undefined,
            });
            navigate(ROUTES.TEAMS.ROOT, { state: { toast: "Изменения успешно сохранены" } });
        } catch (e) {
            setError(e instanceof Error ? e.message : "Ошибка сохранения");
        } finally {
            setSaving(false);
        }
    }

    return (
        <div className="mx-4 mt-4">
            {initialLoading ? (
                <LoadingSpinner />
            ) : (
                <div className="card border-0 shadow-sm rounded-4 p-4 mb-4 mx-auto">
                    <button
                        className="btn btn-link text-primary px-0 mb-2 d-flex align-items-center text-decoration-none"
                        type="button"
                        onClick={() => navigate(-1)}
                    >
                        <i className="bi bi-chevron-left small align-middle"></i>
                        <span className="ms-1">Назад</span>
                    </button>
                    <div className="text-center fs-4 mb-4 mt-2">Редактирование команды</div>
                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 required">Название команды</label>
                            <input
                                type="text"
                                className="form-control rounded-3"
                                placeholder="Название команды"
                                value={name}
                                onChange={(e) => setName(e.target.value)}
                                required
                                minLength={2}
                                maxLength={100}
                            />
                        </div>
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 required">Отдел</label>
                            <select
                                className="form-select rounded-3"
                                value={departmentId}
                                onChange={(e) => setDepartmentId(e.target.value ? Number(e.target.value) : "")}
                                required
                            >
                                <option value="" disabled>
                                    Выберите отдел
                                </option>
                                {departments.map((dep) => (
                                    <option key={dep.id} value={dep.id}>
                                        {dep.name}
                                    </option>
                                ))}
                            </select>
                        </div>
                        <div className="mb-4">
                            <label className="form-label fs-6 mb-1">Руководитель команды</label>
                            <select
                                className="form-select rounded-3"
                                value={leaderId}
                                onChange={(e) => setLeaderId(e.target.value ? Number(e.target.value) : "")}
                            >
                                <option value="">Без руководителя</option>
                                {leaders.map((leader) => (
                                    <option key={leader.id} value={leader.id}>
                                        {leader.firstName} {leader.lastName}
                                    </option>
                                ))}
                            </select>
                        </div>
                        {error && <div className="alert alert-danger">{error}</div>}
                        <button
                            type="submit"
                            className="btn btn-primary w-100 py-2 rounded-3 fw-semibold"
                            disabled={saving}
                        >
                            {saving ? "Сохранение..." : "Сохранить"}
                        </button>
                    </form>
                </div>
            )}
        </div>
    );
}
