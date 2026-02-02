namespace Application.UseCases.Teams.Queries.GetTeams;
public class GetTeamsQuery
{
    public string? SearchTerm { get; set; }
    public int? DepartmentId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
