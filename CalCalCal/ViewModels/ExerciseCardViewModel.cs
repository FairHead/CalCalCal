using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CalCalCal.Models;
using CalCalCal.Services;
using System.Collections.ObjectModel;

namespace CalCalCal.ViewModels;

public partial class ExerciseCardViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;
    private readonly CalorieCalculatorService _calorieCalculator;

    [ObservableProperty]
    private int sessionId;

    [ObservableProperty]
    private ObservableCollection<ExerciseCard> exerciseCards = new();

    [ObservableProperty]
    private ExerciseCard? selectedCard;

    [ObservableProperty]
    private ExerciseType exerciseType = ExerciseType.Running;

    [ObservableProperty]
    private IntensityLevel intensity = IntensityLevel.Moderate;

    [ObservableProperty]
    private int durationMinutes = 30;

    [ObservableProperty]
    private string notes = string.Empty;

    [ObservableProperty]
    private double estimatedCalories;

    [ObservableProperty]
    private double totalSessionCalories;

    [ObservableProperty]
    private int totalSessionDuration;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool isEditing;

    [ObservableProperty]
    private bool hasNoCards;

    private UserProfile? _userProfile;

    public ExerciseCardViewModel(DatabaseService databaseService, CalorieCalculatorService calorieCalculator)
    {
        _databaseService = databaseService;
        _calorieCalculator = calorieCalculator;
    }

    public List<ExerciseType> ExerciseTypes => Enum.GetValues<ExerciseType>().ToList();
    public List<IntensityLevel> IntensityLevels => Enum.GetValues<IntensityLevel>().ToList();

    partial void OnExerciseTypeChanged(ExerciseType value) => UpdateEstimatedCalories();
    partial void OnIntensityChanged(IntensityLevel value) => UpdateEstimatedCalories();
    partial void OnDurationMinutesChanged(int value) => UpdateEstimatedCalories();

    private void UpdateEstimatedCalories()
    {
        if (_userProfile != null)
        {
            EstimatedCalories = _calorieCalculator.CalculateCalories(
                ExerciseType, Intensity, DurationMinutes, _userProfile);
        }
    }

    private void UpdateSessionTotals()
    {
        TotalSessionCalories = ExerciseCards.Sum(c => c.CaloriesBurned);
        TotalSessionDuration = ExerciseCards.Sum(c => c.DurationMinutes);
    }

    [RelayCommand]
    private async Task LoadCardsAsync(int sessionId)
    {
        SessionId = sessionId;
        IsLoading = true;

        try
        {
            _userProfile = await _databaseService.GetUserProfileAsync();
            var cards = await _databaseService.GetExerciseCardsAsync(sessionId);

            ExerciseCards.Clear();
            foreach (var card in cards)
            {
                ExerciseCards.Add(card);
            }

            HasNoCards = ExerciseCards.Count == 0;
            UpdateSessionTotals();
            UpdateEstimatedCalories();
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task AddCardAsync()
    {
        if (_userProfile == null)
        {
            // Show message to set up profile first
            return;
        }

        var calories = _calorieCalculator.CalculateCalories(
            ExerciseType, Intensity, DurationMinutes, _userProfile);

        var newCard = new ExerciseCard
        {
            WorkoutSessionId = SessionId,
            ExerciseType = ExerciseType,
            Intensity = Intensity,
            DurationMinutes = DurationMinutes,
            CaloriesBurned = calories,
            Notes = Notes,
            Order = ExerciseCards.Count
        };

        await _databaseService.SaveExerciseCardAsync(newCard);
        await _databaseService.UpdateSessionTotalsAsync(SessionId);

        ExerciseCards.Add(newCard);
        HasNoCards = false;
        UpdateSessionTotals();

        // Reset form
        ResetForm();
    }

    [RelayCommand]
    private void StartEdit(ExerciseCard? card)
    {
        if (card == null) return;

        SelectedCard = card;
        ExerciseType = card.ExerciseType;
        Intensity = card.Intensity;
        DurationMinutes = card.DurationMinutes;
        Notes = card.Notes;
        IsEditing = true;
        UpdateEstimatedCalories();
    }

    [RelayCommand]
    private async Task SaveEditAsync()
    {
        if (SelectedCard == null || _userProfile == null) return;

        var calories = _calorieCalculator.CalculateCalories(
            ExerciseType, Intensity, DurationMinutes, _userProfile);

        SelectedCard.ExerciseType = ExerciseType;
        SelectedCard.Intensity = Intensity;
        SelectedCard.DurationMinutes = DurationMinutes;
        SelectedCard.CaloriesBurned = calories;
        SelectedCard.Notes = Notes;

        await _databaseService.SaveExerciseCardAsync(SelectedCard);
        await _databaseService.UpdateSessionTotalsAsync(SessionId);

        // Refresh the card in the list using RemoveAt/Insert to trigger UI update
        var index = ExerciseCards.IndexOf(SelectedCard);
        if (index >= 0)
        {
            ExerciseCards.RemoveAt(index);
            ExerciseCards.Insert(index, SelectedCard);
        }

        UpdateSessionTotals();
        CancelEdit();
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsEditing = false;
        SelectedCard = null;
        ResetForm();
    }

    [RelayCommand]
    private async Task DeleteCardAsync(ExerciseCard? card)
    {
        if (card == null) return;

        await _databaseService.DeleteExerciseCardAsync(card);
        await _databaseService.UpdateSessionTotalsAsync(SessionId);

        ExerciseCards.Remove(card);
        HasNoCards = ExerciseCards.Count == 0;
        UpdateSessionTotals();

        if (SelectedCard == card)
        {
            CancelEdit();
        }
    }

    private void ResetForm()
    {
        ExerciseType = ExerciseType.Running;
        Intensity = IntensityLevel.Moderate;
        DurationMinutes = 30;
        Notes = string.Empty;
        UpdateEstimatedCalories();
    }
}
