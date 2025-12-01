using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CalCalCal.Models;
using CalCalCal.Services;

namespace CalCalCal.ViewModels;

public partial class UserProfileViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private double weight;

    [ObservableProperty]
    private double height;

    [ObservableProperty]
    private int age;

    [ObservableProperty]
    private Gender gender = Gender.Other;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string statusMessage = string.Empty;

    private int _profileId;

    public static List<Gender> GenderOptions => Enum.GetValues<Gender>().ToList();

    public UserProfileViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [RelayCommand]
    private async Task LoadProfileAsync()
    {
        IsLoading = true;
        try
        {
            var profile = await _databaseService.GetUserProfileAsync();
            if (profile != null)
            {
                _profileId = profile.Id;
                Name = profile.Name;
                Weight = profile.Weight;
                Height = profile.Height;
                Age = profile.Age;
                Gender = profile.Gender;
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task SaveProfileAsync()
    {
        if (Weight <= 0 || Height <= 0 || Age <= 0)
        {
            StatusMessage = "Please enter valid values for weight, height, and age.";
            return;
        }

        IsLoading = true;
        try
        {
            var profile = new UserProfile
            {
                Id = _profileId,
                Name = Name,
                Weight = Weight,
                Height = Height,
                Age = Age,
                Gender = Gender
            };

            await _databaseService.SaveUserProfileAsync(profile);
            _profileId = profile.Id;
            StatusMessage = "Profile saved successfully!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving profile: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
