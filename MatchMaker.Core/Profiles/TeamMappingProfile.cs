using Mapster;
using MatchMaker.Domain.DTOs.Teams;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Profiles;

public class TeamMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateTeamDTO, Team>()
            .Map(dest => dest.TeamName, src => src.TeamName)
            .Map(dest => dest.TeamLogo, src => src.TeamLogo);
    }
}
