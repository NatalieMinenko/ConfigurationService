using System.Collections.Generic;
using ConfigurationService.Persistence.DTO;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationService.Persistence;

public class SettingsContext(DbContextOptions<SettingsContext> options) : DbContext (options)
{
    public DbSet<Settings> Settings { get; set; }
}

