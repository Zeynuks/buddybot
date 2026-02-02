import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { createCountry } from "../../services/countryService";
import { ROUTES } from "../../constants/routes";

export default function CreateCountry() {
    const [name, setName] = useState("");
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setSaving(true);
        setError(null);
        try {
            await createCountry({ name });
            navigate(ROUTES.COUNTRIES.ROOT);
        } catch (e) {
            setError(e instanceof Error ? e.message : "Ошибка добавления страны");
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
                <div className="text-center fs-4 mb-4 mt-2">Добавление страны</div>
                <form onSubmit={handleSubmit}>
                    <div className="mb-4">
                        <label className="form-label fs-6 mb-1">Название страны</label>
                        <input
                            type="text"
                            className="form-control rounded-3"
                            placeholder="Введите название страны"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            required
                        />
                    </div>
                    {error && <div className="alert alert-danger py-2 small mb-3">{error}</div>}
                    <button
                        type="submit"
                        className="btn btn-primary w-100 py-2 rounded-3 fw-semibold"
                        disabled={saving}
                    >
                        {saving ? "Добавление..." : "Добавить страну"}
                    </button>
                </form>
            </div>
        </div>
    );
}
