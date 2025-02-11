using ConfigurationService.Persistence.DTO;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationService.Persistence;

public class SettingsContext(DbContextOptions<SettingsContext> options) : DbContext(options)
{
    public DbSet<Setting> Settings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setting>()
            .HasIndex(s => s.Id)
            .IsUnique();
        modelBuilder.Entity<Setting>()
            .Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);
        modelBuilder.Entity<Setting>()
            .Property(s => s.Value)
            .IsRequired()
            .HasMaxLength(200);
        modelBuilder.Entity<Setting>()
            .Property(s => s.Service)
            .IsRequired()
            .HasMaxLength(200);
    }
}

