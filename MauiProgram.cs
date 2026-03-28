using driverightnepal;
using driverightnepal.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using driverightnepal.Core.Services;
using Microsoft.AspNetCore.Components.WebView.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        // 1. Database Connection 
        // Reverted to 10.0.2.2 for Android Emulator access to local PostgreSQL
        var connectionString = "Host=10.0.2.2;Port=5432;Database=DriveRightDb;Username=postgres;Password=admin123";
        builder.Services.AddDbContextFactory<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        builder.Services.AddSingleton<driverightnepal.Core.Services.UserStateService>();

        // 2. Config Service - Points to your Admin Server
        // Reverted to 10.0.2.2 for Emulator to hit your PC's Port 5015
        var adminApiUrl = "http://10.0.2.2:5015";
        builder.Services.AddSingleton(new ConfigService { BaseUrl = adminApiUrl });
        builder.Services.AddScoped<MockGeneratorService>();
        // 3. PLATFORM SPECIFIC: Keep these settings as they ensure the WebView works correctly
#if ANDROID
        Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping("BlazorCustomization", (handler, view) =>
        {
            if (handler.PlatformView is Android.Webkit.WebView webView)
            {
                webView.Settings.JavaScriptEnabled = true;
                // Keep AlwaysAllow to prevent "Mixed Content" blocks during development
                webView.Settings.MixedContentMode = Android.Webkit.MixedContentHandling.AlwaysAllow;
                webView.Settings.MediaPlaybackRequiresUserGesture = false;

                webView.SetLayerType(Android.Views.LayerType.Hardware, null);
            }
        });
#endif

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

public class ConfigService { public string BaseUrl { get; set; } = ""; }