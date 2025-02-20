using ConfigurationService.Persistence.DTO;
using ConfigurationService.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigurationService.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using ConfigurationService.Presentation.Models;
namespace ConfigurationService.Tests;

public class SettingsRepositoryTest
{
    private readonly SettingsContext _context;
    private readonly SettingsRepository _settingsRepository;

    public SettingsRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<SettingsContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new SettingsContext(options);
        _settingsRepository = new SettingsRepository(_context);
    }

    [Fact]
    public async Task GetSettingByIdAsync_ReturnsSetting_WhenExists()
    {
        // Arrange
        var settingId = 2;
        var expectedSetting = new SettingsDto
        {
            Id = settingId,
            Name = "TestSetting",
            Value = "TestValue",
            Service = ServiceTypeDto.CustomersService
        };

        await _context.Settings.AddAsync(expectedSetting);
        await _context.SaveChangesAsync();

        // Act
        var result = await _settingsRepository.GetSettingByIdAsync(settingId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSetting.Id, result.Id);
        Assert.Equal(expectedSetting.Name, result.Name);
        Assert.Equal(expectedSetting.Value, result.Value);
        Assert.Equal(expectedSetting.Service, result.Service);
    }

    [Fact]
    public async Task GetSettingByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Act
        var result = await _settingsRepository.GetSettingByIdAsync(10);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetSettingsByServiceAsync_ReturnsSetting_WhenExists()
    {
        // Arrange
        var expectedSetting = new SettingsDto
        {
            Name = "TestSetting",
            Value = "TestValue",
            Service = ServiceTypeDto.CustomersService
        };

        await _context.Settings.AddAsync(expectedSetting);
        await _context.SaveChangesAsync();

        // Act
        var result = await _settingsRepository.GetSettingsByServiceAsync(ServiceTypeDto.CustomersService);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSetting.Name, result.Name);
        Assert.Equal(expectedSetting.Value, result.Value);
        Assert.Equal(expectedSetting.Service, result.Service);
    }

    [Fact]
    public async Task GetSettingsByServiceAsync_ReturnsNull_WhenNotExists()
    {
        // Act
        var result = await _settingsRepository.GetSettingsByServiceAsync(ServiceTypeDto.Unknown);

        // Assert
        Assert.Null(result);
    }
    [Fact]
    public async Task GetSettingByNameAsync_ReturnsSetting_WhenExists()
    {
        // Arrange
        var setting = new SettingsDto
        {
            Id = 17,
            Name = "TestSetting",
            Value = "TestValue",
            Service = ServiceTypeDto.CustomersService
        };

        await _context.Settings.AddAsync(setting);
        await _context.SaveChangesAsync();

        // Act
        var result = await _settingsRepository.GetSettingByNameAsync("TestSetting");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(setting.Id, result.Id);
        Assert.Equal(setting.Name, result.Name);
        Assert.Equal(setting.Value, result.Value);
        Assert.Equal(setting.Service, result.Service);
    }

    [Fact]
    public async Task GetSettingByNameAsync_ReturnsNull_WhenDoesNotExist()
    {
        // Act
        var result = await _settingsRepository.GetSettingByNameAsync("NonExistentSetting");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddSettingAsync_AddsSetting()
    {
        // Arrange
        var setting = new SettingsDto
        {
            Id = 15,
            Name = "NewSetting",
            Value = "NewValue",
            Service = ServiceTypeDto.TransactionsStore
        };

        // Act
        await _settingsRepository.AddSettingAsync(setting);
        var result = await _context.Settings.FindAsync(setting.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(setting.Name, result.Name);
        Assert.Equal(setting.Value, result.Value);
        Assert.Equal(setting.Service, result.Service);
    }

    [Fact]
    public async Task UpdateSettingAsync_UpdatesSetting_WhenExists()
    {
        // Arrange
        var setting = new SettingsDto
        {
            Id = 3,
            Name = "UpdateSetting",
            Value = "OldValue",
            Service = ServiceTypeDto.RatesProvider
        };

        await _context.Settings.AddAsync(setting);
        await _context.SaveChangesAsync();

        setting.Value = "UpdatedValue";

        // Act
        await _settingsRepository.UpdateSettingAsync(setting);
        var updatedSetting = await _context.Settings.FindAsync(setting.Id);

        // Assert
        Assert.NotNull(updatedSetting);
        Assert.Equal("UpdatedValue", updatedSetting.Value);
    }

    [Fact]
    public async Task DeleteSettingAsync_RemovesSetting_WhenExists()
    {
        // Arrange
        var setting = new SettingsDto
        {
            Id = 2,
            Name = "DeleteSetting",
            Value = "ValueToDelete",
            Service = ServiceTypeDto.ReportingService
        };

        await _context.Settings.AddAsync(setting);
        await _context.SaveChangesAsync();

        // Act
        await _settingsRepository.DeleteSettingAsync(setting.Id);
        var deletedSetting = await _context.Settings.FindAsync(setting.Id);

        // Assert
        Assert.Null(deletedSetting);
    }
    [Fact]
    public async Task DeleteSettingAsync_DoesNotThrow_WhenDoesNotExist()
    {
        // Act
        await _settingsRepository.DeleteSettingAsync(999);

        // Assert
        // No exceptions should be thrown
    }
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
