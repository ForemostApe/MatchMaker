using Mapster;
using MatchMaker.Domain.DTOs.Games;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Profiles;

public class GameMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateGameDTO, Game>()
           .Map(dest => dest.StartTime, src => src.StartTime)
           .Map(dest => dest.GameType, src => src.GameType)
           .Map(dest => dest.Location, src => src.Location)
           .Map(dest => dest.HomeTeamId, src => src.HomeTeamId)
           .Map(dest => dest.AwayTeamId, src => src.AwayTeamId)
           .Map(dest => dest.RefereeId, src => src.RefereeId)
           .Map(dest => dest.Conditions, src => new Conditions
           {
               Court = src.Conditions.Court,
               OffensiveConditions = src.Conditions.OffensiveConditions,
               DefensiveConditions = src.Conditions.DefensiveConditions,
               Specialists = src.Conditions.Specialists,
               Penalties = src.Conditions.Penalties
           });

        config.NewConfig<UpdateGameDTO, Game>()
           .Map(dest => dest.StartTime, src => src.StartTime)
           .Map(dest => dest.Location, src => src.Location)
           .Map(dest => dest.RefereeId, src => src.RefereeId)
           .Map(dest => dest.Conditions, src => new Conditions
           {
               Court = src.Conditions.Court,
               Timing = src.Conditions.Timing,
               OffensiveConditions = src.Conditions.OffensiveConditions,
               DefensiveConditions = src.Conditions.DefensiveConditions,
               Specialists = src.Conditions.Specialists,
               Penalties = src.Conditions.Penalties
           })
            .IgnoreNullValues(true)
            .IgnoreNonMapped(true);
    }
}
