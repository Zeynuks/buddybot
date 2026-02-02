using Domain.Enums;

namespace Application.UseCases.Users.Queries.GetUsers;
public class GetUsersQuery
{
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public List<RoleName>? Roles { get; set; }
    public int? DepartmentId { get; set; }
    public int? TeamId { get; set; }
    public ProcessKind? ProcessKind { get; set; }

}
