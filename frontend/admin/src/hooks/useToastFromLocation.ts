import { useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";

export function useToastFromLocation(setToastMessage: (msg: string) => void, setShowToast: (show: boolean) => void) {
    const location = useLocation();
    const navigate = useNavigate();

    useEffect(() => {
        if (location.state?.toast) {
            setToastMessage(location.state.toast);
            setShowToast(true);
            navigate(location.pathname, { replace: true });
        }
        // eslint-disable-next-line
    }, [location, navigate]);
}
