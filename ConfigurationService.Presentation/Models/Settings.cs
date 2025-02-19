namespace ConfigurationService.Presentation.Models;

public class Settings
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public ServiceType Service { get; set; }
}
