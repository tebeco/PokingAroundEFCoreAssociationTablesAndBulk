using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Allergens;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Cards;

namespace PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Users;

public record UserDto(int Id, string Name, IEnumerable<AllergenDto> Allergens, IEnumerable<CardDto> Cards);

public record CreateUserDto(string Name, int[] AllergenIds, List<CreateCardDto> Cards);

public record UpdateUserDto(string Name, List<int> AllergenIds, List<CreateCardDto> Cards);

public record DeleteUserDto(int Id);
