using MongoDB.Entities;

namespace MatchMaker.Domain.Entities;

class TeamEntity : Entity
{ 
    public string TeamName { get; set; } = null!;
    public List<UserEntity> TeamCoaches { get; set; } = new();
}
