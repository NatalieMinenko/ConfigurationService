namespace ConfigurationService.Presentation.Models.Requests;

public class GetSettingsForTheService
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string Service { get; set; }
}
