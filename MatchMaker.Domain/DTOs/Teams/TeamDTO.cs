namespace MatchMaker.Domain.DTOs.Teams
{
    public class TeamDTO
    {
        public string Id { get; set; } = null!;
        public string TeamName { get; set; } = null!;
        public string? TeamLogo{ get; set; }
    }
}
