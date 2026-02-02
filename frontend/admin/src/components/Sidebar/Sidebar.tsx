import { NavLink } from "react-router-dom";
import { ROUTES } from "../../constants/routes";

type SidebarProps = {
    onHide?: () => void;
    className?: string;
};

const SIDEBAR_LINKS = [
    {
        to: ROUTES.DEPARTMENTS.ROOT,
        icon: "bi-diagram-3",
        label: "Отделы",
    },
    {
        to: ROUTES.TEAMS.ROOT,
        icon: "bi-people",
        label: "Команды",
    },
    {
        to: ROUTES.MEMBERS.ROOT,
        icon: "bi-person-badge-fill",
        label: "Участники",
    },
    {
        to: ROUTES.INVITATIONS.ROOT,
        icon: "bi-link-45deg",
        label: "Приглашения в бота",
    },
    {
        to: ROUTES.CANDIDATES.ROOT,
        icon: "bi-person",
        label: "Кандидаты",
    },
    {
        to: ROUTES.ONBOARDING_ACCESS_REQUESTS.ROOT,
        icon: "bi-clipboard-check",
        label: "Заявки на онбординг",
    },
    {
        to: ROUTES.FEEDBACKS.ROOT,
        icon: "bi-star",
        label: "Отзывы",
    },
    // {
    //     to: ROUTES.COUNTRIES.ROOT,
    //     icon: "bi-globe2",
    //     label: "Страны",
    // },
];

function Sidebar({ onHide, className = "" }: SidebarProps) {
    return (
        <nav className={`d-flex flex-column gap-2 py-4 px-2 ${className}`}>
            {SIDEBAR_LINKS.map((link) => (
                <NavLink
                    key={link.to}
                    to={link.to}
                    className="nav-link d-flex align-items-center gap-2"
                    onClick={onHide}
                >
                    <i className={`bi ${link.icon} fs-5`}></i>
                    <span>{link.label}</span>
                </NavLink>
            ))}
        </nav>
    );
}

export default Sidebar;
