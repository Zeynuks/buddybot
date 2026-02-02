import { useState } from "react";
import { useNavigate } from "react-router-dom";

type DashboardCardProps = {
    icon: string;
    label: string;
    to: string;
    iconBg: string;
    iconColor: string;
};

function DashboardCard({ icon, label, to, iconBg, iconColor }: DashboardCardProps) {
    const navigate = useNavigate();
    const [hovered, setHovered] = useState(false);

    return (
        <div
            className={`card text-center border-0 shadow-sm mb-2`}
            style={{
                borderRadius: 16,
                background: "#fff",
                minHeight: 170,
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                cursor: "pointer",
                transition: "transform 0.18s cubic-bezier(0.24, 0.88, 0.45, 1.18), box-shadow 0.16s",
                transform: hovered ? "translateY(-8px) scale(1.03)" : "none",
                zIndex: hovered ? 2 : "auto",
            }}
            onMouseEnter={() => setHovered(true)}
            onMouseLeave={() => setHovered(false)}
            onFocus={() => setHovered(true)}
            onBlur={() => setHovered(false)}
            onClick={() => navigate(to)}
            tabIndex={0}
        >
            <div className="card-body d-flex flex-column align-items-center justify-content-center p-4">
                <div
                    className="d-flex justify-content-center align-items-center mb-2 rounded-circle"
                    style={{
                        width: 80,
                        height: 80,
                        background: iconBg,
                    }}
                >
                    <i className={`bi ${icon} fs-2`} style={{ color: iconColor }}></i>
                </div>
                <div className="fw-medium">{label}</div>
            </div>
        </div>
    );
}

export default DashboardCard;
