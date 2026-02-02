import { Link } from "react-router-dom";
import { ROUTES } from "../../constants/routes";
import type { TeamList } from "../../models/Teams";
import useAuth from "../../hooks/useAuth";
import { hasPermission } from "../../utils/hasPermission";
import { PermissionName } from "../../constants/permissions";

export default function TeamCard({ team, onDelete }: { team: TeamList; onDelete: (team: TeamList) => void }) {
    const { permissions } = useAuth();
    const leader =
        team.leaderFirstName || team.leaderLastName
            ? `${team.leaderFirstName ?? ""} ${team.leaderLastName ?? ""}`.trim()
            : null;
    return (
        <div className="card border-0 shadow-sm rounded-4 px-3 py-3 position-relative">
            <div className="position-absolute top-0 end-0 m-2">
                {hasPermission(permissions, PermissionName.TeamUpdate) && (
                    <Link
                        to={ROUTES.TEAMS.EDIT(team.id)}
                        className="btn btn-link text-primary p-0"
                        title="Редактировать"
                    >
                        <i className="bi bi-pencil-square fs-5"></i>
                    </Link>
                )}
                {hasPermission(permissions, PermissionName.TeamDelete) && (
                    <button
                        className="btn btn-link text-danger p-0 ms-4"
                        title="Удалить"
                        onClick={() => onDelete(team)}
                    >
                        <i className="bi bi-trash fs-5"></i>
                    </button>
                )}
            </div>
            <div>
                <div className="fw-semibold mb-1">{team.name}</div>
                <div className="small text-secondary mb-1">
                    <i className="bi bi-person-badge me-1"></i>
                    Руководитель:{" "}
                    {leader ? (
                        <span className="fw-semibold">{leader}</span>
                    ) : (
                        <span className="text-danger">Отсутствует</span>
                    )}
                </div>
                <div className="d-flex gap-2 mt-1">
                    <span className="badge bg-primary bg-opacity-10 text-primary fw-normal">
                        {team.memberCount} участник{team.memberCount !== 1 ? "а" : ""}
                    </span>
                    <span className="badge bg-secondary bg-opacity-10 text-secondary fw-normal">
                        {team.departmentName}
                    </span>
                </div>
                <div className="mt-2 text-end">
                    <Link
                        to={`${ROUTES.MEMBERS.ROOT}?teamId=${team.id}&departmentId=${team.departmentId}`}
                        className="small text-primary fw-semibold text-decoration-none"
                    >
                        Участники <i className="bi bi-chevron-right small align-middle"></i>
                    </Link>
                </div>
            </div>
        </div>
    );
}
