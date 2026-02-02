type Props = {
    total: number;
    current: number;
    onPageChange?: (page: number) => void;
};

export default function Pagination({ total, current, onPageChange }: Props) {
    const visiblePages = [1, 2, 3];

    return (
        <nav>
            <ul className="pagination justify-content-center mb-0">
                <li className={`page-item${current === 1 ? " disabled" : ""}`}>
                    <button
                        className="page-link"
                        tabIndex={current === 1 ? -1 : 0}
                        disabled={current === 1}
                        onClick={() => onPageChange?.(current - 1)}
                        aria-label="Предыдущая"
                    >
                        <span aria-hidden="true">&laquo;</span>
                    </button>
                </li>
                {visiblePages.map((n) => (
                    <li key={n} className={`page-item${current === n ? " active" : ""}`}>
                        <button className="page-link" onClick={() => onPageChange?.(n)}>
                            {n}
                        </button>
                    </li>
                ))}
                <li className="page-item disabled">
                    <span className="page-link">…</span>
                </li>
                <li className={`page-item${current === total ? " active" : ""}`}>
                    <button className="page-link" onClick={() => onPageChange?.(total)}>
                        {total}
                    </button>
                </li>
                <li className={`page-item${current === total ? " disabled" : ""}`}>
                    <button
                        className="page-link"
                        tabIndex={current === total ? -1 : 0}
                        disabled={current === total}
                        onClick={() => onPageChange?.(current + 1)}
                        aria-label="Следующая"
                    >
                        <span aria-hidden="true">&raquo;</span>
                    </button>
                </li>
            </ul>
        </nav>
    );
}
