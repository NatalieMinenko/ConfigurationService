using ConfigurationService.Persistence.DTO;
using ConfigurationService.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationService.Persistence.Repository;

public class SettingsRepository : ISettingsRepository
{
    private readonly SettingsContext _context;

    public SettingsRepository(SettingsContext context)
    {
        _context = context;
    }

    public async Task<SettingsDto> GetSettingsByServiceAsync(ServiceTypeDto service)
    {
        return await _context.Settings.FirstOrDefaultAsync(s => s.Service == service);
    }

    public async Task<SettingsDto> GetSettingByIdAsync(int id)
    {
        return await _context.Settings.FindAsync(id);
    }

    public async Task<SettingsDto> GetSettingByNameAsync(string name)
    {
        return await _context.Settings.FirstOrDefaultAsync(s => s.Name == name);
    }

    public async Task AddSettingAsync(SettingsDto setting)
    {
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSettingAsync(SettingsDto setting)
    {
        _context.Settings.Update(setting);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSettingAsync(int id)
    {
        var setting = await _context.Settings.FindAsync(id);
        if (setting != null)
        {
            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();
        }
    }
}
