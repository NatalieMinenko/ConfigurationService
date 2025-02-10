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

    public async Task<List<Settings>> GetSettingsByServiceAsync(string service)
    {
        return await _context.Settings.Where(s => s.Service == service).ToListAsync();
    }

    public async Task AddSettingAsync(Settings setting)
    {
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateSettingAsync(Settings setting, Settings newSetting)
    {
        setting.Name = newSetting.Name;
        setting.Value = newSetting.Value;
        setting.Service = newSetting.Service;
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
    public async Task<Settings> GetSettingByIdAsync(int id)
    {
        return await _context.Settings.FindAsync(id);
    }
}
