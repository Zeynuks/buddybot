import { ROUTES } from "../../constants/routes";
import DashboardCard from "./DashboardCard";

const DASHBOARD_CARDS = [
    {
        icon: "bi-diagram-3",
        label: "Отделы",
        to: ROUTES.DEPARTMENTS.ROOT,
        iconBg: "#CFF4FC",
        iconColor: "#3DD5F3",
    },
    {
        icon: "bi-people",
        label: "Команды",
        to: ROUTES.TEAMS.ROOT,
        iconBg: "#FFE5D0",
        iconColor: "#FD9843",
    },
    {
        icon: "bi-person-badge-fill",
        label: "Участники",
        to: ROUTES.MEMBERS.ROOT,
        iconBg: "#FCE2DB",
        iconColor: "#FF6F61",
    },
    {
        icon: "bi-link-45deg",
        label: "Приглашения в бота",
        to: ROUTES.INVITATIONS.ROOT,
        iconBg: "#E2D9F3",
        iconColor: "#8C68CD",
    },
    {
        icon: "bi-person",
        label: "Кандидаты",
        to: ROUTES.CANDIDATES.ROOT,
        iconBg: "#CFE2FF",
        iconColor: "#3D8BFD",
    },
    {
        icon: "bi-clipboard-check",
        label: "Заявки на онбординг",
        to: ROUTES.ONBOARDING_ACCESS_REQUESTS.ROOT,
        iconBg: "#D1E7DD",
        iconColor: "#479F76",
    },
    {
        icon: "bi-star",
        label: "Отзывы",
        to: ROUTES.FEEDBACKS.ROOT,
        iconBg: "#E0CFFC",
        iconColor: "#8540F5",
    },
    // {
    //     icon: "bi-globe2",
    //     label: "Страны",
    //     to: ROUTES.COUNTRIES.ROOT,
    //     iconBg: "#FFF3CD",
    //     iconColor: "#FFD600",
    // },
];
function Dashboard() {
    return (
        <div>
            {/* <div className="text-center rounded-3 py-3 mb-4 mx-4" style={{ background: "#CFE2FF" }}>
                <span className="fw-medium text-primary">Активные заявки</span>
                <br />
                <span className="fs-4 fw-bold text-primary" style={{ cursor: "pointer" }}>
                    12
                </span>
            </div> */}
            <div className="row g-4 justify-content-center">
                {DASHBOARD_CARDS.map((card) => (
                    <div className="col-12 col-sm-6 col-lg-4 mx-4" key={card.label}>
                        <DashboardCard {...card} />
                    </div>
                ))}
            </div>
        </div>
    );
}

export default Dashboard;
