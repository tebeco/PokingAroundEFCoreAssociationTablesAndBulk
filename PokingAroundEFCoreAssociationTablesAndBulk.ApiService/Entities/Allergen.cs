namespace PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Entities;

public record Allergen
{
    public int Id { get; init; }

    public required string Name { get; set; }

    public List<User> Users { get; set; } = null!;
}
