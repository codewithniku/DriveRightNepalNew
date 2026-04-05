using driverightnepal;
using driverightnepal.Core.Data;
using driverightnepal.Core.Services;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        var connectionString = "Host=10.0.2.2;Port=5432;Database=DriveRightDb;Username=postgres;Password=admin123";
        builder.Services.AddDbContextFactory<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        builder.Services.AddSingleton<driverightnepal.Core.Services.UserStateService>();

        var adminApiUrl = "http://10.0.2.2:5015";
        builder.Services.AddSingleton(new ConfigService { BaseUrl = adminApiUrl });
        builder.Services.AddScoped<MockGeneratorService>();
        builder.Services.AddScoped<IEmailService, EmailService>();

#if ANDROID
        Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping("BlazorCustomization", (handler, view) =>
        {
            if (handler.PlatformView is Android.Webkit.WebView webView)
            {
                webView.Settings.AllowFileAccess = true;
                webView.Settings.AllowContentAccess = true;
                webView.Settings.JavaScriptEnabled = true;
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