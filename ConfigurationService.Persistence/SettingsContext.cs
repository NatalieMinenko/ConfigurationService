using System.Collections.Generic;
using ConfigurationService.Persistence.DTO;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationService.Persistence;

public class SettingsContext : DbContext
{
    public SettingsContext(DbContextOptions<SettingsContext> options) : base(options)
    {
    }

    public DbSet<Settings> Settings { get; set; }
}

