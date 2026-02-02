import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import SearchInput from "../../components/SearchInput/SearchInput";
import { ROUTES } from "../../constants/routes";
import CityCard from "./CityCard";
import EmptyList from "../../components/EmptyList/EmptyList";
import type { City } from "../../models/Cities";
import { getCountry } from "../../services/countryService";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import { getCities } from "../../services/cityService";

export default function Cities() {
    const { countryId } = useParams<{ countryId: string }>();
    const [cities, setCities] = useState<City[]>([]);
    const [countryName, setCountryName] = useState<string>("");

    const [search, setSearch] = useState("");
    const [page, setPage] = useState(1);
    const [totalCount, setTotalCount] = useState(0);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!countryId) return;
        setLoading(true);
        setError(null);

        getCountry(Number(countryId))
            .then((c) => setCountryName(c.name ?? "-"))
            .catch(() => setCountryName("-"));

        getCities(Number(countryId), search, page)
            .then((result) => {
                setCities((prev) => (page === 1 ? result.items : [...prev, ...result.items]));
                setTotalCount(result.totalCount);
            })
            .catch(() => setError("Не удалось загрузить города"))
            .finally(() => setLoading(false));
    }, [countryId, search, page]);

    useEffect(() => {
        setCities([]);
        setPage(1);
        setTotalCount(0);
    }, [search, countryId]);

    const handleSearch = (val: string) => {
        setSearch(val);
        setPage(1);
    };

    const handleShowMore = () => {
        setPage((prev) => prev + 1);
    };

    const noMore = cities.length >= totalCount;

    return (
        <div className="mx-4">
            <h5 className="text-center fw-semibold mb-3 mt-2">{countryName || "Города"}</h5>
            <div className="mb-3">
                <Link
                    to={countryId ? ROUTES.CITIES.CREATE(countryId) : "#"}
                    className="btn btn-primary w-100 fw-medium py-2 mb-3 rounded-pill"
                >
                    + Добавить город
                </Link>
                <SearchInput placeholder="Поиск города…" value={search} onChange={handleSearch} />
            </div>
            {loading ? (
                <LoadingSpinner />
            ) : error ? (
                <div className="alert alert-danger">{error}</div>
            ) : (
                <div className="d-flex flex-column gap-3">
                    {cities.length > 0 ? (
                        <>
                            {cities.map((city) => (
                                <CityCard key={city.id} city={city} />
                            ))}
                            {!noMore && (
                                <button
                                    className="btn btn-outline-primary mt-3 mb-2 align-self-center px-5"
                                    disabled={loading}
                                    onClick={handleShowMore}
                                >
                                    {loading ? "Загрузка..." : "Показать ещё"}
                                </button>
                            )}
                        </>
                    ) : (
                        <EmptyList
                            title="Здесь пока пусто"
                            subtitle="Нажмите «Добавить город», чтобы создать новый город."
                        />
                    )}
                </div>
            )}
        </div>
    );
}
