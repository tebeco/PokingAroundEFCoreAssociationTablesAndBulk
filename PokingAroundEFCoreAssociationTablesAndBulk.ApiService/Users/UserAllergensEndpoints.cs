using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Allergens;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Cards;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Entities;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Users;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Routing;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class UserAllergensEndpoints
{
    public static IEndpointRouteBuilder MapUserAllergens(this IEndpointRouteBuilder app)
    {
        var userAllergensGroup = app.MapGroup("/{userId:int}");

        userAllergensGroup.MapGet("/allergens", GetUserAllergensAsync);
        userAllergensGroup.MapDelete("/allergens/{allergenId:int}", DeleteUserAllergenAsync);

        return app;
    }

    public static async Task<List<AllergenDto>> GetUserAllergensAsync([FromServices] MyDbContext myDbContext, [FromRoute] int userId)
    {
        return await myDbContext
            .Allergens
            .Where(a => a.Id == userId)
            .Select(a => new AllergenDto(a.Id, a.Name))
            .ToListAsync();
    }

    public static async Task<Results<NotFound, Ok<UserDto>>>  DeleteUserAllergenAsync([FromServices] UserService userService, [FromRoute] int userId, [FromRoute] int allergenId)
    { 
        var user = await userService.DeleteUserAllergenAsync(userId, allergenId);

        return user is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new UserDto(
                user.Id,
                user.Name,
                user.Allergens.Select(allergen => new AllergenDto(allergen.Id, allergen.Name)),
                user.Cards.Select(card => new CardDto(card.Id, card.CardNumber))));
    }
}
