namespace Domain.Entities;
public class UserContactInfo
{
    public int UserId { get; init; }
    public int? CityId { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? MentorVideoUrl { get; private set; }
    public string? MentorPhotoUrl { get; private set; }
    public string? MicrosoftTeamsUrl { get; private set; }
    public string? TelegramContact { get; private set; }
    public long TelegramId { get; private set; }

    public User? User { get; init; }
    public City? City { get; init; }

    public UserContactInfo( int userId, string firstName, string lastName)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
    }

    public UserContactInfo( int userId, long telegramId )
    {
        UserId = userId;
        TelegramId = telegramId;
    }

    public void SetFirstName( string firstName )
    {
        if ( string.IsNullOrWhiteSpace( firstName ) )
            throw new ArgumentException( "Иня не может быть пустым." );

        FirstName = firstName;
    }

    public void SetLastName( string lastName )
    {
        if ( string.IsNullOrWhiteSpace( lastName ) )
            throw new ArgumentException( "Фамилия не может быть пустой." );

        LastName = lastName;
    }

    public void SetTelegramContact( string telegramContact )
    {
        if ( string.IsNullOrWhiteSpace( telegramContact ) )
            throw new ArgumentException( "TelegramContact не может быть пустым." );

        if ( !telegramContact.StartsWith( "https://t.me/" ) )
            throw new ArgumentException( "TelegramContact должен быть ссылкой вида https://t.me/username" );

        TelegramContact = telegramContact;
    }

    public void SetTelegramId( long telegramId )
    {
        TelegramId = telegramId;
    }

    public void AddVideo( string mentorVideoUrl )
    {
        if ( string.IsNullOrWhiteSpace( mentorVideoUrl) )
            throw new ArgumentException( "URL не может быть пустым." );

        MentorVideoUrl = mentorVideoUrl;
    }
    public void AddPhoto( string mentorPhotoUrl )
    {
        if ( string.IsNullOrWhiteSpace( mentorPhotoUrl ) )
            throw new ArgumentException( "URL не может быть пустым." );

        MentorPhotoUrl = mentorPhotoUrl;
    }

    public void AddMicrosoftTeamsLink( string microsoftTeamsUrl )
    {
        if ( string.IsNullOrWhiteSpace( microsoftTeamsUrl ) )
            throw new ArgumentException( "URL не может быть пустым." );

        MicrosoftTeamsUrl = microsoftTeamsUrl;
    }

    public void RemovePhoto()
    {
        MentorPhotoUrl = null;
    }
}
