namespace ConfigurationService.Persistence.DTO;

public class SettingsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public ServiceTypeDto Service { get; set; }
}
