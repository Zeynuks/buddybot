import { Link, useNavigate } from "react-router-dom";
import Logo from "../../assets/logo.svg";
import useAuth from "../../hooks/useAuth";
import { useEffect, useRef, useState } from "react";

type HeaderProps = {
    userName: string;
    isMobile: boolean;
};

function Header({ userName, isMobile }: HeaderProps) {
    const { logout } = useAuth();
    const [menuOpen, setMenuOpen] = useState(false);
    const menuRef = useRef<HTMLDivElement>(null);
    const navigate = useNavigate();

    useEffect(() => {
        function handleClickOutside(event: MouseEvent) {
            if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
                setMenuOpen(false);
            }
        }
        if (menuOpen) {
            document.addEventListener("mousedown", handleClickOutside);
        }
        return () => document.removeEventListener("mousedown", handleClickOutside);
    }, [menuOpen]);

    const handleLogout = () => {
        logout();
        navigate("/login", { replace: true });
    };

    return (
        <header
            className="d-flex align-items-center justify-content-between px-3 py-2 border-bottom bg-white"
            style={{ minHeight: 56 }}
        >
            <div className="d-flex align-items-center gap-2">
                {isMobile && (
                    <button
                        className="btn btn-link p-0 me-2 d-lg-none"
                        style={{ fontSize: 24 }}
                        aria-label="Открыть меню"
                        type="button"
                        data-bs-toggle="offcanvas"
                        data-bs-target="#sidebarOffcanvas"
                    >
                        <i className="bi bi-list text-secondary"></i>
                    </button>
                )}
                {!isMobile && (
                    <Link to="/" className="d-flex align-items-center">
                        <img
                            src={Logo}
                            alt="BuddyBot"
                            height={34}
                            style={{
                                width: "auto",
                                maxWidth: 130,
                                display: "block",
                            }}
                        />
                    </Link>
                )}
            </div>
            {isMobile ? (
                <div className="flex-grow-1 d-flex justify-content-center align-items-center">
                    <Link to="/" className="d-flex align-items-center">
                        <img
                            src={Logo}
                            alt="BuddyBot"
                            height={32}
                            style={{
                                width: "auto",
                                maxWidth: 120,
                                display: "block",
                            }}
                        />
                    </Link>
                </div>
            ) : (
                <div className="flex-grow-1"></div>
            )}
            <div
                className="fw-medium ms-3 d-flex align-items-center gap-3"
                ref={menuRef}
                style={{ position: "relative" }}
            >
                <button
                    className="btn btn-link fw-medium px-2 py-1 text-dark"
                    style={{ textDecoration: "none" }}
                    onClick={() => setMenuOpen((open) => !open)}
                >
                    <i className="bi bi-person-circle me-2"></i>
                    {userName}
                    <i className={`bi ms-2 ${menuOpen ? "bi-chevron-up" : "bi-chevron-down"}`}></i>
                </button>
                {menuOpen && (
                    <div
                        className="dropdown-menu show"
                        style={{
                            minWidth: 180,
                            boxShadow: "0 2px 16px rgba(0,0,0,0.12)",
                            left: 0,
                            right: "auto",
                            top: "100%",
                            marginTop: 8,
                            zIndex: 1000,
                            position: "absolute",
                        }}
                    >
                        <button className="dropdown-item text-danger" onClick={handleLogout}>
                            <i className="bi bi-box-arrow-right me-2"></i>Выйти
                        </button>
                    </div>
                )}
            </div>
        </header>
    );
}

export default Header;
