using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CandidateProcesses.Commands.TransferToPersonalArea;
public class TransferToPersonalAreaCommandHandler(
    ICandidateProcessRepository candidateProcessRepository,
    IUnitOfWork unitOfWork,
    ILogger<TransferToPersonalAreaCommand> logger
) : CommandBaseHandlerWithResult<TransferToPersonalAreaCommand, CandidateProcess>( logger )
{
    protected override async Task<Result<CandidateProcess>> HandleImplAsync( TransferToPersonalAreaCommand command )
    {
        CandidateProcess? onboarding = await candidateProcessRepository.Get( command.CandidateId, ProcessKind.Onboarding );
        if ( onboarding != null && onboarding.IsActive )
        {
            onboarding.SetActive( false );
        }

        CandidateProcess? personalArea = await candidateProcessRepository.Get( command.CandidateId, ProcessKind.PersonalArea );
        if ( personalArea != null )
        {
            personalArea.SetActive( true );
        }
        else
        {
            personalArea = new CandidateProcess( command.CandidateId, ProcessKind.PersonalArea, StepKind.PersonalAreaHome );
            candidateProcessRepository.Add( personalArea );
        }

        await unitOfWork.CommitAsync();

        return Result<CandidateProcess>.FromSuccess( personalArea );
    }
}