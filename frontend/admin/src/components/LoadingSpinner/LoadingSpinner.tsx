import Spinner from "react-bootstrap/Spinner";

export default function LoadingSpinner() {
    return (
        <div
            style={{
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                minHeight: "100vh",
                width: "100%",
            }}
        >
            <Spinner animation="border" variant="primary" role="status" style={{ width: 48, height: 48 }}>
                <span className="visually-hidden">Загрузка...</span>
            </Spinner>
        </div>
    );
}
