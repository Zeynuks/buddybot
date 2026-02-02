import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import type { DepartmentLookup } from "../../models/Department";
import type { TeamLookup } from "../../models/Teams";
import type { UserLookup } from "../../models/User";
import type { OnboardingAccessRequest } from "../../models/OnboardingAccessRequests";
import {
    getOnboardingAccessRequestById,
    updateOnboardingAccessRequest,
} from "../../services/onboardingAccessRequestService";
import { getDepartmentsLookup } from "../../services/departmentService";
import { getUsersLookup } from "../../services/userService";
import { getTeamsLookup } from "../../services/teamService";
import { ROUTES } from "../../constants/routes";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import dayjs from "dayjs";
import "dayjs/locale/ru";
import DatePicker from "react-datepicker";
dayjs.locale("ru");
import "react-datepicker/dist/react-datepicker.css";
import { ru } from "date-fns/locale";

export default function EditOnboardingAccessRequest() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();

    const [departmentId, setDepartmentId] = useState<number | undefined>();
    const [teamId, setTeamId] = useState<number | undefined>();
    const [onboardingAccessTime, setOnboardingAccessTime] = useState<Date | null>(null);
    const [mentorId, setMentorId] = useState<number | undefined>();

    const [departments, setDepartments] = useState<DepartmentLookup[]>([]);
    const [teams, setTeams] = useState<TeamLookup[]>([]);
    const [mentors, setMentors] = useState<UserLookup[]>([]);

    const [initialLoading, setInitialLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [request, setRequest] = useState<OnboardingAccessRequest>();

    useEffect(() => {
        let mounted = true;
        setInitialLoading(true);
        setError(null);

        (async () => {
            try {
                const [request, departmentsList, mentorsList] = await Promise.all([
                    getOnboardingAccessRequestById(id!),
                    getDepartmentsLookup(),
                    getUsersLookup({ roles: ["Mentor"] }),
                ]);
                if (!mounted) return;

                setRequest(request);
                setDepartments(departmentsList);
                setOnboardingAccessTime(request.onboardingAccessTime ? new Date(request.onboardingAccessTime) : null);
                setMentors(mentorsList);

                setMentorId(request.mentors?.[0]?.id);
                setDepartmentId(request.department?.id);

                if (request.department?.id) {
                    const teamsList = await getTeamsLookup(request.department.id);
                    if (!mounted) return;
                    setTeams(teamsList);
                    setTeamId(request.team?.id);
                } else {
                    setTeams([]);
                    setTeamId(undefined);
                }
            } catch {
                setError("Не удалось загрузить приглашение или справочники");
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

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setSaving(true);
        setError(null);

        if (!onboardingAccessTime) {
            setError("Выберите дату и время начала онбординга");
            setSaving(false);
            return;
        }
        if (onboardingAccessTime.getTime() <= Date.now()) {
            setError("Дата и время начала онбординга не может быть в прошлом");
            setSaving(false);
            return;
        }

        try {
            await updateOnboardingAccessRequest(id!, {
                onboardingAccessTimeUtc: onboardingAccessTime ? onboardingAccessTime.toISOString() : undefined,
                teamId,
                mentorIds: mentorId ? [mentorId] : undefined,
            });
            navigate(ROUTES.ONBOARDING_ACCESS_REQUESTS.ROOT, { state: { toast: "Изменения успешно сохранены" } });
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
            ) : !request ? (
                <div className="alert alert-danger py-2 small mb-3">Не удалось загрузить заявку</div>
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

                    <div className="text-center fs-4 mb-4 mt-2">Редактирование заявки</div>
                    <div className="fw-semibold mb-1">
                        {request.firstName} {request.lastName}
                    </div>
                    <div className="small text-secondary mb-2">
                        <i className="bi bi-calendar me-1"></i>
                        Создана: {dayjs(request.createdAt).format("D MMM YYYY HH:mm")}
                    </div>
                    <form onSubmit={handleSubmit} autoComplete="off">
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 required">
                                Дата и время начала <span className="text-muted me-2">(время по МСК)</span>
                            </label>
                            <DatePicker
                                showIcon
                                icon="bi bi-calendar"
                                selected={onboardingAccessTime}
                                onChange={setOnboardingAccessTime}
                                showTimeSelect
                                timeFormat="HH:mm"
                                timeIntervals={15}
                                dateFormat="dd.MM.yyyy HH:mm"
                                locale={ru}
                                className="form-control rounded-3"
                                placeholderText="Выберите дату и время"
                                minDate={new Date()}
                                minTime={
                                    onboardingAccessTime &&
                                    onboardingAccessTime.toDateString() === new Date().toDateString()
                                        ? new Date(new Date().setMinutes(new Date().getMinutes() + 1))
                                        : undefined
                                }
                                maxTime={
                                    onboardingAccessTime &&
                                    onboardingAccessTime.toDateString() === new Date().toDateString()
                                        ? new Date(new Date().setHours(23, 59, 0, 0))
                                        : undefined
                                }
                                autoComplete="off"
                                timeCaption="Время"
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
