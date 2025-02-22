
using ConfigurationService.Persistence.DTO;
using ConfigurationService.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Microsoft.Extensions.DependencyInjection;

using System.Text.Json;
using ConfigurationService.Presentation;
using System.Net;
using ConfigurationService.Persistence.Repository;

namespace ConfigurationService.Tests
{
    public class EndpointsTest
    {
        private readonly Mock<ISettingsRepository> _repoMock = new();



        [Fact]
        public async Task TestRootEndpoint()
        {
            //
            var setting = new SettingsDto()
            {
                Id = 1,
                Name = "name",
                Value = "value",
                Service = ServiceTypeDto.CustomersService
            };
            await using var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => builder
               .ConfigureServices(services =>
               {
                   services.AddScoped<ISettingsRepository,FakeSettingsRepository>();            
               }));
            using var client = application.CreateClient();
            //act
            var response = await client.GetAsync($"/settings/?service=0");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SettingsDto>(
                content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(setting, result);
        }
    }
}
