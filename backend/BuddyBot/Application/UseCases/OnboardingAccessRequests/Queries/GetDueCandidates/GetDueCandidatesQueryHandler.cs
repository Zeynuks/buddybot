using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.OnboardingAccessRequests.Queries.GetDueCandidates;
public class GetDueCandidatesQueryHandler(
    IOnboardingAccessRequestRepository onboardingAccessRequestRepository,
    ILogger<GetDueCandidatesQuery> logger )
    : QueryBaseHandler<List<User>, GetDueCandidatesQuery>( logger )
{
    protected override async Task<Result<List<User>>> HandleImplAsync( GetDueCandidatesQuery query )
    {
        List<User> users = await onboardingAccessRequestRepository.GetDueCandidates( query.UtcNow );
        if ( users == null || users.Count == 0 )
        {
            return Result<List<User>>.FromError( "Нет пользователей, ожидающих доступа к онбордингу." );
        }

        return Result<List<User>>.FromSuccess( users );
    }
}
