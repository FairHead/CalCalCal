namespace CalCalCal.App.Models;

public class Session
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public List<Exercise> Exercises { get; set; } = new();
    
    // Calculated values
    public int ExerciseCount => Exercises.Count;
    public double TotalCalories => Exercises.Sum(e => e.EstimatedCalories);
    public double TotalDurationMinutes => Exercises.Sum(e => e.EstimatedDurationMinutes);
    
    // Display helpers
    public string MetaInfo => $"{ExerciseCount} exercises • ~{TotalDurationMinutes:F0} min";
    public string TotalCaloriesDisplay => $"{TotalCalories:F0} kcal";
}
