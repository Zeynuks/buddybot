import type { DepartmentLookup } from "../../models/Department";
import type { TeamLookup } from "../../models/Teams";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getUserById, updateMember } from "../../services/userService";
import { getDepartmentsLookup } from "../../services/departmentService";
import { getTeamsLookup } from "../../services/teamService";
import { ROUTES } from "../../constants/routes";
import { uploadMedia } from "../../services/mediaService";
import { HelpTooltip } from "../../components/HelpTooltip/HelpTooltip";
import { PhotoUploader } from "../../components/PhotoUploader/PhotoUploader";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import { parseTelegramUsername } from "../../utils/parseTelegramUsername";

export default function EditMember() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();

    const [departmentId, setDepartmentId] = useState<number | undefined>();
    const [teamId, setTeamId] = useState<number | undefined>();
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [microsoftTeamsUrl, setMicrosoftTeamsUrl] = useState("");
    const [photoId, setPhotoId] = useState<string | null>(null);
    const [telegramContact, setTelegramContact] = useState("");

    const [departments, setDepartments] = useState<DepartmentLookup[]>([]);
    const [teams, setTeams] = useState<TeamLookup[]>([]);

    const [initialLoading, setInitialLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [photoUploading, setPhotoUploading] = useState(false);
    const [photoUploadError, setPhotoUploadError] = useState<string | null>(null);

    const [roles, setRoles] = useState<string[]>([]);

    useEffect(() => {
        let mounted = true;
        setInitialLoading(true);

        (async () => {
            try {
                const [member, departmentsList] = await Promise.all([getUserById(Number(id)), getDepartmentsLookup()]);

                if (!mounted) return;

                setRoles(member.roles || []);
                setDepartments(departmentsList);
                setTelegramContact(parseTelegramUsername(member.telegramContact || ""));
                setFirstName(member.firstName || "");
                setLastName(member.lastName || "");
                setMicrosoftTeamsUrl(member.microsoftTeamsUrl || "");
                setPhotoId(member.photoUrl || null);

                if (member.team?.departmentId) {
                    setDepartmentId(member.team.departmentId);
                    const teamsList = await getTeamsLookup(member.team.departmentId);
                    if (!mounted) return;
                    setTeams(teamsList);
                    setTeamId(member.team.id);
                } else {
                    setDepartmentId(undefined);
                    setTeams([]);
                    setTeamId(undefined);
                }
            } catch {
                setError("Не удалось загрузить данные участника");
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
        try {
            await updateMember(Number(id), {
                firstName,
                lastName,
                teamId,
                microsoftTeamsUrl,
                photoUrl: photoId || null,
                telegramContact: telegramContact ? `https://t.me/${telegramContact.trim()}` : undefined,
            });
            navigate(ROUTES.MEMBERS.ROOT, { state: { toast: "Изменения успешно сохранены" } });
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
                    <div className="text-center fs-4 mb-4 mt-2">Редактирование участника</div>
                    <form onSubmit={handleSubmit}>
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
                        {roles.includes("HR") && (
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
                            <label className="form-label fs-6 mb-1">Фотография участника</label>
                            <PhotoUploader
                                photoId={photoId ?? ""}
                                setPhotoId={(id) => setPhotoId(id || null)}
                                photoUploading={photoUploading}
                                setPhotoUploading={setPhotoUploading}
                                photoUploadError={photoUploadError}
                                setPhotoUploadError={setPhotoUploadError}
                                onUpload={uploadMedia}
                            />
                        </div>

                        {error && <div className="alert alert-danger py-2 small mb-3">{error}</div>}
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
