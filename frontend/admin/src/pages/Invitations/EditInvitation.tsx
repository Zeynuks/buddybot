import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import type { InvitationDetail } from "../../models/Invitation";
import type { DepartmentLookup } from "../../models/Department";
import type { TeamLookup } from "../../models/Teams";
import { getInvitationByToken, updateInvitation } from "../../services/invitationService";
import { getDepartmentsLookup } from "../../services/departmentService";
import { getUsersLookup } from "../../services/userService";
import { getTeamsLookup } from "../../services/teamService";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import { ROUTES } from "../../constants/routes";
import type { UserLookup } from "../../models/User";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { ru } from "date-fns/locale";
import { parseTelegramUsername } from "../../utils/parseTelegramUsername";

export default function EditInvitation() {
    const { token } = useParams<{ token: string }>();
    const navigate = useNavigate();

    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [departmentId, setDepartmentId] = useState<number | undefined>();
    const [teamId, setTeamId] = useState<number | undefined>();
    const [expireDate, setExpireDate] = useState<Date | null>(null);
    const [telegramContact, setTelegramContact] = useState("");
    const [hrId, setHrId] = useState<number | undefined>();
    const [mentorId, setMentorId] = useState<number | undefined>();

    const [departments, setDepartments] = useState<DepartmentLookup[]>([]);
    const [teams, setTeams] = useState<TeamLookup[]>([]);
    const [hrs, setHrs] = useState<UserLookup[]>([]);
    const [mentors, setMentors] = useState<UserLookup[]>([]);

    const [initialLoading, setInitialLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [invitation, setInvitation] = useState<InvitationDetail>();

    useEffect(() => {
        let mounted = true;
        setInitialLoading(true);
        setError(null);

        (async () => {
            try {
                const [inv, departmentsList, hrsList] = await Promise.all([
                    getInvitationByToken(token!),
                    getDepartmentsLookup(),
                    getUsersLookup({ roles: ["HR"] }),
                ]);
                if (!mounted) return;

                setInvitation(inv);
                setDepartments(departmentsList);
                setHrs(hrsList);

                setFirstName(inv.user.firstName || "");
                setLastName(inv.user.lastName || "");
                setTelegramContact(parseTelegramUsername(inv.user.telegramContact || ""));
                setExpireDate(inv.expireDate ? new Date(inv.expireDate) : null);
                setHrId(inv.hRs?.[0]?.id);

                if (inv.department?.id) {
                    setDepartmentId(inv.department.id);
                    const teamsList = await getTeamsLookup(inv.department.id);
                    if (!mounted) return;
                    setTeams(teamsList);
                    setTeamId(inv.team?.id);
                    setMentorId(inv.mentors?.[0]?.id);
                } else {
                    setTeams([]);
                    setTeamId(undefined);
                    setMentorId(undefined);
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
    }, [token]);

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
        getUsersLookup({ roles: ["Mentor"], teamId })
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

        if (!expireDate) {
            setError("Выберите дату истечения ссылки");
            setSaving(false);
            return;
        }
        if (expireDate.getTime() <= Date.now()) {
            setError("Дата истечения ссылки не может быть в прошлом");
            setSaving(false);
            return;
        }

        try {
            const result = await updateInvitation(token!, {
                firstName,
                lastName,
                teamId,
                expireDate: expireDate ? expireDate.toISOString() : undefined,
                mentorIds: invitation?.userRole === "candidate" && mentorId ? [mentorId] : undefined,
                hrIds: invitation?.userRole === "candidate" && hrId ? [hrId] : undefined,
                telegramContact: isHR && telegramContact ? `https://t.me/${telegramContact.trim()}` : undefined,
            });
            navigate(ROUTES.INVITATIONS.DETAIL(result.tokenValue), { state: { toast: "Изменения успешно сохранены" } });
        } catch (e) {
            setError(e instanceof Error ? e.message : "Ошибка сохранения изменений");
        } finally {
            setSaving(false);
        }
    }

    const isCandidate = invitation?.userRole === "candidate";
    const isHR = invitation?.userRole === "hr";

    useEffect(() => {
        if (isCandidate) {
            if (teamId) {
                getUsersLookup({ roles: ["Mentor"], teamId: Number(teamId) })
                    .then(setMentors)
                    .catch(() => setMentors([]));
            } else {
                setMentors([]);
                setMentorId(undefined);
            }
        }
    }, [teamId, isCandidate]);

    return (
        <div className="mx-4 mt-4">
            {initialLoading ? (
                <LoadingSpinner />
            ) : !invitation ? (
                <div className="alert alert-danger py-2 small mb-3">Не удалось загрузить приглашение</div>
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
                    <div className="text-center fs-4 mb-4 mt-2">Редактирование приглашения</div>
                    <form onSubmit={handleSubmit} autoComplete="off">
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
                        {isHR && (
                            <div className="mb-3">
                                <label className="form-label fs-6 mb-1 required">Ссылка на Telegram</label>
                                {telegramContact && (
                                    <div className="small mt-1 text-secondary mb-1">
                                        Итоговая ссылка:{" "}
                                        <a
                                            href={`https://t.me/${telegramContact.trim()}`}
                                            target="_blank"
                                            rel="noopener noreferrer"
                                        >
                                            https://t.me/{telegramContact.trim()}
                                        </a>
                                    </div>
                                )}
                                <div className="input-group">
                                    <span className="input-group-text" id="telegram-addon">
                                        https://t.me/
                                    </span>
                                    <input
                                        type="text"
                                        className="form-control rounded-3"
                                        placeholder="username"
                                        aria-label="Telegram username"
                                        aria-describedby="telegram-addon"
                                        value={telegramContact}
                                        onChange={(e) =>
                                            setTelegramContact(e.target.value.replace(/^@/, "").replace(/\s/g, ""))
                                        }
                                        required
                                    />
                                </div>
                            </div>
                        )}

                        {isCandidate && (
                            <>
                                <div className="mb-3">
                                    <label className="form-label fs-6 mb-1 required"> Закреплённый HR</label>
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
                                <div className="mb-3">
                                    <label className="form-label fs-6 mb-1  required">Наставник</label>
                                    <select
                                        className="form-select rounded-3"
                                        value={mentorId ?? ""}
                                        onChange={(e) => setMentorId(Number(e.target.value) || undefined)}
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
                            </>
                        )}
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 required">Дата истечения ссылки</label>
                            <DatePicker
                                showIcon
                                icon="bi bi-calendar"
                                selected={expireDate}
                                onChange={setExpireDate}
                                showTimeSelect
                                timeFormat="HH:mm"
                                timeIntervals={15}
                                dateFormat="dd.MM.yyyy HH:mm"
                                locale={ru}
                                className="form-control rounded-3"
                                placeholderText="Выберите дату и время"
                                minDate={new Date()}
                                minTime={
                                    expireDate && expireDate.toDateString() === new Date().toDateString()
                                        ? new Date(new Date().setMinutes(new Date().getMinutes() + 1))
                                        : undefined
                                }
                                maxTime={
                                    expireDate && expireDate.toDateString() === new Date().toDateString()
                                        ? new Date(new Date().setHours(23, 59, 0, 0))
                                        : undefined
                                }
                                autoComplete="off"
                                timeCaption="Время"
                            />
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
