using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Teams.Commands.CreateTeam;
public class CreateTeamCommand
{
    [StringLength( 100, MinimumLength = 2, ErrorMessage = "Название команды должно быть от {2} до {1} символов." )]
    public string? Name { get; set; }
    public int DepartmentId { get; set; }
}
