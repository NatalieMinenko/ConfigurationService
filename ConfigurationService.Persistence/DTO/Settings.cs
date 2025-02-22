namespace ConfigurationService.Persistence.DTO;

public class Settings
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public ServiceName Service { get; set; }
}
