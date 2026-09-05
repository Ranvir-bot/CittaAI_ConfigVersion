using ConfigurationVersioning.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationVersioning.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Configuration> Configurations { get; set; }

    public DbSet<ConfigurationVersion> ConfigurationVersions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Configuration>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<ConfigurationVersion>()
            .HasKey(x => x.Id);


        // One Configuration can have many ConfigurationVersions.
        // Each ConfigurationVersion belongs to one Configuration.
        modelBuilder.Entity<Configuration>()
            .HasMany(x => x.Versions)
            .WithOne(x => x.Configuration)
            .HasForeignKey(x => x.ConfigurationId)
            .OnDelete(DeleteBehavior.Cascade);

        // A Configuration cannot have duplicate VersionNumbers.
        // Example:
        // ConfigurationId = 1, VersionNumber = 1  -> Allowed
        // ConfigurationId = 1, VersionNumber = 2  -> Allowed
        // ConfigurationId = 1, VersionNumber = 1  -> Not Allowed
        // ConfigurationId = 2, VersionNumber = 1  -> Allowed
        modelBuilder.Entity<ConfigurationVersion>()
            .HasIndex(x => new
            {
                x.ConfigurationId,
                x.VersionNumber
            })
            .IsUnique();

        // Creates an index on PreviousVersionId
        // to make previous-version lookups more efficient.
        modelBuilder.Entity<ConfigurationVersion>()
            .HasIndex(x => x.PreviousVersionId);
    }
}