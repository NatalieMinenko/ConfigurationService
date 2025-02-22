using System.Collections.Generic;
using ConfigurationService.Persistence.DTO;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationService.Persistence;

public class SettingsContext(DbContextOptions<SettingsContext> options) : DbContext (options)
{
    public DbSet<Settings> Settings { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Settings>()
            .HasIndex(s => s.Id)
            .IsUnique();
        modelBuilder.Entity<Settings>()
            .Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);
        modelBuilder.Entity<Settings>()
            .Property(s => s.Value)
            .IsRequired()
            .HasMaxLength(200);
        modelBuilder.Entity<Settings>()
            .Property(s => s.Service)
            .IsRequired();
    }
}

