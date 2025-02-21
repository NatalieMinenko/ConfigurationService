using ConfigurationService.Persistence.Interfaces;
using ConfigurationService.Persistence.DTO;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace ConfigurationService.Tests;

public class FakeSettingsRepository : ISettingsRepository
{
    private SettingsDto settings = new SettingsDto()
    {
        Id = 1,
        Name = "name",
        Value = "value",
        Service = ServiceTypeDto.CustomersService
    };

    public async Task<SettingsDto> GetSettingsByServiceAsync(ServiceTypeDto service) 
    {
        await Task.Delay(1000);
        //if (service == ServiceTypeDto.CustomersService) return null;
       
        return settings;
    }
    public async Task AddSettingAsync(SettingsDto setting)
    { }
    public async Task UpdateSettingAsync(SettingsDto setting)
    { }
    public async Task DeleteSettingAsync(int id)
    { }
    public async Task<SettingsDto> GetSettingByIdAsync(int id = 1) 
    {
        await Task.Delay(1000);
        return settings;
    }
    public async Task<SettingsDto> GetSettingByNameAsync(string name) 
    {
        await Task.Delay(1000);
        return settings;
    }
}