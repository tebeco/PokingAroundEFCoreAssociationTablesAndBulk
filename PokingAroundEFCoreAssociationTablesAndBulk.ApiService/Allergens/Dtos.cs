namespace PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Allergens;

public record AllergenDto(int Id, string Name);

public record CreateAllergenDto(string Name);

public record UpdateAllergenDto(string Name);
