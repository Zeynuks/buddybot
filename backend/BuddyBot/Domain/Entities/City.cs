namespace Domain.Entities;
public class City
{
    public int Id { get; init; }
    public int CountryId { get; set; }
    public string Name { get; set; }

    public Country? Country { get; init; }

    public List<UserContactInfo> Candidates { get; init; }

    public City( int countryId, string name )
    {
        CountryId = countryId;
        Name = name;
        Candidates = new List<UserContactInfo>();
    }
}
