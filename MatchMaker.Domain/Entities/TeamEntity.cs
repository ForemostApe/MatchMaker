using MongoDB.Entities;

namespace MatchMaker.Domain.Entities;
public class TeamEntity : Entity
{ 
    public string TeamName { get; set; } = null!;
    public List<UserEntity> TeamCoaches { get; set; } = new();
}