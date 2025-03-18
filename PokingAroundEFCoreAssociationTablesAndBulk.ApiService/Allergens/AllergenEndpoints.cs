using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Allergens;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Routing;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class AllergenEndpoints
{
    public static IEndpointRouteBuilder MapAllergens(this IEndpointRouteBuilder app)
    {
        var allergenGroup = app.MapGroup("/allergens");

        allergenGroup.MapGet("/", GetAllergensAsync);
        allergenGroup.MapGet("/{id:int}", GetAllergenByIdAsync);
        allergenGroup.MapPost("/", CreateAllergenAsync);
        allergenGroup.MapPut("/{id:int}", UpdateAllergenAsync);

        return app;
    }

    public static async Task<List<AllergenDto>> GetAllergensAsync([FromServices] AllergenService allergenService)
    {
        var allergens = await allergenService.GetAllergensAsync();

        return [.. allergens.Select(u => new AllergenDto(u.Id, u.Name))];
    }

    public static async Task<Results<NotFound, Ok<AllergenDto>>> GetAllergenByIdAsync([FromServices] AllergenService allergenService, int id)
    {
        var allergen = await allergenService.GetAllergenByIdAsync(id);

        return allergen is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new AllergenDto(allergen.Id, allergen.Name));
    }

    public static async Task<Ok<AllergenDto>> CreateAllergenAsync([FromServices] AllergenService allergenService, [FromBody] CreateAllergenDto createAllergenDto)
    {
        var allergen = await allergenService.CreateAllergenAsync(createAllergenDto);

        return TypedResults.Ok(new AllergenDto(allergen.Id, allergen.Name));
    }

    public static async Task<Results<NotFound, Ok<AllergenDto>>> UpdateAllergenAsync([FromServices] AllergenService allergenService, [FromBody] UpdateAllergenDto updateAllergenDto, int id)
    {
        var allergen = await allergenService.UpdateAllergenAsync(updateAllergenDto, id);

        return allergen is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new AllergenDto(allergen.Id, allergen.Name));
    }
}
