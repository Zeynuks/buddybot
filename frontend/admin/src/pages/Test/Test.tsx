import { useEffect, useState } from "react";

export default function TestPage() {
    const [accessToken, setAccessToken] = useState<string | null>(null);
    const [refreshToken, setRefreshToken] = useState<string | null>(null);
    const [payload, setPayload] = useState<Record<string, unknown> | null>(null);

    useEffect(() => {
        const at = localStorage.getItem("AccessToken");
        setAccessToken(at);

        if (at) {
            try {
                const payloadBase64 = at.split(".")[1];
                const payloadJson = at && JSON.parse(atob(payloadBase64));
                setPayload(payloadJson);
            } catch {
                setPayload(null);
            }
        }

        const matches = document.cookie.match(/(^|; )RefreshToken=([^;]*)/);
        setRefreshToken(matches ? decodeURIComponent(matches[2]) : null);
    }, []);

    return (
        <div className="container mt-4">
            <h3 className="mb-3">Данные авторизации (тест)</h3>
            <div className="mb-3">
                <strong>AccessToken (localStorage):</strong>
                <pre className="bg-light rounded-3 p-2">{accessToken || "—"}</pre>
            </div>
            <div className="mb-3">
                <strong>AccessToken payload (JWT):</strong>
                <pre className="bg-light rounded-3 p-2">{payload ? JSON.stringify(payload, null, 2) : "—"}</pre>
            </div>
            <div className="mb-3">
                <strong>RefreshToken (cookie):</strong>
                <pre className="bg-light rounded-3 p-2">{refreshToken || "—"}</pre>
            </div>
        </div>
    );
}
