using Microsoft.EntityFrameworkCore;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //builder.Services.AddDbContext<SettingsContext>(opt => opt.UseInMemoryDatabase("SettingsList"));
        //builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        //builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();

        var app = builder.Build();

        app.Run();
    }
}
