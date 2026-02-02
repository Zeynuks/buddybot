import { Link } from "react-router-dom";
import { ROUTES } from "../../constants/routes";
import type { Country } from "../../models/Countries";

export default function CountryCard({ country }: { country: Country }) {
    return (
        <div className="card border-0 shadow-sm rounded-4 px-3 py-3 position-relative">
            <div className="position-absolute top-0 end-0 m-2">
                <Link
                    to={ROUTES.COUNTRIES.EDIT(country.id)}
                    className="btn btn-link text-primary p-0"
                    title="Редактировать"
                >
                    <i className="bi bi-pencil-square fs-5"></i>
                </Link>
            </div>
            <div className="d-flex flex-column">
                <div>
                    <div className="fw-semibold mb-1">{country.name}</div>
                    <span className="badge bg-primary bg-opacity-10 text-primary fw-normal">
                        {country.citiesCount} {country.citiesCount === 1 ? "город" : "города"}
                    </span>
                </div>
                <div className="mt-auto d-flex justify-content-end align-items-end pt-2">
                    <Link
                        to={ROUTES.CITIES.BY_COUNTRY(country.id)}
                        className="small text-decoration-none text-primary fw-semibold"
                    >
                        Города <i className="bi bi-chevron-right small align-middle"></i>
                    </Link>
                </div>
            </div>
        </div>
    );
}
