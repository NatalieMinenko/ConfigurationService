using ConfigurationService.Persistence.DTO;
using ConfigurationService.Persistence.Interfaces;
using ConfigurationService.Presentation.Models;
using ConfigurationService.Presentation.Models.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace ConfigurationService.Tests;

public class SettingsControllersTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly Mock<ISettingsRepository> _repoMock = new();

    public SettingsControllersTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_repoMock.Object);
            });
        });
        _client = _factory.CreateClient();
    }
    [Fact]
    public async Task GetSettingsByService_ReturnsNotFound_WhenServiceIsUnknown()
    {
        // Arrange
        var client = _factory.CreateClient();
        var service = ServiceTypeDto.Unknown;

        // Act
        var response = await _client.GetAsync($"/settings/service/{service}");

        // Assert
        Assert.Equal((int)HttpStatusCode.NotFound, (int)response.StatusCode); // 400
    }
    [Fact]
    public async Task GetSettingsByService_ReturnsOk_ForValidService()
    {
        // Arrange
        var service = ServiceTypeDto.CustomersService;
        
        // Act
        var response = await _client.GetAsync($"/settings/service/{service}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateSetting_ReturnsCreated_WhenSettingIsNotExist()
    {
        // Arrange
        var newSetting = new SettingCreateRequest
        {
            Name = "TestSetting",
            Value = "TestValue",
            Service = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/settings", newSetting);

        // Assert
        response.EnsureSuccessStatusCode(); // 201
        var createdSetting = await response.Content.ReadAsStringAsync();
        Assert.NotNull(createdSetting);
    }
    [Fact]
    public async Task CreateSetting_ReturnsConflict_WhenSettingAlreadyExists()
    {
        // Arrange
        var existingSetting = new SettingCreateRequest
        {
            Name = "TestSetting",
            Value = "TestValue",
            Service = 1
        };

        // Act
        var response1 = await _client.PostAsJsonAsync("/settings", existingSetting);
        var response2 = await _client.PostAsJsonAsync("/settings", existingSetting);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode); // 409
    }
    [Fact]
    public async Task UpdateSetting_ReturnsNoContent_WhenSettingExists()
    {
        // Arrange
        var updateRequest = new SettingUpdateRequest
        {
            Value = "UpdatedValue"
        };
        int settingId = 1;

        // Act
        var response = await _client.PatchAsJsonAsync($"/settings/{settingId}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode); // 204
    }
        [Fact]
    public async Task UpdateSetting_ReturnsNotFound_WhenSettingDoesNotExist()
    {
        // Arrange
        var updateRequest = new SettingUpdateRequest
        {
            Value = "UpdatedValue"
        };
        int nonExistentSettingId = 999;

        // Act
        var response = await _client.PatchAsJsonAsync($"/settings/{nonExistentSettingId}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); // 404
    }
    [Fact]
    public async Task DeleteSetting_ReturnsNoContent_WhenSettingIsDeleted()
    {
        // Arrange
        int existingSettingId = 2;

        // Act
        var response = await _client.DeleteAsync($"/settings/{existingSettingId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); // 204
    }

    [Fact]
    public async Task DeleteSetting_ReturnsNotFound_WhenSettingDoesNotExist()
    {
        // Arrange
        int nonExistentSettingId = 999;

        // Act
        var response = await _client.DeleteAsync($"/settings/{nonExistentSettingId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); // 404
    }
}

