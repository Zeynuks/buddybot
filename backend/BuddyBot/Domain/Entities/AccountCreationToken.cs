using Domain.Enums;

namespace Domain.Entities;
public class AccountCreationToken
{
    public Guid TokenValue { get; init; }
    public DateTime IssuedAtUtc { get; init; }
    public DateTime? ActivatedAtUtc { get; private set; }
    public DateTime? ExpireDate { get; private set; }
    public AccountCreationTokenStatus Status { get; private set; }
    public string? TelegramInviteLink { get; private set; }
    public string? QrCodeBase64 { get; private set; }
    public int? UserId { get; private set; }
    public int? CreatorId { get; private set; }
    public bool IsDeleted { get; private set; }

    public User? User { get; private set; }
    public User? Creator { get; private set; }

    public AccountCreationToken()
    {
    }

    public AccountCreationToken( DateTime expireDate )
    {
        TokenValue = Guid.NewGuid();
        IssuedAtUtc = DateTime.UtcNow;
        Status = AccountCreationTokenStatus.Issued;
        ExpireDate = expireDate;
    }

    public void SetUser( User user )
    {
        User = user;
        UserId = user.Id;
    }

    public void SetInviteData( string telegramInviteLink, string qrCodeBase64 )
    {
        TelegramInviteLink = telegramInviteLink;
        QrCodeBase64 = qrCodeBase64;
    }

    public void Activate()
    {
        Status = AccountCreationTokenStatus.Activated;
        ActivatedAtUtc = DateTime.UtcNow;
    }

    public void SetStatusRevoked()
    {
        Status = AccountCreationTokenStatus.Revoked;
    }

    public void SetStatusExpired()
    {
        Status = AccountCreationTokenStatus.Expired;
    }

    public void SetExpireDate( DateTime expireDate )
    {
        ExpireDate = expireDate;
    }

    public void SetCreator( User? user )
    {
        Creator = user;
        CreatorId = user?.Id;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
