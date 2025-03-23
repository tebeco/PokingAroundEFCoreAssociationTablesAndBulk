using PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Allergens;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class AllergenServiceCollectionExtensions
{
    public static IServiceCollection AddAllergens(this IServiceCollection services)
    {
        services.AddTransient<AllergenService>();

        return services;
    }
}
