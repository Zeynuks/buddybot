type AuthLayoutProps = {
    children: React.ReactNode;
};

function AuthLayout({ children }: AuthLayoutProps) {
    return (
        <div className="d-flex justify-content-center align-items-center min-vh-100 bg-light px-2">
            <div
                className="p-4 rounded shadow bg-white w-100"
                style={{
                    maxWidth: 500,
                }}
            >
                {children}
            </div>
        </div>
    );
}

export default AuthLayout;
