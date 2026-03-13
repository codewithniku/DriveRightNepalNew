using driverightnepal;
using driverightnepal.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public static class MauiProgram
{

    public static MauiApp CreateMauiApp()
    {
#if ANDROID
        // This tells the Android OS to ignore SSL errors for local development
        // (Don't use this in a real production app!)
        var handler = new Xamarin.Android.Net.AndroidMessageHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            if (cert != null && cert.Issuer.Contains("localhost"))
                return true;
            return errors == System.Net.Security.SslPolicyErrors.None;
        };
#endif
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        // 1. Updated Database Name (matching your successful pgAdmin connection)
        var connectionString = "Host=10.0.2.2;Port=5432;Database=DriveRightDb;Username=postgres;Password=admin123";

        builder.Services.AddDbContextFactory<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        builder.Services.AddSingleton<driverightnepal.Core.Services.UserStateService>();

        // 2. Register a "Media Base URL" so the app knows where to find the PDFs
        // On Android Emulator, 10.0.2.2 points to your PC. 
        // 5000 is the typical default port for the Admin Web App.
        var adminApiUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
        builder.Services.AddSingleton(new ConfigService { BaseUrl = adminApiUrl });

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

// Simple helper class to keep track of your server address
public class ConfigService { public string BaseUrl { get; set; } }