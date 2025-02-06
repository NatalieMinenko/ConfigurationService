namespace ConfigurationService.Presentation.Models.Requests;

public class DeleteSettingsForTheService
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string Service { get; set; }
}
