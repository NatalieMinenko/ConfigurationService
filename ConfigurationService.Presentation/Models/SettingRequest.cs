namespace ConfigurationService.Presentation.Models;

public class SettingRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string Service { get; set; }
}
