import { Link } from "react-router-dom";
import { ROUTES } from "../../constants/routes";
import type { DepartmentList } from "../../models/Department";
import useAuth from "../../hooks/useAuth";
import { hasPermission } from "../../utils/hasPermission";
import { PermissionName } from "../../constants/permissions";

export default function DepartmentCard({
    department,
    onDelete,
}: {
    department: DepartmentList;
    onDelete: (dep: DepartmentList) => void;
}) {
    const { permissions } = useAuth();

    function pluralize(n: number) {
        if (n % 10 === 1 && n % 100 !== 11) return "команда";
        if ([2, 3, 4].includes(n % 10) && ![12, 13, 14].includes(n % 100)) return "команды";
        return "команд";
    }

    return (
        <div className="card border-0 shadow-sm rounded-4 px-3 py-3 position-relative">
            <div className="position-absolute top-0 end-0 m-2">
                {hasPermission(permissions, PermissionName.DepartmentUpdate) && (
                    <Link
                        to={ROUTES.DEPARTMENTS.EDIT(department.id)}
                        className="btn btn-link text-primary p-0"
                        title="Редактировать"
                        tabIndex={0}
                    >
                        <i className="bi bi-pencil-square fs-5"></i>
                    </Link>
                )}
                {hasPermission(permissions, PermissionName.DepartmentDelete) && (
                    <button
                        className="btn btn-link text-danger p-0 ms-4"
                        title="Удалить"
                        tabIndex={0}
                        onClick={() => onDelete(department)}
                    >
                        <i className="bi bi-trash fs-5"></i>
                    </button>
                )}
            </div>
            <div className="d-flex flex-column min-vh-0">
                <div>
                    <div className="fw-semibold mb-1">{department.name}</div>
                    <div className="small text-secondary mb-1">
                        <i className="bi bi-person-badge me-1"></i>
                        Руководитель:{" "}
                        <span className="fw-semibold">
                            {department.headFirstName} {department.headLastName}
                        </span>
                    </div>
                    <div>
                        <span className="badge bg-primary bg-opacity-10 text-primary fw-normal me-2">
                            {department.teamCount ?? 0} {pluralize(department.teamCount ?? 0)}
                        </span>
                    </div>
                </div>
                <div className="mt-auto d-flex justify-content-end align-items-end pt-2">
                    <Link
                        to={`${ROUTES.TEAMS.ROOT}?departmentId=${department.id}`}
                        className="small text-decoration-none text-primary fw-semibold"
                    >
                        Команды <i className="bi bi-chevron-right small align-middle"></i>
                    </Link>
                </div>
            </div>
        </div>
    );
}
