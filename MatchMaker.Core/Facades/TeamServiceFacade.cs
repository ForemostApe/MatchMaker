using Mapster;
using MapsterMapper;
using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Utilities;
using MatchMaker.Domain.DTOs.Teams;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Facades;

public class TeamServiceFacade
    (
        ILogger<TeamServiceFacade> logger, 
        IMapper mapper, 
        ITeamService teamService,
        IFileValidationService fileValidationService,
        IFileStorageService fileStorageService
    ) 
    : ITeamServiceFacade
{
    private readonly ILogger<TeamServiceFacade> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly ITeamService _teamService = teamService;
    private readonly IFileValidationService _fileValidationService = fileValidationService;
    private readonly IFileStorageService _fileStorageService = fileStorageService;

    public async Task<Result<TeamDto>> CreateTeamAsync(CreateTeamDto newTeam)
    {
        ArgumentNullException.ThrowIfNull(newTeam.TeamName);

        try
        {
            const string storagePath = "\teamLogos";
            
            if (newTeam.TeamLogo != null)
            {
                bool isLogoValid = _fileValidationService.ValidateTeamLogoFile(newTeam.TeamLogo);
                if (isLogoValid) await _fileStorageService.StoreFileAsync(newTeam.TeamLogo, storagePath);
                
                //TODO
                //Add proper result-handling from the service so the client knows why the file was rejected, a bool
                //doesn't cut it.
            }
            
            var team = _mapper.Map<Team>(newTeam);
            var result = await _teamService.CreateTeamAsync(team);

            return result.IsSuccess
                ? Result<TeamDto>.Success(_mapper.Map<TeamDto>(result.Data!), result.Message)
                : Result<TeamDto>.Failure(result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in TeamServiceFacade while trying create team.");
            throw;
        }
    }

    public async Task<Result<List<TeamDto>>> GetAllTeamsAsync()
    {
        try
        {
            var result = await _teamService.GetAllTeamsAsync();

            return result.IsSuccess
                ? Result<List<TeamDto>>.Success(_mapper.Map<List<TeamDto>>(result.Data!), result.Message)
                : Result<List<TeamDto>>.Failure(result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in TeamServiceFacade while trying get all teams.");
            throw;
        }
    }

    public async Task<Result<TeamDto>> GetTeamByIdAsync(string teamId)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamId);

        try
        {
            var result = await _teamService.GetTeamByIdAsync(teamId);

            return result.IsSuccess
                ? Result<TeamDto>.Success(_mapper.Map<TeamDto>(result.Data!), result.Message)
                : Result<TeamDto>.Failure(result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in TeamServiceFacade while trying get team by TeamId.");
            throw;
        }
    }

    public async Task<Result<TeamDto>> GetTeamByNameAsync(string teamName)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamName);

        try
        {
            var result = await _teamService.GetTeamByNameAsync(teamName);

            return result.IsSuccess
                ? Result<TeamDto>.Success(_mapper.Map<TeamDto>(result.Data!), result.Message)
                : Result<TeamDto>.Failure(result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in TeamServiceFacade while trying get team by TeamName.");
            throw;
        }
    }

    public async Task<Result<TeamDto>> UpdateTeamAsync(UpdateTeamDto updatedTeam)
    {
        ArgumentNullException.ThrowIfNull(updatedTeam);

        try
        {
            var existingTeam = await _teamService.GetTeamByIdAsync(updatedTeam.TeamId);
            if (existingTeam == null) return Result<TeamDto>.Failure("Coulnd't find the specified team.");

            updatedTeam.Adapt(existingTeam.Data!);

            var result = await _teamService.UpdateTeamAsync(existingTeam.Data!);

            return result.IsSuccess
                ? Result<TeamDto>.Success(result.Data!.Adapt<TeamDto>(), result.Message)
                : Result<TeamDto>.Failure(result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in TeamServiceFacade while trying update team.");
            throw;
        }
    }

    public async Task<Result<TeamDto>> DeleteTeamAsync(DeleteTeamDto deleteTeamDto)
    {
        ArgumentNullException.ThrowIfNull(deleteTeamDto);

        try
        {
            var result = await _teamService.DeleteTeamAsync(deleteTeamDto.TeamId);

            return result.IsSuccess ?
                Result<TeamDto>.Success(null, result.Message)
                : Result<TeamDto>.Failure(result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in TeamServiceFacade while trying delete team.");
            throw;
        }
    }
}
