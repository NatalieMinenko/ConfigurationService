using ConfigurationService.Persistence.DTO;

namespace ConfigurationService.Persistence.Interfaces;

public interface ISettingsRepository
{
    Task<List<Settings>> GetSettingsByServiceAsync(string service);
    Task AddSettingAsync(Settings setting);
    Task UpdateSettingAsync(Settings setting, Settings newSetting);
    Task DeleteSettingAsync(int id);
    Task<Settings> GetSettingByIdAsync(int id);
}
