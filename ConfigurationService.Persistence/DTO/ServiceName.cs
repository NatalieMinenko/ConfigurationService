using System.Text.Json.Serialization;

namespace ConfigurationService.Persistence.DTO;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum ServiceName
{
    Unknown = 0,
    CustomersService,
    TransactionsStore,
    RatesProvider,
    ReportingService,
    CustomersRoleUpdater
}
