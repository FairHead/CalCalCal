using CalCalCal.ViewModels;

namespace CalCalCal.Views;

public partial class WorkoutSessionsPage : ContentPage
{
    private readonly WorkoutSessionViewModel _viewModel;

    public WorkoutSessionsPage(WorkoutSessionViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadSessionsCommand.ExecuteAsync(null);
    }
}
