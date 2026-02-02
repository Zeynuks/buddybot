namespace Domain.Entities;
public class HRInfo
{
    public int UserId { get; init; }
    public bool IsActive { get; set; }

    public User? HR { get; init; }

    public HRInfo( int userId )
    {
        UserId = userId;
        IsActive = true;
    }
}
