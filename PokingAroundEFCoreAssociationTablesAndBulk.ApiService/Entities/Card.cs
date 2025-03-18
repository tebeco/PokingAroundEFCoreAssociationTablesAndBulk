namespace PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Entities;

public record Card
{
    public int Id { get; set; }

    public required string CardNumber { get; set; }

    public User User { get; set; } = null!;
}
