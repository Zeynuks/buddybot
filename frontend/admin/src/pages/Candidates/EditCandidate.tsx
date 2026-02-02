import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getDepartmentsLookup } from "../../services/departmentService";
import { getTeamsLookup } from "../../services/teamService";
import { getUserById, getUsersLookup, updateCandidate } from "../../services/userService";
import { ROUTES } from "../../constants/routes";
import type { DepartmentLookup } from "../../models/Department";
import type { TeamLookup } from "../../models/Teams";
import type { UserLookup } from "../../models/User";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import { HelpTooltip } from "../../components/HelpTooltip/HelpTooltip";
import { createOnboardingAccessRequest } from "../../services/onboardingAccessRequestService";

export default function EditCandidate() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();

    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [departmentId, setDepartmentId] = useState<number | undefined>();
    const [teamId, setTeamId] = useState<number | undefined>();
    const [hrId, setHrId] = useState<number | undefined>();
    const [mentorId, setMentorId] = useState<number | undefined>();
    const [telegramId, setTelegramId] = useState<number | null>(null);
    const [isOnboardingAccessGranted, setIsOnboardingAccessGranted] = useState(false);

    const [onboardingAccess, setOnboardingAccess] = useState(false);

    const [departments, setDepartments] = useState<DepartmentLookup[]>([]);
    const [teams, setTeams] = useState<TeamLookup[]>([]);
    const [hrs, setHrs] = useState<UserLookup[]>([]);
    const [mentors, setMentors] = useState<UserLookup[]>([]);

    const [initialLoading, setInitialLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        let mounted = true;
        setInitialLoading(true);

        (async () => {
            try {
                const [candidate, departmentsList, hrsList] = await Promise.all([
                    getUserById(Number(id)),
                    getDepartmentsLookup(),
                    getUsersLookup({ roles: ["HR"] }),
                ]);

                if (!mounted) return;

                setDepartments(departmentsList);
                setHrs(hrsList);

                setFirstName(candidate.firstName || "");
                setLastName(candidate.lastName || "");
                setHrId(candidate.hRs?.[0]?.id);
                setMentorId(candidate.mentors?.[0]?.id);
                setOnboardingAccess(candidate.isOnboardingAccessGranted);
                setTelegramId(candidate.telegramId);
                setIsOnboardingAccessGranted(candidate.isOnboardingAccessGranted);

                if (candidate.team?.departmentId) {
                    setDepartmentId(candidate.team.departmentId);
                    const teamsList = await getTeamsLookup(candidate.team.departmentId);
                    if (!mounted) return;
                    setTeams(teamsList);
                    setTeamId(candidate.team.id);
                } else {
                    setDepartmentId(undefined);
                    setTeams([]);
                    setTeamId(undefined);
                }
            } catch {
                setError("Не удалось загрузить данные кандидата");
            } finally {
                setInitialLoading(false);
            }
        })();

        return () => {
            mounted = false;
        };
    }, [id]);

    useEffect(() => {
        if (!departmentId) {
            setTeams([]);
            setTeamId(undefined);
            return;
        }
        getTeamsLookup(departmentId).then((teamsList) => {
            setTeams(teamsList);
            setTeamId((prev) => (teamsList.some((t) => t.id === prev) ? prev : undefined));
        });
    }, [departmentId]);

    useEffect(() => {
        if (!teamId) {
            setMentors([]);
            setMentorId(undefined);
            return;
        }
        getUsersLookup({ roles: ["Mentor"], teamId: teamId })
            .then((list) => {
                setMentors(list);
                setMentorId((prev) => (prev && list.some((m) => m.id === prev) ? prev : undefined));
            })
            .catch(() => {
                setMentors([]);
                setMentorId(undefined);
            });
    }, [teamId]);

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setSaving(true);
        setError(null);

        try {
            await updateCandidate(Number(id), {
                firstName,
                lastName,
                teamId,
                hrIds: hrId ? [hrId] : [],
                mentorIds: mentorId ? [mentorId] : [],
                onboardingAccessTimeUtc: onboardingAccess ? new Date().toISOString() : undefined,
            });
            if (onboardingAccess && !isOnboardingAccessGranted) {
                try {
                    await createOnboardingAccessRequest(Number(id), "Scheduled");
                } catch (e) {
                    setError(e instanceof Error ? e.message : "Ошибка при создании заявки на онбординг");
                    setSaving(false);
                    return;
                }
            }
            navigate(ROUTES.CANDIDATES.ROOT, {
                state: { toast: "Изменения успешно сохранены" },
            });
        } catch (e) {
            setError(e instanceof Error ? e.message : "Ошибка сохранения изменений");
        } finally {
            setSaving(false);
        }
    }

    return (
        <div className="mx-4 mt-4">
            {initialLoading ? (
                <LoadingSpinner />
            ) : error ? (
                <div className="alert alert-danger">{error}</div>
            ) : (
                <div className="card border-0 shadow-sm rounded-4 p-4 mb-4 position-relative mx-auto">
                    <button
                        className="btn btn-link text-primary px-0 mb-2 d-flex align-items-center text-decoration-none"
                        type="button"
                        onClick={() => navigate(-1)}
                    >
                        <i className="bi bi-chevron-left small align-middle"></i>
                        <span className="ms-1">Назад</span>
                    </button>
                    <div className="text-center fs-4 mb-4 mt-2">Редактирование кандидата</div>
                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 required">Имя</label>
                            <input
                                type="text"
                                className="form-control rounded-3"
                                placeholder="Введите имя"
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
                                placeholder="Введите фамилию"
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
                                value={departmentId ?? ""}
                                onChange={(e) => setDepartmentId(Number(e.target.value) || undefined)}
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
                                value={teamId ?? ""}
                                onChange={(e) => setTeamId(Number(e.target.value) || undefined)}
                                required
                                disabled={!departmentId}
                            >
                                <option value="">Выберите команду</option>
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
                                value={mentorId ?? ""}
                                onChange={(e) => setMentorId(Number(e.target.value) || undefined)}
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
                            <label className="form-label fs-6 mb-1 required">HR</label>
                            <select
                                className="form-select rounded-3"
                                value={hrId ?? ""}
                                onChange={(e) => setHrId(Number(e.target.value) || undefined)}
                                required
                            >
                                <option value="">Выберите HR</option>
                                {hrs.map((hr) => (
                                    <option key={hr.id} value={hr.id}>
                                        {hr.firstName} {hr.lastName}
                                    </option>
                                ))}
                            </select>
                        </div>
                        {typeof telegramId === "number" && telegramId > 0 && (
                            <div className="mb-4 form-check d-flex align-items-center">
                                <input
                                    type="checkbox"
                                    className="form-check-input"
                                    id="onboardingAccess"
                                    checked={onboardingAccess}
                                    onChange={() => setOnboardingAccess((v) => !v)}
                                    disabled={isOnboardingAccessGranted}
                                />
                                <label className="form-check-label ms-2" htmlFor="onboardingAccess">
                                    Доступ к онбордингу
                                </label>
                                {isOnboardingAccessGranted && (
                                    <HelpTooltip>
                                        Доступ к онбордингу уже выдан или заявка на онбординг уже создана.
                                    </HelpTooltip>
                                )}
                            </div>
                        )}
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
