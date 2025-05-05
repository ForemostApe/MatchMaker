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
        .Map(dest => dest.EndTime, src => src.EndTime)
        .Map(dest => dest.GameType, src => src.GameType)
        .Map(dest => dest.Location, src => src.Location)
        .Map(dest => dest.GameStatus, src => src.GameStatus)
        .Map(dest => dest.HomeTeamId, src => src.HomeTeamId)
        .Map(dest => dest.AwayTeamId, src => src.AwayTeamId)
        .Map(dest => dest.RefereeId, src => src.RefereeId)
        .Map(dest => dest.Conditions, src => src.Conditions);
    }
}
