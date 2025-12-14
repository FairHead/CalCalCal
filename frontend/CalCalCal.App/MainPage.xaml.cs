using CalCalCal.App.Models;

namespace CalCalCal.App;

public partial class MainPage : ContentPage
{
	private Session _currentSession;

	public MainPage()
	{
		InitializeComponent();
		LoadSampleData();
	}

	private void LoadSampleData()
	{
		// Create "Back Day" session with 4 exercises
		_currentSession = new Session
		{
			Name = "Back Day",
			Exercises = new List<Exercise>
			{
				new Exercise
				{
					Name = "Pull-ups",
					Reps = new List<int> { 8, 10, 12 },
					Intensity = 8, // 75%
					RestSeconds = 90,
					EstimatedCalories = 85,
					EstimatedDurationMinutes = 8,
					ImageUrl = "🏋️"
				},
				new Exercise
				{
					Name = "Kettlebell Swings",
					Reps = new List<int> { 12, 15, 15 },
					Intensity = 6, // 60%
					RestSeconds = 60,
					EstimatedCalories = 95,
					EstimatedDurationMinutes = 10,
					ImageUrl = "🏋️"
				},
				new Exercise
				{
					Name = "Push-ups",
					Reps = new List<int> { 10, 12, 12 },
					Intensity = 7, // 65%
					RestSeconds = 60,
					EstimatedCalories = 70,
					EstimatedDurationMinutes = 9,
					ImageUrl = "💪"
				},
				new Exercise
				{
					Name = "Squats",
					Reps = new List<int> { 12, 15, 15 },
					Intensity = 7, // 70%
					RestSeconds = 90,
					EstimatedCalories = 90,
					EstimatedDurationMinutes = 11,
					ImageUrl = "🏃"
				}
			}
		};

		UpdateUI();
	}

	private void UpdateUI()
	{
		// Update session header
		SessionTitle.Text = _currentSession.Name;
		SessionMeta.Text = _currentSession.MetaInfo;
		TotalCalories.Text = _currentSession.TotalCaloriesDisplay;

		// Clear and populate exercise grid
		ExerciseGrid.Clear();
		
		for (int i = 0; i < _currentSession.Exercises.Count; i++)
		{
			var exercise = _currentSession.Exercises[i];
			var exerciseCard = CreateExerciseCard(exercise);
			
			int row = i / 2;
			int col = i % 2;
			
			ExerciseGrid.Add(exerciseCard, col, row);
		}
	}

	private Frame CreateExerciseCard(Exercise exercise)
	{
		var card = new Frame
		{
			Padding = 12,
			CornerRadius = 16,
			BackgroundColor = Color.FromArgb("#2A2A3A"),
			HasShadow = false
		};

		var layout = new VerticalStackLayout
		{
			Spacing = 8
		};

		// Exercise image placeholder
		var imageFrame = new Frame
		{
			CornerRadius = 12,
			Padding = 0,
			HeightRequest = 80,
			BackgroundColor = Color.FromArgb("#3A3A4A"),
			HasShadow = false
		};

		var imageLabel = new Label
		{
			Text = exercise.ImageUrl ?? "🏋️",
			FontSize = 40,
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

		imageFrame.Content = imageLabel;
		layout.Add(imageFrame);

		// Exercise name
		var nameLabel = new Label
		{
			Text = exercise.Name.Length > 15 ? exercise.Name.Substring(0, 12) + "..." : exercise.Name,
			FontSize = 14,
			FontAttributes = FontAttributes.Bold,
			TextColor = Color.FromArgb("#F5F5F5")
		};
		layout.Add(nameLabel);

		// Sets info
		var setsLabel = new Label
		{
			Text = $"Sets:\n{exercise.RepsDisplay}",
			FontSize = 11,
			TextColor = Color.FromArgb("#8A8A9A")
		};
		layout.Add(setsLabel);

		// Intensity bar
		var intensityStack = new VerticalStackLayout { Spacing = 4 };
		
		var intensityLabel = new Label
		{
			Text = $"Intensity {exercise.IntensityPercent}%",
			FontSize = 11,
			TextColor = Color.FromArgb("#00D9FF")
		};
		intensityStack.Add(intensityLabel);

		var intensityBar = new ProgressBar
		{
			Progress = exercise.IntensityPercent / 100.0,
			ProgressColor = Color.FromArgb("#00D9FF"),
			HeightRequest = 4
		};
		intensityStack.Add(intensityBar);
		layout.Add(intensityStack);

		// Calories
		var caloriesLabel = new Label
		{
			Text = exercise.CaloriesDisplay,
			FontSize = 14,
			FontAttributes = FontAttributes.Bold,
			TextColor = Color.FromArgb("#FF6B35")
		};
		layout.Add(caloriesLabel);

		card.Content = layout;
		return card;
	}
}
