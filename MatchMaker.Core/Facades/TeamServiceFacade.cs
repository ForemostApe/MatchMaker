using MapsterMapper;
using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.DTOs.Teams;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Facades;

public class TeamServiceFacade(IMapper mapper, ITeamService teamService) : ITeamServiceFacade
{
    private readonly IMapper _mapper = mapper;
    private readonly ITeamService _teamService = teamService;

    public async Task<Result<TeamDTO>> CreateTeamAsync(CreateTeamDTO newTeam)
    {
        ArgumentNullException.ThrowIfNull(newTeam);

        try
        {
            var team = _mapper.Map<Team>(newTeam);
            var result = await _teamService.CreateTeamAsync(team);

            if (!result.IsSuccess) return Result<TeamDTO>.Failure(result.Message);

            TeamDTO teamDTO = _mapper.Map<TeamDTO>(result.Data!);

            return Result<TeamDTO>.Success(teamDTO, result.Message);
        }
        catch
        {
            throw;
        }
    }
}
