using Microsoft.EntityFrameworkCore;
using ConfigurationService.Persistence.Repository;
using ConfigurationService.Persistence.Interfaces;
using ConfigurationService.Persistence;
using ConfigurationService.Persistence.DTO;
using ConfigurationService.Presentation.Models.Requests;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationService.Presentation;
public  class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.secrets.json", optional: true, reloadOnChange: true)
            .AddCommandLine(args)
            .AddEnvironmentVariables()
            .Build();

        builder.Services.AddDbContext<SettingsContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("ConfigurationServiceDb")));

        builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.MapGet("/settings", async (
            [FromQuery] ServiceTypeDto service,
            ISettingsRepository repository
            ) =>
        {
            var settings = await repository.GetSettingsByServiceAsync(service);
            if (settings == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(settings);
        });

        app.MapPost("/settings", async (ISettingsRepository repository, SettingCreateRequest request) =>
        {
            var existingSetting = await repository.GetSettingByNameAsync(request.Name);
            if (existingSetting != null)
            {
                return Results.Conflict("Setting with the name already exists for the service");
            }

            var setting = new SettingsDto
            {
                Name = request.Name,
                Value = request.Value,
                Service = (ServiceTypeDto)request.Service
            };

            await repository.AddSettingAsync(setting);
            return Results.Created($"/settings/{setting.Id}", setting);

        });

        app.MapPatch("/settings/{id}", async (ISettingsRepository repository, int id, SettingUpdateRequest request) =>
        {
            var setting = await repository.GetSettingByIdAsync(id);
            if (setting == null)
            {
                return Results.NotFound();
            }

            setting.Value = request.Value;
            await repository.UpdateSettingAsync(setting);

            return Results.NoContent();
        });

        app.MapDelete("/settings/{id}", async (ISettingsRepository repository, int id) =>
        {
            var setting = await repository.GetSettingByIdAsync(id);
            if (setting == null)
            {
                return Results.NotFound();
            }

            await repository.DeleteSettingAsync(id);
            return Results.NoContent();
        });

        app.Run();
    } 
}

//public partial class Program { }

