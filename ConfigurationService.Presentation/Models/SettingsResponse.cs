using ConfigurationService.Persistence.DTO;

namespace ConfigurationService.Presentation.Models;

public class SettingsResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public ServiceName Service { get; set; }
}
