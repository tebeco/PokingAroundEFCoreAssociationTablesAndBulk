using Microsoft.EntityFrameworkCore;

namespace PokingAroundEFCoreAssociationTablesAndBulk.ApiService.Entities;

public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }

    public required DbSet<Allergen> Allergens { get; set; }

    public required DbSet<Card> Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<User>().HasMany<Allergen>().WithMany();
    }
}
