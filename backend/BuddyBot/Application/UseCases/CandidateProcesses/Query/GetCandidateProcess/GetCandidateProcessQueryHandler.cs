using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CandidateProcesses.Query.GetCandidateProcess;
public class GetCandidateProcessQueryHandler(
    ICandidateProcessRepository candidateProcessRepository,
    ILogger<GetCandidateProcessQuery> logger
) : QueryBaseHandler<CandidateProcess, GetCandidateProcessQuery>( logger )
{
    protected override async Task<Result<CandidateProcess>> HandleImplAsync( GetCandidateProcessQuery query )
    {
        CandidateProcess? process = await candidateProcessRepository.Get( query.CandidateId, query.ProcessKind );
        if ( process == null )
        {
            return Result<CandidateProcess>.FromError( "Процесс не найден." );
        }

        return Result<CandidateProcess>.FromSuccess( process );
    }
}
