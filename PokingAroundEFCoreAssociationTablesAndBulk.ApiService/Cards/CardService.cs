using Microsoft.EntityFrameworkCore;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Entities;

namespace PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Cards;

public class CardService(MyDbContext myDbContext)
{
    public async Task<List<Card>> GetCardsAsync()
    {
        return await myDbContext
            .Cards
            .ToListAsync();
    }

    public async Task<Card?> GetCardByIdAsync(int id)
    {
        return await myDbContext
            .Cards
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Card> CreateCardAsync(CreateCardDto createCardDto)
    {
        var card = new Card { CardNumber = createCardDto.CardNumber };

        myDbContext.Cards.Add(card);
        await myDbContext.SaveChangesAsync();

        return card;
    }

    public async Task<Card?> UpdateCardAsync(int id, UpdateCardDto updateCardDto)
    {
        var card = await myDbContext
            .Cards
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();

        if (card is null)
        {
            return null;
        }

        card.CardNumber = updateCardDto.CardNumber;
        await myDbContext.SaveChangesAsync();

        return card;
    }
}
