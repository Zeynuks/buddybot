namespace Contracts.UserDtos;
public class CurrentUserDto
{
    public int Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Login { get; init; }
    public List<string>? Roles { get; init; }
}
