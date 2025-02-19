using ConfigurationService.Persistence.DTO;

namespace ConfigurationService.Persistence.Interfaces;

public interface ISettingsRepository
{
    Task<SettingsDto> GetSettingsByServiceAsync(ServiceTypeDto service);
    Task AddSettingAsync(SettingsDto setting);
    Task UpdateSettingAsync(SettingsDto setting);
    Task DeleteSettingAsync(int id);
    Task<SettingsDto> GetSettingByIdAsync(int id);
    Task<SettingsDto> GetSettingByNameAsync(string name);
}
