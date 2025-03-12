namespace MatchMaker.Domain.Entities;
public class Team : SchemaBase<Team>
{ 
    public string TeamName { get; set; } = null!;
    public List<User> TeamCoaches { get; set; } = new();
}