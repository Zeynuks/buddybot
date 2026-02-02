import { Link } from "react-router-dom";
import { ROUTES } from "../../constants/routes";
import type { City } from "../../models/Cities";

export default function CityCard({ city }: { city: City }) {
    function pluralize(n: number) {
        if (n % 10 === 1 && n % 100 !== 11) return "кандидат";
        if ([2, 3, 4].includes(n % 10) && ![12, 13, 14].includes(n % 100)) return "кандидата";
        return "кандидатов";
    }

    return (
        <div className="card border-0 shadow-sm rounded-4 px-3 py-3 position-relative">
            <div className="position-absolute top-0 end-0 m-2">
                <Link to={ROUTES.CITIES.EDIT(city.countryId, city.id)}>
                    <i className="bi bi-pencil-square fs-5"></i>
                </Link>
            </div>
            <div>
                <div className="fw-semibold mb-1">{city.name}</div>
                <span className="badge bg-primary bg-opacity-10 text-primary fw-normal">
                    {city.candidateCount ?? 0} {pluralize(city.candidateCount ?? 0)}
                </span>
            </div>
        </div>
    );
}
