using Microsoft.Extensions.Configuration;

namespace Code_explainer;

/// <summary>
/// MauiProgram + appsettings.json access
/// </summary>

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        var config = new ConfigurationBuilder()
            .AddJsonFile("Properties/appsettings.json", optional: true)
            .AddJsonFile("Properties/appsettings.local.json", optional: true)
            .Build();

        builder.Configuration.AddConfiguration(config);
        builder.Services.AddSingleton(config);
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
    }
}