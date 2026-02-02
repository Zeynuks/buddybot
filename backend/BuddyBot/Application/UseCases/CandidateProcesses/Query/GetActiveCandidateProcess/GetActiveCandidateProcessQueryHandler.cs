using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CandidateProcesses.Query.GetActiveCandidateProcess;
public class GetActiveCandidateProcessQueryHandler(
    ICandidateProcessRepository candidateProcessRepository,
    ILogger<GetActiveCandidateProcessQuery> logger
) : QueryBaseHandler<CandidateProcess, GetActiveCandidateProcessQuery>( logger )
{
    protected override async Task<Result<CandidateProcess>> HandleImplAsync( GetActiveCandidateProcessQuery query )
    {
        CandidateProcess? process = await candidateProcessRepository.GetActive( query.CandidateId );
        if ( process == null )
        {
            return Result<CandidateProcess>.FromError( "Процесс не найден." );
        }

        return Result<CandidateProcess>.FromSuccess( process );
    }
}