using ConfigurationService.Persistence.DTO;
using ConfigurationService.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConfigurationService.Persistence.Repository;

public class SettingsRepository(ILogger<SettingsRepository> logger) : ISettingsRepository
{
    private readonly SettingsContext _context;

    public SettingsRepository(SettingsContext context, ILogger<SettingsRepository> logger) : this(logger)
    {
        _context = context;
    }

    public async Task<Settings> GetSettingsByServiceAsync(ServiceName service)
    {
        logger.LogInformation($"received service type: {service}");
        return await _context.Settings.FirstOrDefaultAsync(s => s.Service == service);
    }

    public async Task<Settings> GetSettingByIdAsync(int id)
    {
        logger.LogInformation($"received service id: {id}");
        return await _context.Settings.FindAsync(id);
    }

    public async Task<Settings> GetSettingByNameAndServiceNameAsync(string name, ServiceName service)
    {
        logger.LogInformation($"received service name: {name}");
        return await _context.Settings.FirstOrDefaultAsync(s => s.Name == name && s.Service == service);
    }

    public async Task AddSettingAsync(Settings setting)
    {
        logger.LogInformation($"add service: {setting.Name}");
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSettingAsync(Settings setting)
    {
        logger.LogInformation($"update service: {setting.Name}");
        _context.Settings.Update(setting);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSettingAsync(int id)
    {
        logger.LogInformation($"delete service id: {id}");
        var setting = await _context.Settings.FindAsync(id);
        if (setting != null)
        {
            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();
        }
    }
}
