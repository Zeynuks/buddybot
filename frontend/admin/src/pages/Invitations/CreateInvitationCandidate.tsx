import type { DepartmentLookup } from "../../models/Department";
import type { TeamLookup } from "../../models/Teams";
import type { UserLookup } from "../../models/User";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getDepartmentsLookup } from "../../services/departmentService";
import { getTeamsLookup } from "../../services/teamService";
import { getUsersLookup } from "../../services/userService";
import { createCandidateInvitation } from "../../services/invitationService";
import { ROUTES } from "../../constants/routes";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import useAuth from "../../hooks/useAuth";

export default function CreateInvitationCandidate() {
    const { user } = useAuth();

    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [departmentId, setDepartmentId] = useState<number | "">("");
    const [teamId, setTeamId] = useState<number | "">("");
    const [mentorId, setMentorId] = useState<number | "">("");
    const [hrId, setHrId] = useState<number | "">("");
    const [expirationDays, setExpirationDays] = useState(1);

    const [departments, setDepartments] = useState<DepartmentLookup[]>([]);
    const [teams, setTeams] = useState<TeamLookup[]>([]);
    const [mentors, setMentors] = useState<UserLookup[]>([]);
    const [hrs, setHrs] = useState<UserLookup[]>([]);

    const [initialLoading, setInitialLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        let mounted = true;
        async function fetchAllLookups() {
            setInitialLoading(true);
            setError(null);
            try {
                const [deps, hrsList] = await Promise.all([getDepartmentsLookup(), getUsersLookup({ roles: ["HR"] })]);
                if (!mounted) return;
                setDepartments(deps);
                setHrs(hrsList);
            } catch {
                if (mounted) setError("Ошибка загрузки справочников");
            } finally {
                if (mounted) setInitialLoading(false);
            }
        }
        fetchAllLookups();
        return () => {
            mounted = false;
        };
    }, []);

    useEffect(() => {
        if (departmentId) {
            getTeamsLookup(Number(departmentId))
                .then(setTeams)
                .catch(() => setTeams([]));
        } else {
            setTeams([]);
            setTeamId("");
        }
        setMentors([]);
        setMentorId("");
    }, [departmentId]);

    useEffect(() => {
        if (teamId) {
            getUsersLookup({ roles: ["Mentor"], teamId: Number(teamId) })
                .then(setMentors)
                .catch(() => setMentors([]));
        } else {
            setMentors([]);
            setMentorId("");
        }
    }, [teamId]);

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setSaving(true);
        setError(null);

        try {
            if (!user) {
                throw new Error("Не удалось определить пользователя");
            }
            const result = await createCandidateInvitation({
                firstName,
                lastName,
                teamId: Number(teamId),
                expirationDays,
                mentorIds: mentorId ? [Number(mentorId)] : [],
                hrIds: hrId ? [Number(hrId)] : [],
                creatorId: user.id,
            });

            navigate(ROUTES.INVITATIONS.DETAIL(result.tokenValue), {
                state: { toast: "Приглашение успешно создано" },
            });
        } catch (e) {
            setError(e instanceof Error ? e.message : "Ошибка при создании приглашения для кандидата");
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
                    <div className="text-center fs-4 mb-4 mt-2">Генерация ссылки / QR-кода</div>
                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 required">Имя</label>
                            <input
                                type="text"
                                className="form-control rounded-3"
                                placeholder="Введите имя кандидата"
                                value={firstName}
                                onChange={(e) => setFirstName(e.target.value)}
                                required
                                minLength={2}
                                maxLength={50}
                            />
                        </div>
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 required">Фамилия</label>
                            <input
                                type="text"
                                className="form-control rounded-3"
                                placeholder="Введите фамилию кандидата"
                                value={lastName}
                                onChange={(e) => setLastName(e.target.value)}
                                required
                                minLength={2}
                                maxLength={50}
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
                                <option value="">Выберите отдел</option>
                                {departments.map((dep) => (
                                    <option key={dep.id} value={dep.id}>
                                        {dep.name}
                                    </option>
                                ))}
                            </select>
                        </div>
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 required">Команда</label>
                            <select
                                className="form-select rounded-3"
                                value={teamId}
                                onChange={(e) => setTeamId(e.target.value ? Number(e.target.value) : "")}
                                required
                                disabled={!departmentId}
                            >
                                <option value="" disabled>
                                    Выберите команду
                                </option>
                                {teams.map((team) => (
                                    <option key={team.id} value={team.id}>
                                        {team.name}
                                    </option>
                                ))}
                            </select>
                        </div>
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 required">Наставник</label>
                            <select
                                className="form-select rounded-3"
                                value={mentorId}
                                onChange={(e) => setMentorId(e.target.value ? Number(e.target.value) : "")}
                                disabled={!teamId}
                                required
                            >
                                <option value="">Выберите наставника</option>
                                {mentors.map((mentor) => (
                                    <option key={mentor.id} value={mentor.id}>
                                        {mentor.firstName} {mentor.lastName}
                                    </option>
                                ))}
                            </select>
                        </div>
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 required">Закрепляемый HR</label>
                            <select
                                className="form-select rounded-3"
                                value={hrId}
                                onChange={(e) => setHrId(e.target.value ? Number(e.target.value) : "")}
                                required
                            >
                                <option value="">Выберите HR-а</option>
                                {hrs.map((hr) => (
                                    <option key={hr.id} value={hr.id}>
                                        {hr.firstName} {hr.lastName}
                                    </option>
                                ))}
                            </select>
                        </div>
                        <div className="mb-4">
                            <label className="form-label fs-6 mb-2">Срок действия ссылки</label>
                            <div className="d-flex gap-2 justify-content-center">
                                {[1, 7, 30, 3650].map((d) => (
                                    <button
                                        key={d}
                                        type="button"
                                        className={`btn rounded-3 px-3 ${
                                            expirationDays === d ? "btn-primary" : "btn-outline-primary"
                                        }`}
                                        style={{ minWidth: 120 }}
                                        onClick={() => setExpirationDays(d)}
                                    >
                                        {d === 3650 ? "Без срока" : `${d} ${d === 1 ? "день" : "дней"}`}
                                    </button>
                                ))}
                            </div>
                        </div>
                        {error && <div className="alert alert-danger mb-3">{error}</div>}
                        <button
                            type="submit"
                            className="btn btn-primary w-100 py-2 rounded-3 fw-semibold"
                            disabled={saving}
                        >
                            {saving ? "Сохраняем..." : "Сгенерировать ссылку"}
                        </button>
                    </form>
                </div>
            )}
        </div>
    );
}
