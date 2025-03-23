using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Allergens;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Cards;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Users;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Routing;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class AllergenEndpoints
{
    public static IEndpointRouteBuilder MapAllergens(this IEndpointRouteBuilder app)
    {
        var allergenGroup = app.MapGroup("/allergens");

        allergenGroup.MapGet("/", GetAllergensAsync);
        allergenGroup.MapGet("/{allergenId:int}", GetAllergenByIdAsync);
        allergenGroup.MapGet("/{allergenId:int}/users", GetUsersForAllergenAsync);
        allergenGroup.MapPost("/", CreateAllergenAsync);
        allergenGroup.MapPut("/{allergenId:int}", UpdateAllergenAsync);
        allergenGroup.MapDelete("/{allergenId:int}", DeleteAllergenAsync);

        return app;
    }

    public static async Task<List<AllergenDto>> GetAllergensAsync([FromServices] AllergenService allergenService)
    {
        var allergens = await allergenService.GetAllergensAsync();

        return [.. allergens.Select(u => new AllergenDto(u.Id, u.Name))];
    }

    public static async Task<Results<NotFound, Ok<AllergenDto>>> GetAllergenByIdAsync([FromServices] AllergenService allergenService, int allergenId)
    {
        var allergen = await allergenService.GetAllergenByIdAsync(allergenId);

        return allergen is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new AllergenDto(allergen.Id, allergen.Name));
    }

    public static async Task<List<UserDto>> GetUsersForAllergenAsync([FromServices] AllergenService allergenService, int allergenId)
    {
        var users = await allergenService.GetUsersForAllergenAsync(allergenId);

        return [.. users
            .Select(u => new UserDto(
                u.Id,
                u.Name,
                u.Allergens.Select(a => new AllergenDto(a.Id, a.Name)),
                u.Cards.Select(c => new CardDto(c.Id, c.CardNumber))
            ))];
    }
    

    public static async Task<Ok<AllergenDto>> CreateAllergenAsync([FromServices] AllergenService allergenService, [FromBody] CreateAllergenDto createAllergenDto)
    {
        var allergen = await allergenService.CreateAllergenAsync(createAllergenDto);

        return TypedResults.Ok(new AllergenDto(allergen.Id, allergen.Name));
    }

    public static async Task<Results<NotFound, Ok<AllergenDto>>> UpdateAllergenAsync([FromServices] AllergenService allergenService, [FromBody] UpdateAllergenDto updateAllergenDto, int allergenId)
    {
        var allergen = await allergenService.UpdateAllergenAsync(updateAllergenDto, allergenId);

        return allergen is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new AllergenDto(allergen.Id, allergen.Name));
    }

    public static async Task<Results<NotFound, Ok>> DeleteAllergenAsync([FromServices] AllergenService allergenService, int allergenId)
    {
        var affectedRows = await allergenService.DeleteAllergenAsync(allergenId);

        return affectedRows > 0
            ? TypedResults.Ok()
            : TypedResults.NotFound();
    }
}
