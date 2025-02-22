using Microsoft.EntityFrameworkCore;
using ConfigurationService.Persistence.Repository;
using ConfigurationService.Persistence.Interfaces;
using ConfigurationService.Persistence;
using ConfigurationService.Persistence.DTO;
using ConfigurationService.Presentation.Models;
using ConfigurationService.Presentation.Models.Requests;
using Microsoft.AspNetCore.Mvc;

public class Program
{
    private static void Main(string[] args)

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

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.MapGet("/settings/", async ([FromQuery] ServiceName service, ISettingsRepository repository) =>
        {
            var settings = await repository.GetSettingsByServiceAsync(service);
            if (settings == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(settings);
        });

        app.MapPost("/settings/", async (ISettingsRepository repository, SettingCreateRequest request) =>
        {
            var existingSetting = await repository.GetSettingByNameAndServiceNameAsync(request.Name, request.Service);
            if (existingSetting != null)
            {
                return Results.Conflict("Setting with the name already exists for the service");
            }

            var setting = new Settings()
            {
                Name = request.Name,
                Value = request.Value,
                Service = (ServiceName)request.Service
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
