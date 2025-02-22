using ConfigurationService.Persistence.DTO;

namespace ConfigurationService.Presentation.Models.Requests;

public class SettingCreateRequest
{
    public string Name { get; set; }
    public string Value { get; set; }
    public ServiceName Service { get; set; }
}
