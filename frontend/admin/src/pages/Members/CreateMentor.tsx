import { useEffect, useState } from "react";
import type { TeamLookup } from "../../models/Teams";
import { useNavigate } from "react-router-dom";
import { getTeamsLookup } from "../../services/teamService";
import { ROUTES } from "../../constants/routes";
import { createMentor } from "../../services/userService";
import type { DepartmentLookup } from "../../models/Department";
import { getDepartmentsLookup } from "../../services/departmentService";
import { uploadMedia } from "../../services/mediaService";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import { HelpTooltip } from "../../components/HelpTooltip/HelpTooltip";
import { PhotoUploader } from "../../components/PhotoUploader/PhotoUploader";

export default function CreateMentor() {
    const navigate = useNavigate();

    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [microsoftTeamsUrl, setMicrosoftTeamsUrl] = useState("");
    const [mentorPhotoId, setMentorPhotoId] = useState("");
    const [departmentId, setDepartmentId] = useState<number | "">("");
    const [teamId, setTeamId] = useState<number | "">("");
    const [isTeamLeader, setIsTeamLeader] = useState(false);

    const [departments, setDepartments] = useState<DepartmentLookup[]>([]);
    const [teams, setTeams] = useState<TeamLookup[]>([]);

    const [initialLoading, setInitialLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [photoUploading, setPhotoUploading] = useState(false);
    const [photoUploadError, setPhotoUploadError] = useState<string | null>(null);

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
            await createMentor({
                firstName,
                lastName,
                mentorPhotoUrl: mentorPhotoId,
                microsoftTeamsUrl,
                teamId: Number(teamId),
                isTeamLeader,
            });
            navigate(ROUTES.MEMBERS.ROOT, { state: { toast: "Наставник успешно создан" } });
        } catch (e) {
            if (e instanceof Error) setError(e.message ?? "Ошибка при создании наставника");
            else setError("Ошибка при создании наставника");
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
                    <div className="text-center fs-4 mb-4 mt-2">Создание наставника</div>
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
                        <div className="form-check mb-3 d-flex align-items-start">
                            <input
                                type="checkbox"
                                className="form-check-input mt-1"
                                id="isTeamLeader"
                                checked={isTeamLeader}
                                onChange={(e) => setIsTeamLeader(e.target.checked)}
                            />
                            <label className="form-check-label ms-2" htmlFor="isTeamLeader">
                                Назначить руководителем команды
                            </label>
                            <HelpTooltip>
                                Если в команде уже есть назначенный руководитель, он будет автоматически заменён.
                                <br />
                                Убедитесь, что вы действительно хотите изменить текущего руководителя команды.
                            </HelpTooltip>
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
                            <label className="form-label fs-6 mb-1">Фотография наставника</label>
                            <PhotoUploader
                                photoId={mentorPhotoId}
                                setPhotoId={setMentorPhotoId}
                                photoUploading={photoUploading}
                                setPhotoUploading={setPhotoUploading}
                                photoUploadError={photoUploadError}
                                setPhotoUploadError={setPhotoUploadError}
                                onUpload={uploadMedia}
                            />
                        </div>
                        {error && <div className="alert alert-danger">{error}</div>}
                        <button
                            type="submit"
                            className="btn btn-primary w-100 py-2 rounded-3 fw-semibold"
                            disabled={saving}
                        >
                            {saving ? "Сохраняем..." : "Создать наставника"}
                        </button>
                    </form>
                </div>
            )}
        </div>
    );
}
