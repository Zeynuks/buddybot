using Domain.Enums;

namespace Domain.Entities;
public class CandidateProcess
{
    public int CandidateId { get; init; }
    public ProcessKind ProcessKind { get; init; }
    public StepKind CurrentStep { get; private set; }
    public bool IsActive { get; private set; }

    public User? Candidate { get; init; }

    public CandidateProcess( int candidateId, ProcessKind processKind, StepKind currentStep )
    {
        CandidateId = candidateId;
        ProcessKind = processKind;
        CurrentStep = currentStep;
        IsActive = true;
    }

    public void SetCurrentStep( StepKind step )
    {
        CurrentStep = step;
    }

    public void SetActive( bool active )
    {
        IsActive = active;
    }
}