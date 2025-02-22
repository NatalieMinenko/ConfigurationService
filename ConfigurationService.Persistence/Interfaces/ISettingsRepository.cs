using ConfigurationService.Persistence.DTO;

namespace ConfigurationService.Persistence.Interfaces;

public interface ISettingsRepository
{
    Task<Settings> GetSettingsByServiceAsync(ServiceName service);
    Task AddSettingAsync(Settings setting);
    Task UpdateSettingAsync(Settings setting);
    Task DeleteSettingAsync(int id);
    Task<Settings> GetSettingByIdAsync(int id);
    Task<Settings> GetSettingByNameAndServiceNameAsync(string name, ServiceName service);
}
