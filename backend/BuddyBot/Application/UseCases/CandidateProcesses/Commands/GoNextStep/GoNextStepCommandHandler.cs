using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.ProcessTemplates;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CandidateProcesses.Commands.GoNextStep;
public class GoNextStepCommandHandler(
    ICandidateProcessRepository candidateProcessRepository,
    IUnitOfWork unitOfWork,
    ILogger<GoNextStepCommand> logger
) : CommandBaseHandlerWithResult<GoNextStepCommand, CandidateProcess>( logger )
{
    protected override async Task<Result<CandidateProcess>> HandleImplAsync( GoNextStepCommand command )
    {
        CandidateProcess? process = await candidateProcessRepository.Get( command.CandidateId, command.ProcessKind );
        if ( process == null )
        {
            return Result<CandidateProcess>.FromError( "Процесс не найден." );
        }

        StepKind? nextStep = ProcessTransitionProvider.GetNextStep(
            command.ProcessKind,
            process.CurrentStep,
            command.CallbackData ?? string.Empty
        );

        if ( nextStep == null )
        {
            return Result<CandidateProcess>.FromError( "Переход невозможен." );
        }

        process.SetCurrentStep( nextStep.Value );
        await unitOfWork.CommitAsync();

        return Result<CandidateProcess>.FromSuccess( process );
    }
}
