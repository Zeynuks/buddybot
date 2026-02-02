using Domain.Enums;

namespace Application.UseCases.AccountCreationTokens.Queries.GetAccountCreationTokens;
public class GetAccountCreationTokensQuery
{
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public AccountCreationTokenStatus? Status { get; set; }
    public List<RoleName>? Roles { get; set; } = new();
}
