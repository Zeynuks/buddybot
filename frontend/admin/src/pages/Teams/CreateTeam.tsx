import { useEffect, useState } from "react";
import type { DepartmentLookup } from "../../models/Department";
import { useNavigate } from "react-router-dom";
import { getDepartmentsLookup } from "../../services/departmentService";
import { createTeam } from "../../services/teamService";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import { ROUTES } from "../../constants/routes";

export default function CreateTeam() {
    const [name, setName] = useState("");
    const [departmentId, setDepartmentId] = useState<number | "">("");
    const [departments, setDepartments] = useState<DepartmentLookup[]>([]);

    const [initialLoading, setInitialLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const navigate = useNavigate();

    useEffect(() => {
        let mounted = true;
        setInitialLoading(true);
        setError(null);
        getDepartmentsLookup()
            .then((deps) => {
                if (mounted) setDepartments(deps);
            })
            .catch(() => {
                if (mounted) setError("Не удалось загрузить справочники");
            })
            .finally(() => {
                if (mounted) setInitialLoading(false);
            });
        return () => {
            mounted = false;
        };
    }, []);

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setError(null);
        setSaving(true);
        try {
            await createTeam({
                name,
                departmentId: Number(departmentId),
            });
            navigate(ROUTES.TEAMS.ROOT, { state: { toast: "Команда успешно создана" } });
        } catch (e) {
            setError(e instanceof Error ? e.message : "Ошибка добавления команды");
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
                    <div className="text-center fs-4 mb-4 mt-2">Создание команды</div>
                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 required">Название команды</label>
                            <input
                                type="text"
                                className="form-control rounded-3"
                                placeholder="Введите название команды"
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
                        {error && <div className="alert alert-danger">{error}</div>}
                        <button
                            type="submit"
                            className="btn btn-primary w-100 py-2 rounded-3 fw-semibold"
                            disabled={saving}
                        >
                            {saving ? "Создание..." : "Создать команду"}
                        </button>
                    </form>
                </div>
            )}
        </div>
    );
}
