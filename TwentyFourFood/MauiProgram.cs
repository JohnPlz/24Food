using Microsoft.Extensions.Logging;
using TwentyFourFood.Services;
using TwentyFourFood.ViewModels;

namespace TwentyFourFood
{
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

            builder.Services.AddSingleton<LiteDbService>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<CreateIngredientPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
