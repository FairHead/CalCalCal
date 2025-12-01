using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CalCalCal.Models;
using CalCalCal.Services;
using System.Collections.ObjectModel;

namespace CalCalCal.ViewModels;

public partial class WorkoutSessionViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;
    private readonly CalorieCalculatorService _calorieCalculator;

    [ObservableProperty]
    private ObservableCollection<WorkoutSession> sessions = new();

    [ObservableProperty]
    private WorkoutSession? selectedSession;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool hasNoSessions;

    public WorkoutSessionViewModel(DatabaseService databaseService, CalorieCalculatorService calorieCalculator)
    {
        _databaseService = databaseService;
        _calorieCalculator = calorieCalculator;
    }

    [RelayCommand]
    private async Task LoadSessionsAsync()
    {
        IsLoading = true;
        try
        {
            var sessionList = await _databaseService.GetWorkoutSessionsAsync();
            Sessions.Clear();
            foreach (var session in sessionList)
            {
                Sessions.Add(session);
            }
            HasNoSessions = Sessions.Count == 0;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task CreateSessionAsync()
    {
        var newSession = new WorkoutSession
        {
            Name = $"Workout {DateTime.Now:MMM dd, yyyy}",
            SessionDate = DateTime.Now
        };

        await _databaseService.SaveWorkoutSessionAsync(newSession);
        Sessions.Insert(0, newSession);
        HasNoSessions = false;
        SelectedSession = newSession;
    }

    [RelayCommand]
    private async Task DeleteSessionAsync(WorkoutSession? session)
    {
        if (session == null) return;

        await _databaseService.DeleteWorkoutSessionAsync(session);
        Sessions.Remove(session);
        HasNoSessions = Sessions.Count == 0;

        if (SelectedSession == session)
        {
            SelectedSession = null;
        }
    }

    [RelayCommand]
    private async Task CompleteSessionAsync(WorkoutSession? session)
    {
        if (session == null) return;

        session.IsCompleted = true;
        await _databaseService.SaveWorkoutSessionAsync(session);
    }

    [RelayCommand]
    private async Task NavigateToSessionAsync(WorkoutSession? session)
    {
        if (session == null) return;

        await Shell.Current.GoToAsync($"ExerciseCardsPage?sessionId={session.Id}");
    }
}
