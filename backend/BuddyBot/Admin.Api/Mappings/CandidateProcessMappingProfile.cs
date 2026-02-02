using AutoMapper;
using Contracts.CandidateProcessDtos;
using Domain.Entities;

namespace Admin.Api.Mappings;

public class CandidateProcessMappingProfile : Profile
{
    public CandidateProcessMappingProfile()
    {
        CreateMap<CandidateProcess, CandidateProcessDto>()
            .ForMember( d => d.CurrentStep,
                       opt => opt.MapFrom( src => src.CurrentStep.ToString() ) )
            .ForMember( d => d.Candidate,
                       opt => opt.MapFrom( src => src.Candidate ) );
    }
}
