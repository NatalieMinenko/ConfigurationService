using ConfigurationService.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationService.Persistence.Repository.Interfaces;

public interface ISettingsRepository
{
    Task<List<Settings>> GetSettingsByServiceAsync(string service);
    Task AddSettingAsync(Settings setting);
    Task UpdateSettingAsync(Settings setting, Settings newSetting);
    Task DeleteSettingAsync(int id);
}
