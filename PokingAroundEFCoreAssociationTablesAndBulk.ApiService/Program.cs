using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Entities;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<MyDbContext>((sp, optionsBuilder) =>
{
    var connectionString = builder.Configuration.GetConnectionString("postgresdb");
    optionsBuilder.UseNpgsql(connectionString);
    optionsBuilder.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>());

    optionsBuilder.UseAsyncSeeding(async (dbContext, _, _) =>
    {
        var fish = new Allergen { Name = "Fish" };
        var milk = new Allergen { Name = "Milk" };
        var soy = new Allergen { Name = "Soy" };

        var cardOne = new Card { CardNumber = "1234" };
        var cardTwo = new Card { CardNumber = "ABCD" };

        var bob = new User { Name = "bob", Cards = [cardOne], Allergens = [fish] };
        var jim = new User { Name = "jim", Cards = [cardTwo], Allergens = [fish, milk] };

        dbContext.Set<Allergen>().Add(fish);
        dbContext.Set<Allergen>().Add(milk);
        dbContext.Set<Allergen>().Add(soy);

        dbContext.Set<Card>().Add(cardOne);
        dbContext.Set<Card>().Add(cardTwo);

        dbContext.Set<User>().Add(bob);
        dbContext.Set<User>().Add(jim);

        await dbContext.SaveChangesAsync(default);
    });
});

builder.Services.AddUsers();
builder.Services.AddCards();
builder.Services.AddAllergens();

var app = builder.Build();

app.UseExceptionHandler();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseSwaggerUI(c => c.SwaggerEndpoint($"/{OpenApiConstants.OpenApi}/v1.json", "v1"));

app.MapDefaultEndpoints();

app.MapAllergens();
app.MapUsers();
app.MapCards();

await using (var scope = app.Services.CreateAsyncScope())
{
    await using var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    await dbContext.Database.EnsureDeletedAsync();
    await dbContext.Database.EnsureCreatedAsync();
    //await dbContext.Database.MigrateAsync();
}

app.Run();
