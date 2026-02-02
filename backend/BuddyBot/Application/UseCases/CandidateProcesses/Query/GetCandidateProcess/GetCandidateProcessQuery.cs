using Domain.Enums;

namespace Application.UseCases.CandidateProcesses.Query.GetCandidateProcess;
public class GetCandidateProcessQuery
{
    public int CandidateId { get; set; }
    public ProcessKind ProcessKind { get; set; }
}