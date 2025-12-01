using CalCalCal.ViewModels;

namespace CalCalCal.Views;

[QueryProperty(nameof(SessionId), "sessionId")]
public partial class ExerciseCardsPage : ContentPage
{
    private readonly ExerciseCardViewModel _viewModel;

    public int SessionId { get; set; }

    public ExerciseCardsPage(ExerciseCardViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadCardsCommand.ExecuteAsync(SessionId);
    }
}
