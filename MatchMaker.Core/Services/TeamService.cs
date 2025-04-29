using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.DTOs.Teams;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Services;

public class TeamService(ITeamRepo teamRepo) : ITeamService
{
    private readonly ITeamRepo _teamRepo = teamRepo;
    public async Task<Team> CreateTeamAsync(Team newTeam)
    {
        ArgumentNullException.ThrowIfNull(newTeam);

        var existingTeam = await _teamRepo.GetTeamByNameAsync(newTeam.TeamName);

        if (existingTeam == null) await _teamRepo.CreateTeamAsync(newTeam);
     
        return newTeam;
    }
}
