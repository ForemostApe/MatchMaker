using MongoDB.Entities;

namespace MatchMaker.Domain.Entities;
public class Team : Entity
{ 
    public string TeamName { get; set; } = null!;
    public List<User> TeamCoaches { get; set; } = new();
}