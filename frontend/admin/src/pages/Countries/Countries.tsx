import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import SearchInput from "../../components/SearchInput/SearchInput";
import { ROUTES } from "../../constants/routes";
import type { Country } from "../../models/Countries";
import CountryCard from "./CountryCard";
import EmptyList from "../../components/EmptyList/EmptyList";
import { getCountries } from "../../services/countryService";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";

export default function Countries() {
    const [countries, setCountries] = useState<Country[]>([]);

    const [search, setSearch] = useState("");
    const [page, setPage] = useState(1);
    const [totalCount, setTotalCount] = useState(0);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        setLoading(true);
        setError(null);

        getCountries(search, page)
            .then((result) => {
                setCountries((prev) => (page === 1 ? result.items : [...prev, ...result.items]));
                setTotalCount(result.totalCount);
            })
            .catch(() => setError("Не удалось загрузить страны"))
            .finally(() => setLoading(false));
    }, [search, page]);

    useEffect(() => {
        setCountries([]);
        setPage(1);
        setTotalCount(0);
    }, [search]);

    const handleSearch = (val: string) => {
        setSearch(val);
        setPage(1);
    };

    const handleShowMore = () => {
        setPage((prev) => prev + 1);
    };

    const noMore = countries.length >= totalCount;

    return (
        <div className="mx-4">
            <h5 className="text-center fw-semibold mb-3 mt-2">Страны</h5>
            <div className="mb-3">
                <Link to={ROUTES.COUNTRIES.CREATE} className="btn btn-primary w-100 fw-medium py-2 mb-3 rounded-pill">
                    + Добавить страну
                </Link>
                <SearchInput placeholder="Поиск страны…" value={search} onChange={handleSearch} />
            </div>
            {loading && countries.length === 0 ? (
                <LoadingSpinner />
            ) : error ? (
                <div className="alert alert-danger">{error}</div>
            ) : (
                <div className="d-flex flex-column gap-3">
                    {countries.length > 0 ? (
                        <>
                            {countries.map((country) => (
                                <CountryCard key={country.id} country={country} />
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
                            subtitle="Нажмите «Добавить страну», чтобы создать новую страну."
                        />
                    )}
                </div>
            )}
        </div>
    );
}
