namespace Domain.Entities;
public class Department
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string? HeadFirstName { get; set; }
    public string? HeadLastName { get; set; }
    public string? HeadVideoUrl { get; private set; }
    public string? HeadMicrosoftTeamsUrl { get; private set; }
    public bool IsDeleted { get; private set; }

    public List<Team> Teams { get; private set; }

    public Department( string name, string headFirstName, string headLastName )
    {
        Name = name;
        HeadFirstName = headFirstName;
        HeadLastName = headLastName;
        Teams = new List<Team>();
    }

    public void AddHeadVideo( string headVideoUrl )
    {
        HeadVideoUrl = headVideoUrl;
    }

    public void AddHeadMicrosoftTeamsLink( string headMicrosoftTeamsUrl )
    {
        if ( string.IsNullOrWhiteSpace( headMicrosoftTeamsUrl) )
            throw new ArgumentException( "URL не может быть пустым." );

        HeadMicrosoftTeamsUrl = headMicrosoftTeamsUrl;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
