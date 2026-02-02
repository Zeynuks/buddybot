namespace Application.UseCases.Departments.Queries.GetDepartments;
public class GetDepartmentsQuery
{
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
