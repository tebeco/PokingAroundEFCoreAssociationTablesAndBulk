namespace PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Entities;

public record User
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required List<Allergen> Allergens { get; set; }

    public required List<Card> Cards { get; set; }
}
