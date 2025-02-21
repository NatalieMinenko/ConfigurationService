
using ConfigurationService.Persistence.DTO;
using ConfigurationService.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Microsoft.Extensions.DependencyInjection;

using System.Text.Json;
using ConfigurationService.Presentation;
using System.Net;

namespace ConfigurationService.Tests
{
    public class EndpointsTest
    {
        private readonly Mock<ISettingsRepository> _repoMock = new();



        [Fact]
        public async Task TestRootEndpoint()
        {
            //
            var setting = new SettingsDto() {
                Id = 1,
                Name = "name",
                Value = "value",
                Service = ServiceTypeDto.CustomersService
            };
            var service = ServiceTypeDto.CustomersService;
            var settingAsString = JsonSerializer.Serialize(setting);

            await using var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => builder
               .ConfigureServices(services =>
               {
                   services.AddSingleton<ISettingsRepository, FakeSettingsRepository>();            
               }));
            using var client = application.CreateClient();
            //act
            var response = await client.GetAsync($"/settings/?service=ServiceTypeDto.CustomersService");
            var t = response;
            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Assert.Equivalent(settingAsString, response);
        }
    }
}
