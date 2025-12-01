using CalCalCal.ViewModels;

namespace CalCalCal.Views;

public partial class UserProfilePage : ContentPage
{
    public UserProfilePage(UserProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UserProfileViewModel vm)
        {
            await vm.LoadProfileCommand.ExecuteAsync(null);
        }
    }
}
