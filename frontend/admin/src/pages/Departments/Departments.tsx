import type { DepartmentList } from "../../models/Department";
import { useEffect, useState } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { ROUTES } from "../../constants/routes";
import { getDepartments } from "../../services/departmentService";
import { Toast, ToastContainer } from "react-bootstrap";
import { useToastFromLocation } from "../../hooks/useToastFromLocation";
import DepartmentCard from "./DepartmentCard";
import SearchInput from "../../components/SearchInput/SearchInput";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import EmptyList from "../../components/EmptyList/EmptyList";
import DeleteDepartmentModal from "./DeleteDepartmentModal";
import { deleteDepartment } from "../../services/departmentService";
import useAuth from "../../hooks/useAuth";
import { PermissionName } from "../../constants/permissions";
import { hasPermission } from "../../utils/hasPermission";

export default function Departments() {
    const { permissions } = useAuth();
    const [searchParams, setSearchParams] = useSearchParams();
    const [departments, setDepartments] = useState<DepartmentList[]>([]);

    const search = searchParams.get("search") || "";
    const page = Number(searchParams.get("page") || 1);
    const [totalCount, setTotalCount] = useState(0);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [deletingDepartment, setDeletingDepartment] = useState<DepartmentList | null>(null);

    const [showToast, setShowToast] = useState(false);
    const [toastMessage, setToastMessage] = useState("");
    useToastFromLocation(setToastMessage, setShowToast);

    useEffect(() => {
        setLoading(true);
        setError(null);

        const pagesToLoad = Array.from({ length: page }, (_, i) => i + 1);

        Promise.all(pagesToLoad.map((p) => getDepartments(search, p)))
            .then((results) => {
                const allItems = results.flatMap((res) => res.items);
                const lastTotalCount = results.length > 0 ? results[results.length - 1].totalCount : 0;
                setDepartments(allItems);
                setTotalCount(lastTotalCount);
            })
            .catch((error) => setError(error instanceof Error ? error.message : "Не удалось загрузить отделы"))
            .finally(() => setLoading(false));
    }, [search, page]);

    useEffect(() => {
        setDepartments([]);
        setTotalCount(0);
    }, [search]);

    const handleSearch = (val: string) => {
        setSearchParams(
            (params) => {
                if (val) {
                    params.set("search", val);
                } else {
                    params.delete("search");
                }
                params.set("page", "1");
                return params;
            },
            { replace: true }
        );
    };

    const handleShowMore = () => {
        setSearchParams((params) => {
            const nextPage = Number(params.get("page") || 1) + 1;
            params.set("page", nextPage.toString());
            return params;
        });
    };

    const handleDelete = async () => {
        if (!deletingDepartment) return;
        try {
            await deleteDepartment(deletingDepartment.id);
            setDepartments((prev) => prev.filter((d) => d.id !== deletingDepartment.id));
            setTotalCount((prev) => prev - 1);
            setToastMessage("Отдел успешно удалён");
            setShowToast(true);
        } catch {
            alert("Не удалось удалить отдел");
        } finally {
            setDeletingDepartment(null);
        }
    };

    const noMore = departments.length >= totalCount;

    return (
        <div className="mx-4">
            <h5 className="text-center fw-semibold mb-3 mt-2">Отделы</h5>
            <div className="mb-3">
                {hasPermission(permissions, PermissionName.DepartmentCreate) && (
                    <Link
                        to={ROUTES.DEPARTMENTS.CREATE}
                        className="btn btn-primary w-100 fw-medium py-2 mb-3 rounded-pill"
                    >
                        + Добавить отдел
                    </Link>
                )}
                <SearchInput placeholder="Поиск отдела…" value={search} onChange={handleSearch} />
            </div>

            {loading && departments.length === 0 ? (
                <LoadingSpinner />
            ) : error ? (
                <div className="alert alert-danger">{error}</div>
            ) : (
                <div className="d-flex flex-column gap-3">
                    {departments.length > 0 ? (
                        <>
                            {departments.map((dep) => (
                                <DepartmentCard key={dep.id} department={dep} onDelete={setDeletingDepartment} />
                            ))}
                            {!noMore && (
                                <button
                                    className="btn btn-outline-primary mt-3 mb-2 align-self-center px-5"
                                    disabled={loading}
                                    onClick={handleShowMore}
                                >
                                    {loading ? "Загрузка..." : "Показать ещё"}
                                </button>
                            )}
                        </>
                    ) : (
                        <EmptyList
                            title="Здесь пока пусто"
                            subtitle="Нажмите «Добавить отдел», чтобы создать новый отдел."
                        />
                    )}
                </div>
            )}
            <DeleteDepartmentModal
                open={!!deletingDepartment}
                department={deletingDepartment}
                onClose={() => setDeletingDepartment(null)}
                onDelete={handleDelete}
            />

            <ToastContainer className="position-fixed top-0 start-50 translate-middle-x p-3" style={{ zIndex: 1080 }}>
                <Toast show={showToast} onClose={() => setShowToast(false)} bg="success" delay={2200} autohide>
                    <Toast.Body className="text-white d-flex align-items-center gap-2">
                        <i className="bi bi-check-circle-fill fs-5"></i>
                        <span>{toastMessage}</span>
                    </Toast.Body>
                </Toast>
            </ToastContainer>
        </div>
    );
}
