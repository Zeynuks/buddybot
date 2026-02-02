namespace Domain.Entities;
public class Country
{
    public int Id { get; init; }
    public string Name { get; set; }

    public List<City> Cities { get; private set; }

    public Country( string name )
    {
        Name = name;
        Cities = new List<City>();
    }
}
