import { useCallback, useEffect, useState } from "react";
import { Outlet } from "react-router-dom";
import Sidebar from "../components/Sidebar/Sidebar";
import Header from "../components/Header/Header";
import LoadingSpinner from "../components/LoadingSpinner/LoadingSpinner";
import useAuth from "../hooks/useAuth";

function MainLayout() {
    const [isMobile, setIsMobile] = useState(window.innerWidth < 992);
    const { user, loading } = useAuth();

    const hideSidebar = useCallback(() => {
        const sidebarOffcanvas = document.getElementById("sidebarOffcanvas");
        if (sidebarOffcanvas) {
            // eslint-disable-next-line @typescript-eslint/ban-ts-comment
            // @ts-expect-error
            const bsOffcanvas = window.bootstrap?.Offcanvas?.getInstance(sidebarOffcanvas);
            if (bsOffcanvas) {
                bsOffcanvas.hide();
            } else {
                sidebarOffcanvas.classList.remove("show");
                document.body.classList.remove("offcanvas-backdrop", "show");
                document.querySelectorAll(".offcanvas-backdrop").forEach((el) => el.remove());
            }
        }
    }, []);

    useEffect(() => {
        function handleResize() {
            setIsMobile(window.innerWidth < 992);
        }
        window.addEventListener("resize", handleResize);
        return () => window.removeEventListener("resize", handleResize);
    }, []);

    useEffect(() => {
        document.body.style.overflowX = "hidden";
        return () => {
            document.body.style.overflowX = "";
        };
    }, []);

    if (loading) return <LoadingSpinner />;

    return (
        <div className="min-vh-100 bg-light">
            <Header userName={user ? `${user.firstName} ${user.lastName}` : "—"} isMobile={isMobile} />
            <div className="container-fluid" style={{ maxWidth: "100vw", overflowX: "hidden" }}>
                <div className="row">
                    <div className="col-lg-2 d-none d-lg-block bg-white border-end min-vh-100 px-0">
                        <Sidebar />
                    </div>
                    <div className="d-lg-none">
                        <div
                            className="offcanvas offcanvas-start"
                            tabIndex={-1}
                            id="sidebarOffcanvas"
                            aria-labelledby="sidebarOffcanvasLabel"
                        >
                            <div className="offcanvas-header d-flex justify-content-start align-items-center px-3 pt-3 pb-0">
                                <button
                                    type="button"
                                    className="btn-close ms-0"
                                    data-bs-dismiss="offcanvas"
                                    aria-label="Закрыть"
                                ></button>
                            </div>
                            <div className="offcanvas-body p-0">
                                <Sidebar onHide={hideSidebar} />
                            </div>
                        </div>
                    </div>
                    <main className="col px-0 py-4" style={{ minHeight: "60vh" }}>
                        <Outlet />
                    </main>
                </div>
            </div>
        </div>
    );
}

export default MainLayout;
