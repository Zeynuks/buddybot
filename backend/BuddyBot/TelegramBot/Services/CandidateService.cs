using Application.CQRSInterfaces;
using Application.Results;
using Application.UseCases.CandidateProcesses.Commands.GoNextStep;
using Application.UseCases.CandidateProcesses.Commands.ResetCandidateProcess;
using Application.UseCases.CandidateProcesses.Commands.TransferToPersonalArea;
using Application.UseCases.CandidateProcesses.Query.GetActiveCandidateProcess;
using Application.UseCases.CandidateProcesses.Query.GetCandidateProcess;
using Application.UseCases.OnboardingAccessRequests.Commands.CreateOnboardingAccessRequest;
using Domain.Entities;
using Domain.Enums;

namespace TelegramBot.Services;
public class CandidateService(
    ICommandHandlerWithResult<GoNextStepCommand, CandidateProcess> goNextStepHandler,
    ICommandHandlerWithResult<ResetCandidateProcessCommand, CandidateProcess> resetCandidateProcessHandler,
    IQueryHandler<CandidateProcess, GetCandidateProcessQuery> getCandidateProcessHandler,
    ICommandHandlerWithResult<CreateOnboardingAccessRequestCommand, OnboardingAccessRequest> onboardingAccessRequestHandler,
    IQueryHandler<CandidateProcess, GetActiveCandidateProcessQuery> getActiveProcessHandler,
    ICommandHandlerWithResult<TransferToPersonalAreaCommand, CandidateProcess> transferToPersonalAreaHandler)
{
    public async Task<CandidateProcess?> GoNextStep( int candidateId, ProcessKind processKind, string? callbackData )
    {
        GoNextStepCommand command = new GoNextStepCommand
        {
            CandidateId = candidateId,
            ProcessKind = processKind,
            CallbackData = callbackData
        };

        Result<CandidateProcess> result = await goNextStepHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            return null;
        }

        return result.Value;
    }

    public async Task<Result<CandidateProcess>> ResetCandidateProcess( int candidateId )
    {
        ResetCandidateProcessCommand command = new ResetCandidateProcessCommand
        {
            CandidateId = candidateId
        };

        return await resetCandidateProcessHandler.HandleAsync( command );
    }


    public async Task<CandidateProcess?> GetCandidateProcess( int candidateId, ProcessKind processKind )
    {
        GetCandidateProcessQuery query = new GetCandidateProcessQuery
        {
            CandidateId = candidateId,
            ProcessKind = processKind
        };

        Result<CandidateProcess> result = await getCandidateProcessHandler.HandleAsync( query );

        if ( !result.IsSuccess )
        {
            return null;
        }

        return result.Value;
    }

    public async Task<bool> RequestOnboardingAccess( int candidateId )
    {
        CandidateProcess? existing = await GetCandidateProcess( candidateId, ProcessKind.Preboarding );
        if ( existing?.CurrentStep == StepKind.WaitAdminApprove )
        {
            return false;
        }

        CreateOnboardingAccessRequestCommand command = new CreateOnboardingAccessRequestCommand
        {
            CandidateId = candidateId
        };

        Result<OnboardingAccessRequest> result = await onboardingAccessRequestHandler.HandleAsync( command );

        return result.IsSuccess;
    }

    public async Task<CandidateProcess?> GetActiveProcess( int candidateId )
    {
        GetActiveCandidateProcessQuery query = new GetActiveCandidateProcessQuery
        {
            CandidateId = candidateId
        };

        Result<CandidateProcess> result = await getActiveProcessHandler.HandleAsync( query );

        return result.Value;
    }

    public async Task<Result<CandidateProcess>> TransferToPersonalArea( int candidateId )
    {
        TransferToPersonalAreaCommand command = new TransferToPersonalAreaCommand
        {
            CandidateId = candidateId
        };

        return await transferToPersonalAreaHandler.HandleAsync( command );
    }
}
