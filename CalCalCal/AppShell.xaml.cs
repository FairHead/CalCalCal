using CalCalCal.Views;

namespace CalCalCal;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for navigation
        Routing.RegisterRoute(nameof(ExerciseCardsPage), typeof(ExerciseCardsPage));
    }
}
