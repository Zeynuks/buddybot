using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Departments.Commands.UpdateDepartment;
public class UpdateDepartmentCommand
{
    public int Id { get; set; }

    [StringLength( 100, MinimumLength = 2, ErrorMessage = "Название отдела должно быть от {2} до {1} символов." )]
    public string? Name { get; set; }

    [StringLength( 50, MinimumLength = 2, ErrorMessage = "Имя руководителя должно быть от {2} до {1} символов." )]
    public string? HeadFirstName { get; set; }

    [StringLength( 50, MinimumLength = 2, ErrorMessage = "Фамилия руководителя должна быть от {2} до {1} символов." )]
    public string? HeadLastName { get; set; }
    public string? HeadVideoUrl { get; set; }

    [StringLength( 200, ErrorMessage = "Ссылка на MS Teams не должна превышать {1} символов." )]
    public string? HeadMicrosoftTeamsUrl { get; set; }
}
