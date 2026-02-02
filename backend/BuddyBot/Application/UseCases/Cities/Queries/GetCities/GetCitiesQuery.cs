namespace Application.UseCases.Cities.Queries.GetCities;
public class GetCitiesQuery
{
    public string? SearchTerm { get; set; }
    public int CountryId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
