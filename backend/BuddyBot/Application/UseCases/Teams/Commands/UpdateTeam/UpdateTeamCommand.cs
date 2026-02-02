using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Teams.Commands.UpdateTeam;
public class UpdateTeamCommand
{
    public int Id { get; set; }

    [StringLength( 100, MinimumLength = 2, ErrorMessage = "Название команды должно быть от {2} до {1} символов." )]
    public string? Name { get; set; }
    public int DepartmentId { get; set; }
    public int? LeaderId { get; set; }
}