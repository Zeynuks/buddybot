import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { createCity } from "../../services/cityService";

export default function CreateCity() {
    const { countryId } = useParams<{ countryId: string }>();
    const [name, setName] = useState("");
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        if (!countryId) {
            setError("Не определена страна");
            return;
        }
        setSaving(true);
        setError(null);
        try {
            await createCity({ name, countryId: Number(countryId) });
            navigate(-1);
        } catch (e) {
            setError(e instanceof Error ? e.message : "Ошибка добавления города");
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
                <div className="text-center fs-4 mb-4 mt-2">Добавление города</div>
                <form onSubmit={handleSubmit}>
                    <div className="mb-4">
                        <label className="form-label fs-6 mb-1">Название города</label>
                        <input
                            type="text"
                            className="form-control rounded-3"
                            placeholder="Введите название города"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            required
                        />
                    </div>
                    {error && <div className="alert alert-danger">{error}</div>}
                    <button
                        type="submit"
                        className="btn btn-primary w-100 py-2 rounded-3 fw-semibold"
                        disabled={saving}
                    >
                        {saving ? "Добавление..." : "Добавить город"}
                    </button>
                </form>
            </div>
        </div>
    );
}
