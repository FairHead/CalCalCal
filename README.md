# CalCalCal
A cross-platform mobile app built with .NET MAUI that helps you track your workout sessions efficiently. Create customizable session cards for each exercise, input duration and intensity, and get an automatic calorie estimation based on your personal data.

## Features

- **User Profile**: Set up your personal data (weight, height, age, gender) for accurate calorie calculations
- **Workout Sessions**: Create and manage multiple workout sessions
- **Exercise Cards**: Add individual exercise cards to each session with:
  - Exercise type (Running, Walking, Cycling, Swimming, Weight Training, Yoga, HIIT, Rowing, Jump Rope, Stretching, and more)
  - Duration (in minutes)
  - Intensity level (Light, Moderate, Vigorous, Very Vigorous)
  - Automatic calorie estimation based on MET values
  - Optional notes
- **Grid-based UI**: Clean, intuitive interface with grid layouts for exercise cards
- **Local Storage**: All data is stored locally using SQLite

## Calorie Calculation

The app uses the MET (Metabolic Equivalent of Task) method to estimate calories burned:

```
Calories = MET × Weight (kg) × Duration (hours) × Age Factor × Gender Factor
```

MET values are based on the Compendium of Physical Activities and vary by exercise type and intensity level.

## Project Structure

```
CalCalCal/
├── Models/
│   ├── ExerciseCard.cs       # Individual exercise within a session
│   ├── ExerciseType.cs       # Types of exercises (Running, Walking, etc.)
│   ├── Gender.cs             # Gender enumeration
│   ├── IntensityLevel.cs     # Exercise intensity levels
│   ├── UserProfile.cs        # User's personal data
│   └── WorkoutSession.cs     # Container for exercise cards
├── ViewModels/
│   ├── ExerciseCardViewModel.cs
│   ├── UserProfileViewModel.cs
│   └── WorkoutSessionViewModel.cs
├── Views/
│   ├── ExerciseCardsPage.xaml(.cs)
│   ├── UserProfilePage.xaml(.cs)
│   └── WorkoutSessionsPage.xaml(.cs)
├── Services/
│   ├── CalorieCalculatorService.cs  # MET-based calorie calculation
│   └── DatabaseService.cs           # SQLite database operations
├── Converters/
│   └── Converters.cs         # XAML value converters
├── Platforms/                # Platform-specific code
│   ├── Android/
│   ├── iOS/
│   ├── MacCatalyst/
│   └── Windows/
└── Resources/
    └── Styles/
        ├── Colors.xaml
        └── Styles.xaml
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 with MAUI workload (Windows/macOS)
- Or Visual Studio Code with C# extensions

### Building the App

```bash
# Clone the repository
git clone https://github.com/FairHead/CalCalCal.git
cd CalCalCal

# Restore dependencies
dotnet restore

# Build for your target platform
dotnet build -f net8.0-android
dotnet build -f net8.0-ios
dotnet build -f net8.0-maccatalyst
dotnet build -f net8.0-windows10.0.19041.0
```

### Running Tests

```bash
cd CalCalCal.Tests
dotnet test
```

## Technologies Used

- **.NET MAUI** - Cross-platform UI framework
- **CommunityToolkit.Mvvm** - MVVM framework with source generators
- **SQLite** - Local database storage
- **xUnit** - Unit testing framework

## License

This project is open source and available under the MIT License.
