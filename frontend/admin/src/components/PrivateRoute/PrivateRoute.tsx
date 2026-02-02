import { Navigate, Outlet, useLocation } from "react-router-dom";
import { ROUTES } from "../../constants/routes";
import useAuth from "../../hooks/useAuth";
import LoadingSpinner from "../LoadingSpinner/LoadingSpinner";

export function PrivateRoute() {
    const { isAuthenticated, loading } = useAuth();
    const location = useLocation();

    if (loading) {
        return <LoadingSpinner />;
    }

    return isAuthenticated ? <Outlet /> : <Navigate to={ROUTES.AUTH.LOGIN} state={{ from: location }} replace />;
}
