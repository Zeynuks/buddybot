namespace Domain.Entities;
public class UserAuthToken
{
    public int UserId { get; init; }
    public User? User { get; init; }
    public string RefreshToken { get; set; }
    public DateTime ExpireDate { get; set; }

    public UserAuthToken( int userId, string refreshToken, DateTime expireDate )
    {
        UserId = userId;
        RefreshToken = refreshToken;
        ExpireDate = expireDate;
    }
}
