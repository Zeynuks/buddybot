using Domain.Enums;

namespace Application.UseCases.CandidateProcesses.Commands.GoNextStep;
public class GoNextStepCommand
{
    public int CandidateId { get; set; }
    public ProcessKind ProcessKind { get; set; }
    public string? CallbackData { get; set; }
}
