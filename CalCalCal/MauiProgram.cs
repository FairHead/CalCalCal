using Microsoft.Extensions.Logging;
using CalCalCal.Services;
using CalCalCal.ViewModels;
using CalCalCal.Views;

namespace CalCalCal;

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

        // Register Services
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<CalorieCalculatorService>();

        // Register ViewModels
        builder.Services.AddTransient<UserProfileViewModel>();
        builder.Services.AddTransient<WorkoutSessionViewModel>();
        builder.Services.AddTransient<ExerciseCardViewModel>();

        // Register Views
        builder.Services.AddTransient<UserProfilePage>();
        builder.Services.AddTransient<WorkoutSessionsPage>();
        builder.Services.AddTransient<ExerciseCardsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
