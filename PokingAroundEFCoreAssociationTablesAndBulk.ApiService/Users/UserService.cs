

using Microsoft.EntityFrameworkCore;

using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Allergens;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Entities;

namespace PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Users;

public partial class UserService(MyDbContext myDbContext, AllergenService allergenService)
{
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await myDbContext
            .Users
            .Include(u => u.Allergens)
            .Include(u => u.Cards)
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await myDbContext
            .Users
            .Where(user => user.Id == id)
            .Include(user => user.Allergens)
            .Include(u => u.Cards)
            .FirstOrDefaultAsync();
    }

    public async Task<User> CreateUserAsync(CreateUserDto createUserDto)
    {
        var allergens = await allergenService.GetAllergensByIdsAsync(createUserDto.AllergenIds);

        var user = new User
        {
            Name = createUserDto.Name,
            Allergens = allergens,
            Cards = [.. createUserDto.Cards.Select(c => new Card { CardNumber = c.CardNumber })]
        };

        myDbContext.Users.Add(user);
        await myDbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var user = await myDbContext
            .Users
            .Where(user => user.Id == id)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return null;
        }

        user.Name = updateUserDto.Name;
        await myDbContext.SaveChangesAsync();

        return user;
    }

    public async Task<List<User>> GetUserAllergensAsync(int userId)
    {
        return await myDbContext
            .Users
            .Where(u => u.Id == userId)
            .Include(u => u.Allergens)
            .ToListAsync();
    }

    public async Task<Allergen?> GetUserAllergenByIdAsync(int userId, int allergenId)
    {
        return await myDbContext
            .Allergens
            .Include(a => a.Users.Where(u => u.Id == userId))
            .FirstOrDefaultAsync(a => a.Id == allergenId);
    }

    public async Task<User?> CreateUserAllergenAsync(int userId, int allergenId)
    {
        var user = await myDbContext
            .Users
            .Include(u => u.Allergens)
            .Where(a => a.Id == userId)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return null;
        }

        var allergen = await myDbContext
            .Allergens
            .Where(a => a.Id == allergenId)
            .FirstOrDefaultAsync();

        if (allergen is null)
        {
            return null;
        }

        user.Allergens.Add(allergen);
        await myDbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> DeleteUserAllergenAsync(int userId, int allergenId)
    {
        var user = await myDbContext
            .Users
            .Where(user => user.Id == userId)
            .Include(u => u.Allergens)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return null;
        }

        var allergen = user.Allergens.FirstOrDefault(a => a.Id == allergenId);

        if (allergen is null)
        {
            return null;
        }

        user.Allergens.Remove(allergen);
        await myDbContext.SaveChangesAsync();

        return user;
    }

    internal async Task<User?> UpdateUserAllergensAsync(int userId, int[] allergenIds)
    {
        var user = await myDbContext
            .Users
            .Include(u => u.Allergens)
            .Where(a => a.Id == userId)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return null;
        }

        var allergens = await myDbContext
            .Allergens
            .Where(a => allergenIds.Contains(a.Id))
            .ToListAsync();

        user.Allergens.Clear();
        user.Allergens.AddRange(allergens);
        await myDbContext.SaveChangesAsync();

        return user;
    }
}
