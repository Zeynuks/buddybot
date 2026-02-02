using Domain.Enums;

namespace Application.UseCases.Users.Queries.GetUsersLookup;
public class GetUsersLookupQuery
{
    public List<RoleName>? Roles { get; set; }
    public int? TeamId { get; set; }
}
