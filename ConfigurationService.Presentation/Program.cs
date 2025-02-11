using ConfigurationService.Presentation.Models;
using ConfigurationService.Persistence;
using ConfigurationService.Persistence.DTO;
using Microsoft.EntityFrameworkCore;

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
        var connectionString = builder.Configuration.GetConnectionString("DBConnectionString");
        builder.Services.AddDbContext<SettingsContext>(options => options.UseNpgsql(connectionString));

        var app = builder.Build();

        app.MapGet("/settings/{id}", async (int id, SettingsContext context) =>
        {
            var setting = await context.Settings.FindAsync(id);
            return setting != null ? Results.Ok(setting) : Results.NotFound();
        });

        app.MapPost("/settings", async (SettingRequest setting, SettingsContext context) =>
        {
            if (setting.Name is null || setting.Value is null || setting.Service is null)
                return Results.BadRequest();
            var createdSetting = context.Settings.Add(new Setting
            {
                Name = setting.Name, 
                Value = setting.Value,
                Service = setting.Service, 
            });
            await context.SaveChangesAsync();
            return Results.Created($"/settings/{createdSetting.Entity.Id}", createdSetting.Entity);
        });

        app.MapDelete("/settings/{id}", async (int id, SettingsContext context) =>
        {
            var article = await context.Settings.FindAsync(id);
            if (article is null) return Results.NotFound();
           
            context.Settings.Remove(article);
            await context.SaveChangesAsync();
            return Results.NoContent();
        });

        app.MapPut("/settings/{id}", async (int id, SettingRequest setting, SettingsContext context) =>
        {
            var settingsToUpdate = await context.Settings.FindAsync(id);

            if(settingsToUpdate == null) return Results.NotFound();
            if(setting.Name != null) settingsToUpdate.Name = setting.Name;
            if(setting.Value != null) settingsToUpdate.Value = setting.Value;
            if(setting.Service != null) settingsToUpdate.Service = setting.Service;

            await context.SaveChangesAsync();
            return Results.Ok(settingsToUpdate);
        });

        app.Run();
    }
}
