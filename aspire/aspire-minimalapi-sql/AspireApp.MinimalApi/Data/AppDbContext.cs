using AspireApp.MinimalApi.Domain;
using Microsoft.EntityFrameworkCore;
using System.Xml;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Person> People { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().HasKey(p => p.Id);
        // Optionally, configure Id as identity/auto-increment:
        modelBuilder.Entity<Person>().Property(p => p.Id).ValueGeneratedOnAdd();
    }
}
