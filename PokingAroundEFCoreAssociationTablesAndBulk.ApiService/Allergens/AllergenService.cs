using Microsoft.EntityFrameworkCore;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Entities;

namespace PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Allergens;

public class AllergenService(MyDbContext myDbContext)
{
    public async Task<List<Allergen>> GetAllergensAsync()
    {
        return await myDbContext
            .Allergens
            .ToListAsync();
    }

    public async Task<Allergen?> GetAllergenByIdAsync(int id)
    {
        return await myDbContext
            .Allergens
            .Where(allergen => allergen.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Allergen>> GetAllergensByIdsAsync(int[] ids)
    {
        return await myDbContext
            .Allergens
            .Where(allergen => ids.Contains(allergen.Id))
            .ToListAsync();
    }

    public async Task<Allergen> CreateAllergenAsync(CreateAllergenDto createAllergenDto)
    {
        var allergen = new Allergen
        {
            Name = createAllergenDto.Name,
        };

        myDbContext.Allergens.Add(allergen);
        await myDbContext.SaveChangesAsync();

        return allergen;
    }

    public async Task<Allergen?> UpdateAllergenAsync(UpdateAllergenDto updateAllergenDto, int id)
    {
        var allergen = await myDbContext
            .Allergens
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (allergen is null)
        {
            return null;
        }

        allergen.Name = updateAllergenDto.Name;
        await myDbContext.SaveChangesAsync();

        return allergen;
    }
}
