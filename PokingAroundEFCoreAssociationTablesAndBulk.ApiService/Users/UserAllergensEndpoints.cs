using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Allergens;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Cards;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Users;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Routing;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class UserAllergensEndpoints
{
    public static IEndpointRouteBuilder MapUserAllergens(this IEndpointRouteBuilder app)
    {
        var userAllergensGroup = app.MapGroup("/{userId:int}/allergens");

        userAllergensGroup.MapGet("/", GetUserAllergensAsync);
        userAllergensGroup.MapGet("/{allergenId:int}", GetUserAllergenByIdAsync);
        userAllergensGroup.MapPost("/{allergenId:int}", CreateUserAllergenAsync);
        userAllergensGroup.MapDelete("/{allergenId:int}", DeleteUserAllergenAsync);

        userAllergensGroup.MapPut("/", UpdateUserAllergensAsync);

        return app;
    }

    public static async Task<Results<NotFound, Ok<UserDto>>> UpdateUserAllergensAsync([FromServices] UserService userService, [FromRoute] int userId, [FromBody] int[] allergenIds)
    {
        var user = await userService.UpdateUserAllergensAsync(userId, allergenIds);

        return user is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new UserDto(
                user.Id,
                user.Name,
                user.Allergens.Select(allergen => new AllergenDto(allergen.Id, allergen.Name)),
                user.Cards.Select(card => new CardDto(card.Id, card.CardNumber))));
    }

    public static async Task<Ok<List<AllergenDto>>> GetUserAllergensAsync([FromServices] UserService userService, [FromRoute] int userId)
    {
        var userAllergens = await userService.GetUserAllergensAsync(userId);

        return TypedResults.Ok(userAllergens.Select(a => new AllergenDto(a.Id, a.Name)).ToList());
    }

    public static async Task<Results<NotFound, Ok<AllergenDto>>> GetUserAllergenByIdAsync([FromServices] UserService userService, [FromRoute] int userId, [FromRoute] int allergenId)
    {
        var userAllergen = await userService.GetUserAllergenByIdAsync(userId, allergenId);

        return userAllergen is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new AllergenDto(userAllergen.Id, userAllergen.Name));
    }


    public static async Task<Results<NotFound, Ok<UserDto>>> CreateUserAllergenAsync([FromServices] UserService userService, [FromRoute] int userId, [FromRoute] int allergenId)
    {
        var user = await userService.CreateUserAllergenAsync(userId, allergenId);

        return user is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new UserDto(
                user.Id,
                user.Name,
                user.Allergens.Select(allergen => new AllergenDto(allergen.Id, allergen.Name)),
                user.Cards.Select(card => new CardDto(card.Id, card.CardNumber))));
    }

    public static async Task<Results<NotFound, Ok<UserDto>>> DeleteUserAllergenAsync([FromServices] UserService userService, [FromRoute] int userId, [FromRoute] int allergenId)
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
