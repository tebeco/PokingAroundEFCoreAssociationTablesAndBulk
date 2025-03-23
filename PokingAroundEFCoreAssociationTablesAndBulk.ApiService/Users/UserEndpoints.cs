using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Allergens;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Cards;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Users;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Routing;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUsers(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup("/users");

        userGroup.MapGet("/", GetAllUsersAsync);
        userGroup.MapGet("/{id:int}", GetUserByIdAsync);
        userGroup.MapPost("/", CreateUserAsync);
        userGroup.MapPut("/{id:int}", UpdateUserAsync);

        userGroup.MapUserAllergens();

        return app;
    }

    public static async Task<Ok<List<UserDto>>> GetAllUsersAsync([FromServices] UserService userService)
    {
        var users = await userService.GetAllUsersAsync();
        return TypedResults.Ok(users
                .Select(u => new UserDto(
                    u.Id,
                    u.Name,
                    u.Allergens.Select(a => new AllergenDto(a.Id, a.Name)),
                    u.Cards.Select(c => new CardDto(c.Id, c.CardNumber))
                ))
                .ToList());
    }

    public static async Task<Results<NotFound, Ok<UserDto>>> GetUserByIdAsync([FromServices] UserService userService, [FromRoute] int id)
    {
        var user = await userService.GetUserByIdAsync(id);

        return user is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new UserDto(
                user.Id,
                user.Name,
                user.Allergens.Select(allergen => new AllergenDto(allergen.Id, allergen.Name)),
                user.Cards.Select(card => new CardDto(card.Id, card.CardNumber))));
    }

    public static async Task<Created<UserDto>> CreateUserAsync([FromServices] UserService userService, [FromBody] CreateUserDto createUserDto)
    {
        var user = await userService.CreateUserAsync(createUserDto);

        var userDto = new UserDto(
                user.Id,
                user.Name,
                user.Allergens.Select(allergen => new AllergenDto(allergen.Id, allergen.Name)),
                user.Cards.Select(card => new CardDto(card.Id, card.CardNumber)));

        return TypedResults.Created($"{userDto.Id}", userDto);
    }

    public static async Task<Results<NotFound, Ok<UserDto>>> UpdateUserAsync([FromServices] UserService userService, [FromRoute] int id, [FromBody] UpdateUserDto updateUserDto)
    {
        var user = await userService.UpdateUserAsync(id, updateUserDto);

        return user is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new UserDto(
                user.Id,
                user.Name,
                user.Allergens.Select(a => new AllergenDto(a.Id, a.Name)),
                user.Cards.Select(c => new CardDto(c.Id, c.CardNumber))));
    }
}
