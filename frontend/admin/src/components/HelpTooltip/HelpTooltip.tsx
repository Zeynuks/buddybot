import { useEffect, useState } from "react";

type HelpTooltipProps = {
    children: React.ReactNode;
    minWidth?: number;
    maxWidth?: number;
    iconClassName?: string;
    ariaLabel?: string;
};

export const HelpTooltip = ({
    children,
    minWidth = 390,
    maxWidth = 480,
    iconClassName = "bi bi-question-circle ms-2 text-primary",
    ariaLabel = "Показать подсказку",
}: HelpTooltipProps) => {
    const [show, setShow] = useState(false);
    const [isMobile, setIsMobile] = useState(false);

    useEffect(() => {
        const handleResize = () => setIsMobile(window.innerWidth <= 576);
        handleResize();
        window.addEventListener("resize", handleResize);
        return () => window.removeEventListener("resize", handleResize);
    }, []);

    return (
        <span
            className="position-relative d-inline-block"
            onMouseEnter={() => setShow(true)}
            onMouseLeave={() => setShow(false)}
            onFocus={() => setShow(true)}
            onBlur={() => setShow(false)}
            tabIndex={0}
        >
            <i className={iconClassName} style={{ cursor: "pointer", fontSize: 18 }} aria-label={ariaLabel} />
            {show &&
                (isMobile ? (
                    <div
                        className="help-tooltip-popup-mobile bg-light text-dark rounded-3 shadow p-3"
                        style={{
                            minWidth: 0,
                            maxWidth: "92vw",
                            width: "88vw",
                            fontSize: 14,
                            lineHeight: 1.6,
                            zIndex: 9999,
                            position: "fixed",
                            left: "50%",
                            top: "50%",
                            transform: "translate(-50%, -50%)",
                            boxShadow: "0 4px 24px 2px rgba(0,0,0,.16)",
                        }}
                    >
                        {children}
                    </div>
                ) : (
                    <div
                        className="help-tooltip-popup bg-light text-dark rounded-3 shadow p-3"
                        style={{
                            minWidth,
                            maxWidth,
                            fontSize: 15,
                            lineHeight: 1.7,
                            zIndex: 100,
                            position: "absolute",
                            left: "100%",
                            top: "50%",
                            transform: "translateY(-50%)",
                        }}
                    >
                        {children}
                    </div>
                ))}
        </span>
    );
};
