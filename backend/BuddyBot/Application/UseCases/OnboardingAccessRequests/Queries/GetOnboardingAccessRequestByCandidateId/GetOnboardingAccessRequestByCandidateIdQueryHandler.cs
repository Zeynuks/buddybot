using Application.CQRSInterfaces;
using Application.Results;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.OnboardingAccessRequests.Queries.GetOnboardingAccessRequestByCandidateId;
public class GetOnboardingAccessRequestByCandidateIdQueryHandler( 
    IOnboardingAccessRequestRepository onboardingAccessRequestRepository,
    ILogger<GetOnboardingAccessRequestByCandidateIdQuery> logger )
    : QueryBaseHandler<OnboardingAccessRequest, GetOnboardingAccessRequestByCandidateIdQuery>( logger )
{
    protected override async Task<Result<OnboardingAccessRequest>> HandleImplAsync( GetOnboardingAccessRequestByCandidateIdQuery query )
    {
        OnboardingAccessRequest? onboardingAccessRequest = await onboardingAccessRequestRepository.Get( query.CandidateId );
        if ( onboardingAccessRequest == null )
        {
            return Result<OnboardingAccessRequest>.FromError( $"Заявка на онбординг с ID {query.CandidateId} не найден." );
        }

        return Result<OnboardingAccessRequest>.FromSuccess( onboardingAccessRequest );
    }
}