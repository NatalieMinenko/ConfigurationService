using Microsoft.EntityFrameworkCore;
using ConfigurationService.Persistence.Repository;
using ConfigurationService.Persistence.Interfaces;
using ConfigurationService.Persistence;
using ConfigurationService.Persistence.DTO;


public class Program
{
    private static void Main(string[] args)

    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<SettingsContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("ConfigurationServiceDb")));

        builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.MapGet("/settings/{service}", async (ISettingsRepository repository, string service) =>
        {
            var settings = await repository.GetSettingsByServiceAsync(service);
            return settings.Any() ? Results.Ok(settings) : Results.NotFound();
        });

        app.MapPost("/settings", async (ISettingsRepository repository, Settings setting) =>
        {
            await repository.AddSettingAsync(setting);
            return Results.Created($"/settings/{setting.Service}", setting);
        });

        app.MapPut("/settings/{id}", async (ISettingsRepository repository, int id, Settings newSetting) =>
        {
            var setting = await repository.GetSettingByIdAsync(id);
            if (setting == null)
            {
                return Results.NotFound();
            }

            await repository.UpdateSettingAsync(setting, newSetting);
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
