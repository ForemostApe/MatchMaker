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

    public async Task<Result<List<TeamDTO>>> GetAllTeamsAsync()
    {
        try
        {
            var result = await _teamService.GetAllTeamsAsync();
            if (!result.IsSuccess) return Result<List<TeamDTO>>.Failure(result.Message);

            var teams = _mapper.Map<List<TeamDTO>>(result.Data!);
            return Result<List<TeamDTO>>.Success(teams, result.Message);

        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<TeamDTO>> GetTeamByIdAsync(string teamId)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamId);

        try
        {
            var result = await _teamService.GetTeamByIdAsync(teamId);

            if (!result.IsSuccess) return Result<TeamDTO>.Failure(result.Message);

            TeamDTO teamDTO = _mapper.Map<TeamDTO>(result.Data!);

            return Result<TeamDTO>.Success(teamDTO, result.Message);
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<TeamDTO>> GetTeamByNameAsync(string teamName)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamName);

        try
        {
            var result = await _teamService.GetTeamByNameAsync(teamName);

            if (!result.IsSuccess) return Result<TeamDTO>.Failure(result.Message);

            TeamDTO teamDTO = _mapper.Map<TeamDTO>(result.Data!);

            return Result<TeamDTO>.Success(teamDTO, result.Message);
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<TeamDTO>> UpdateTeamAsync(UpdateTeamDTO updatedTeam)
    {
        ArgumentNullException.ThrowIfNull(updatedTeam);

        try
        {
            var existingTeam = await _teamService.GetTeamByIdAsync(updatedTeam.TeamId);

            if (existingTeam == null) return Result<TeamDTO>.Failure("Coulnd't find the specified team.");

            updatedTeam.Adapt(existingTeam.Data!);

            var result = await _teamService.UpdateTeamAsync(existingTeam.Data!);

            if (!result.IsSuccess) return Result<TeamDTO>.Failure(result.Message);

            var teamDTO = result.Data!.Adapt<TeamDTO>();

            return Result<TeamDTO>.Success(teamDTO, result.Message);
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<TeamDTO>> DeleteTeamAsync(DeleteTeamDTO deleteTeamDTO)
    {
        ArgumentNullException.ThrowIfNull(deleteTeamDTO);

        try
        {
            var result = await _teamService.DeleteTeamAsync(deleteTeamDTO.TeamId);

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
