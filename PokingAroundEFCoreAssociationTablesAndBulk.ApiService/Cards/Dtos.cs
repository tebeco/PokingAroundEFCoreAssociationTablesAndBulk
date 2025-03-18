namespace PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Cards;

public record CardDto(int Id, string CardNumber);

public record CreateCardDto(string CardNumber);

public record UpdateCardDto(string CardNumber);

public record DeleteCardDto(int Id);
