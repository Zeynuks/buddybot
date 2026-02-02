import emptyImg from "../../assets/empty-list.png";

type EmptyListProps = {
    title: string;
    subtitle?: string;
};

export default function EmptyList({ title, subtitle }: EmptyListProps) {
    return (
        <div className="d-flex flex-column align-items-center justify-content-center py-5">
            <div className="mb-3" style={{ maxWidth: 100 }}>
                <img src={emptyImg} alt="Пусто" className="img-fluid w-100" />
            </div>
            <div className="fw-semibold fs-3 mb-2 text-center">{title}</div>
            {subtitle && <div className="text-secondary text-center">{subtitle}</div>}
        </div>
    );
}
