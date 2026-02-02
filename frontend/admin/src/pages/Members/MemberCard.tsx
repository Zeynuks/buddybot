import { Link } from "react-router-dom";
import { ROUTES } from "../../constants/routes";
import type { UserDetail } from "../../models/User";
import { HelpTooltip } from "../../components/HelpTooltip/HelpTooltip";

export default function MemberCard({ member, onDelete }: { member: UserDetail; onDelete: (dep: UserDetail) => void }) {
    const departmentName = member.team?.departmentName;
    const teamName = member.team?.name;
    const isInactiveHR = member.roles.includes("HR") && !member.isActivated;

    return (
        <div className="card border-0 shadow-sm rounded-4 px-3 py-3 position-relative">
            <div className="position-absolute top-0 end-0 m-2">
                <Link
                    to={ROUTES.MEMBERS.EDIT(member.id)}
                    className="btn btn-link text-primary p-0"
                    title="Редактировать"
                >
                    <i className="bi bi-pencil-square fs-5"></i>
                </Link>
                {isInactiveHR && onDelete && (
                    <button
                        className="btn btn-link text-danger p-0 ms-4"
                        title="Удалить участника"
                        onClick={() => onDelete(member)}
                    >
                        <i className="bi bi-trash fs-5"></i>
                    </button>
                )}
            </div>
            <div>
                <div className="d-flex align-items-center mb-1">
                    <span>
                        {member.firstName} {member.lastName}
                    </span>
                    <span>
                        {member.roles.map((role) => (
                            <span key={role} className="badge bg-info bg-opacity-10 text-info fw-normal ms-2">
                                {role === "Mentor" ? "Наставник" : role}
                            </span>
                        ))}
                    </span>
                    {member.isTeamLeader && (
                        <HelpTooltip
                            ariaLabel="Руководитель команды"
                            iconClassName="bi bi-person-badge-fill ms-2 text-success"
                        >
                            <span className="fw-semibold">Руководитель команды</span>
                            <div className="mt-1 small text-secondary">
                                Этот сотрудник является руководителем своей команды
                            </div>
                        </HelpTooltip>
                    )}
                    {isInactiveHR && (
                        <HelpTooltip
                            ariaLabel="HR ещё не активирован"
                            iconClassName="bi bi-exclamation-circle-fill ms-2 text-warning"
                        >
                            <span className="fw-semibold">HR ещё не активирован</span>
                            <div className="mt-1 small text-secondary">
                                Пользователь ещё не начал работу с Telegram-ботом. Активация произойдёт автоматически,
                                как только он нажмёт кнопку <b>«Старт»</b> по ссылке из приглашения.
                            </div>
                        </HelpTooltip>
                    )}
                </div>

                <div className="small text-secondary mb-1">
                    <i className="bi bi-image me-1"></i>
                    Фото участника:{" "}
                    {member.photoUrl ? (
                        <span className="badge bg-success bg-opacity-25 text-success fw-normal">Загружено</span>
                    ) : (
                        <span className="badge bg-warning bg-opacity-25 text-warning fw-normal">Не загружено</span>
                    )}
                </div>
                <div className="d-flex flex-column gap-1 mt-1">
                    <div className="d-flex gap-2 flex-wrap">
                        {departmentName && (
                            <span className="badge bg-primary bg-opacity-10 text-primary fw-normal">
                                {departmentName}
                            </span>
                        )}
                        {teamName && (
                            <span className="badge bg-secondary bg-opacity-10 text-secondary fw-normal">
                                {teamName}
                            </span>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}
