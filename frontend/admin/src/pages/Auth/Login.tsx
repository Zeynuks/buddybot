import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "../../constants/routes";
import Logo from "../../assets/logo.svg";
import useAuth from "../../hooks/useAuth";

export default function Login() {
    const navigate = useNavigate();
    const { login } = useAuth();

    const [loginValue, setLoginValue] = useState("");
    const [password, setPassword] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setError(null);
        setLoading(true);

        const ok = await login(loginValue, password);
        setLoading(false);

        if (ok) {
            navigate(ROUTES.DASHBOARD);
        } else {
            setError("Ошибка авторизации");
        }
    }

    return (
        <form onSubmit={handleSubmit} className="w-100" style={{ maxWidth: 500 }}>
            <div className="text-center mb-3">
                <img src={Logo} alt="BuddyBot" width={130} height={35} className="mb-2" />
            </div>
            <div className="mb-3">
                <label htmlFor="login" className="form-label">
                    Логин
                </label>
                <input
                    id="login"
                    type="text"
                    className="form-control"
                    placeholder="Введите логин"
                    value={loginValue}
                    onChange={(e) => setLoginValue(e.target.value)}
                    autoFocus
                    required
                />
            </div>
            <div className="mb-4">
                <label htmlFor="password" className="form-label">
                    Пароль
                </label>
                <input
                    id="password"
                    type="password"
                    className="form-control"
                    placeholder="Введите пароль"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
            </div>
            {error && (
                <div className="alert alert-danger py-2 small mb-3" role="alert">
                    {error}
                </div>
            )}
            <button type="submit" className="btn btn-primary w-100 rounded-3 fw-semibold py-2 fs-5" disabled={loading}>
                {loading ? "Вход..." : "Войти"}
            </button>
        </form>
    );
}
