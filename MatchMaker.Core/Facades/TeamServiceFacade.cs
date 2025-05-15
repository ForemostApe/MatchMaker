using Mapster;
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

        var team = _mapper.Map<Team>(newTeam);
        var result = await _teamService.CreateTeamAsync(team);

        return result.IsSuccess 
            ? Result<TeamDTO>.Success(_mapper.Map<TeamDTO>(result.Data!), result.Message)
            : Result<TeamDTO>.Failure(result.Message);
    }

    public async Task<Result<List<TeamDTO>>> GetAllTeamsAsync()
    {
        var result = await _teamService.GetAllTeamsAsync();

        return result.IsSuccess 
            ? Result<List<TeamDTO>>.Success(_mapper.Map<List<TeamDTO>>(result.Data!), result.Message) 
            : Result<List<TeamDTO>>.Failure(result.Message);
    }

    public async Task<Result<TeamDTO>> GetTeamByIdAsync(string teamId)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamId);

        var result = await _teamService.GetTeamByIdAsync(teamId);

        return result.IsSuccess 
            ? Result<TeamDTO>.Success(_mapper.Map<TeamDTO>(result.Data!), result.Message) 
            : Result<TeamDTO>.Failure(result.Message);
    }

    public async Task<Result<TeamDTO>> GetTeamByNameAsync(string teamName)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamName);

        var result = await _teamService.GetTeamByNameAsync(teamName);

        return result.IsSuccess 
            ? Result<TeamDTO>.Success(_mapper.Map<TeamDTO>(result.Data!), result.Message)
            : Result<TeamDTO>.Failure(result.Message);
    }

    public async Task<Result<TeamDTO>> UpdateTeamAsync(UpdateTeamDTO updatedTeam)
    {
        ArgumentNullException.ThrowIfNull(updatedTeam);

        var existingTeam = await _teamService.GetTeamByIdAsync(updatedTeam.TeamId);
        if (existingTeam == null) return Result<TeamDTO>.Failure("Coulnd't find the specified team.");

        updatedTeam.Adapt(existingTeam.Data!);

        var result = await _teamService.UpdateTeamAsync(existingTeam.Data!);

        return result.IsSuccess 
            ? Result<TeamDTO>.Success(result.Data!.Adapt<TeamDTO>(), result.Message)
            : Result<TeamDTO>.Failure(result.Message);
    }

    public async Task<Result<TeamDTO>> DeleteTeamAsync(DeleteTeamDTO deleteTeamDTO)
    {
        ArgumentNullException.ThrowIfNull(deleteTeamDTO);

        var result = await _teamService.DeleteTeamAsync(deleteTeamDTO.TeamId);

        return result.IsSuccess ?
            Result<TeamDTO>.Success(null, result.Message)
            : Result<TeamDTO>.Failure(result.Message);
    }
}
