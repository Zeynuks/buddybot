import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import type { TeamLookup } from "../../models/Teams";
import { getTeamsLookup } from "../../services/teamService";
import { createHRInvitation } from "../../services/invitationService";
import type { DepartmentLookup } from "../../models/Department";
import { getDepartmentsLookup } from "../../services/departmentService";
import { ROUTES } from "../../constants/routes";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import { HelpTooltip } from "../../components/HelpTooltip/HelpTooltip";
import useAuth from "../../hooks/useAuth";

export default function CreateInvitationHR() {
    const { user } = useAuth();
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [telegramContact, setTelegramContact] = useState("");
    const [microsoftTeamsUrl, setMicrosoftTeamsUrl] = useState("");
    const [departmentId, setDepartmentId] = useState<number | "">("");
    const [teamId, setTeamId] = useState<number | "">("");
    const [expirationDays, setExpirationDays] = useState(1);

    const [departments, setDepartments] = useState<DepartmentLookup[]>([]);
    const [teams, setTeams] = useState<TeamLookup[]>([]);

    const [initialLoading, setInitialLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [showPassword, setShowPassword] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        let mounted = true;
        async function fetchLookups() {
            setInitialLoading(true);
            setError(null);
            try {
                const deps = await getDepartmentsLookup();
                if (!mounted) return;
                setDepartments(deps);
            } catch {
                if (mounted) setError("Ошибка загрузки справочников");
            } finally {
                if (mounted) setInitialLoading(false);
            }
        }
        fetchLookups();
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
    }, [departmentId]);

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setSaving(true);
        setError(null);
        try {
            if (!user) {
                throw new Error("Не удалось определить пользователя");
            }
            const result = await createHRInvitation({
                login,
                password,
                firstName,
                lastName,
                telegramContact: `https://t.me/${telegramContact.trim()}`,
                microsoftTeamsUrl: microsoftTeamsUrl.trim() || undefined,
                teamId: Number(teamId),
                expirationDays,
                creatorId: user.id,
            });
            navigate(ROUTES.INVITATIONS.DETAIL(result.tokenValue), {
                state: { toast: "Приглашение успешно создано" },
            });
        } catch (e) {
            if (e instanceof Error) setError(e.message ?? "Ошибка при создании приглашения для HR");
            else setError("Ошибка при создании приглашения для HR");
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
                    <div className="text-center fs-4 mb-4 mt-2">Создание приглашения для HR</div>
                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 required">Имя HR</label>
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
                            <label className="form-label fs-6 mb-1 required">Фамилия HR</label>
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
                            <label className="form-label fs-6 mb-1 required">Логин</label>
                            <input
                                type="text"
                                className="form-control rounded-3"
                                placeholder="Введите логин"
                                value={login}
                                onChange={(e) => setLogin(e.target.value)}
                                required
                                autoComplete="username"
                                minLength={3}
                                maxLength={100}
                            />
                        </div>
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 required">Пароль</label>
                            <div className="input-group">
                                <input
                                    type={showPassword ? "text" : "password"}
                                    className="form-control rounded-3"
                                    placeholder="Введите пароль"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                    autoComplete="new-password"
                                    minLength={6}
                                    maxLength={100}
                                />
                                <button
                                    type="button"
                                    className="btn btn-outline-secondary rounded-3"
                                    style={{ border: "1px solid #ced4da" }}
                                    tabIndex={-1}
                                    aria-label={showPassword ? "Скрыть пароль" : "Показать пароль"}
                                    onClick={() => setShowPassword((v) => !v)}
                                >
                                    {showPassword ? <i className="bi bi-eye-slash"></i> : <i className="bi bi-eye"></i>}
                                </button>
                            </div>
                            <div className="form-text text-secondary mt-1">
                                Пароль будет доступен только на этом экране. Пожалуйста, сохраните его заранее.
                            </div>
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
                                    onChange={(e) => setTelegramContact(e.target.value)}
                                    required
                                />
                            </div>
                        </div>

                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 d-flex align-items-center position-relative">
                                Ссылка в Microsoft Teams
                                <HelpTooltip>
                                    <div className="mb-2">
                                        Чтобы получить ссылку, откройте Teams в браузере и перейдите в чат с нужным
                                        человеком.
                                        <br />
                                        Скопируйте адрес страницы из адресной строки и вставьте его в это поле.
                                    </div>
                                </HelpTooltip>
                            </label>
                            <input
                                type="url"
                                className="form-control rounded-3"
                                placeholder="https://teams.microsoft.com/l/chat/0/0?users=username@travelline.ru"
                                value={microsoftTeamsUrl}
                                onChange={(e) => setMicrosoftTeamsUrl(e.target.value)}
                                maxLength={255}
                            />
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
                                        {d === 3650
                                            ? "Без срока"
                                            : `${d} ${d === 1 ? "день" : d === 7 ? "дней" : "дней"}`}
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
