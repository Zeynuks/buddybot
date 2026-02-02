import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "../../constants/routes";
import { createDepartment } from "../../services/departmentService";
import { HelpTooltip } from "../../components/HelpTooltip/HelpTooltip";

export default function CreateDepartment() {
    const navigate = useNavigate();

    const [name, setName] = useState("");
    const [headFirstName, setHeadFirstName] = useState("");
    const [headLastName, setHeadLastName] = useState("");
    const [headMicrosoftTeamsUrl, setHeadMicrosoftTeamsUrl] = useState("");

    const [saving, setSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setError(null);

        setSaving(true);
        try {
            await createDepartment({
                name,
                headFirstName,
                headLastName,
                headMicrosoftTeamsUrl,
            });
            navigate(ROUTES.DEPARTMENTS.ROOT, { state: { toast: "Отдел успешно создан" } });
        } catch (e) {
            setError(e instanceof Error ? e.message : "Ошибка создания отдела");
        } finally {
            setSaving(false);
        }
    }

    return (
        <div className="mx-4 mt-4">
            <div className="card border-0 shadow-sm rounded-4 p-4 mb-4 mx-auto">
                <button
                    className="btn btn-link text-primary px-0 mb-2 d-flex align-items-center text-decoration-none"
                    type="button"
                    onClick={() => navigate(-1)}
                >
                    <i className="bi bi-chevron-left small align-middle"></i>
                    <span className="ms-1">Назад</span>
                </button>
                <div className="text-center fs-4 mb-4 mt-2">Создание отдела</div>
                <form onSubmit={handleSubmit}>
                    <div className="mb-3">
                        <label className="form-label fs-6 mb-1 required">Название отдела</label>
                        <input
                            type="text"
                            className="form-control rounded-3"
                            placeholder="Введите название отдела"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            required
                            minLength={2}
                            maxLength={100}
                        />
                    </div>
                    <div className="mb-3">
                        <label className="form-label fs-6 mb-1 required">Имя руководителя</label>
                        <input
                            type="text"
                            className="form-control rounded-3"
                            placeholder="Введите имя руководителя"
                            value={headFirstName}
                            onChange={(e) => setHeadFirstName(e.target.value)}
                            required
                            minLength={2}
                            maxLength={50}
                        />
                    </div>
                    <div className="mb-3">
                        <label className="form-label fs-6 mb-1 required">Фамилия руководителя</label>
                        <input
                            type="text"
                            className="form-control rounded-3"
                            placeholder="Введите фамилию руководителя"
                            value={headLastName}
                            onChange={(e) => setHeadLastName(e.target.value)}
                            required
                            minLength={2}
                            maxLength={50}
                        />
                    </div>
                    <div className="mb-3">
                        <label className="form-label fs-6 mb-1 d-flex align-items-center position-relative">
                            Ссылка руководителя в Microsoft Teams
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
                            value={headMicrosoftTeamsUrl}
                            onChange={(e) => setHeadMicrosoftTeamsUrl(e.target.value)}
                            autoComplete="off"
                            maxLength={200}
                        />
                    </div>

                    {error && <div className="alert alert-danger py-2 small mb-3">{error}</div>}
                    <button
                        type="submit"
                        className="btn btn-primary w-100 py-2 rounded-3 fw-semibold"
                        disabled={saving}
                    >
                        {saving ? "Создание..." : "Создать отдел"}
                    </button>
                </form>
            </div>
        </div>
    );
}
