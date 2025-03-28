﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Cards;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Routing;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class CardEndpoints
{
    public static void MapCards(this IEndpointRouteBuilder app)
    {
        var allergenGroup = app.MapGroup("/cards");

        allergenGroup.MapGet("/", GetCardsAsync);
        allergenGroup.MapGet("/{id:int}", GetCardByIdAsync).WithName(nameof(GetCardByIdAsync));
        allergenGroup.MapPost("/", CreateCardAsync);
        allergenGroup.MapPut("/{id:int}", UpdateCardAsync);
    }

    public static async Task<Ok<List<CardDto>>> GetCardsAsync([FromServices] CardService cardService)
    {
        var cards = await cardService.GetCardsAsync();

        return TypedResults.Ok(cards.Select(c => new CardDto(c.Id, c.CardNumber)).ToList());
    }

    public static async Task<Results<NotFound, Ok<CardDto>>> GetCardByIdAsync([FromServices] CardService cardService, int id)
    {
        var card = await cardService.GetCardByIdAsync(id);

        return card is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new CardDto(card.Id, card.CardNumber));
    }

    public static async Task<Created<CardDto>> CreateCardAsync(
        HttpContext context,
        [FromBody] CreateCardDto createCardDto,
        [FromServices] LinkGenerator linkGenerator,
        [FromServices] CardService cardService
        )
    {
        var card = await cardService.CreateCardAsync(createCardDto);

        var customerLink = linkGenerator.GetUriByName(context, nameof(GetCardByIdAsync), new { id = card.Id });
        return TypedResults.Created(customerLink, new CardDto(card.Id, card.CardNumber));
    }

    public static async Task<Results<NotFound, Ok<CardDto>>> UpdateCardAsync([FromServices] CardService cardService, [FromRoute] int id, [FromBody] UpdateCardDto updateCardDto)
    {
        var card = await cardService.UpdateCardAsync(id, updateCardDto);

        return card is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new CardDto(card.Id, card.CardNumber));
    }
}
