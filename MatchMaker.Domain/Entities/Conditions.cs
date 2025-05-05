namespace MatchMaker.Domain.Entities;

public class Conditions : SchemaBase<Conditions>
{
    public string Court { get; set; } = null!;
    public string OffensiveConditions { get; set; } = null!;
    public string DefenstiveConditions { get; set; } = null!;
    public string Specialists { get; set; } = null!;
    public string Penalties { get; set; } = null!;
}
