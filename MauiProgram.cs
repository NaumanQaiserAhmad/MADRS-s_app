using MADRSApp.Services;
using MADRSApp.ViewModels;

namespace MADRSApp;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddHttpClient<QuestionService>(client =>
        {
            client.BaseAddress = new Uri("https://flowns-app-test.herokuapp.com/api/");
        });
        builder.Services.AddSingleton<QuestionService>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
    }
}
