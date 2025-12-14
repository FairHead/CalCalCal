namespace CalCalCal.App.Models;

public class Exercise
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    
    // Sets configuration
    public List<int> Reps { get; set; } = new();
    public int Sets => Reps.Count;
    
    // Timing
    public int RestSeconds { get; set; } = 60;
    public double SecondsPerRep { get; set; } = 2.0;
    
    // Intensity (1-10 scale, shown as percentage)
    public int Intensity { get; set; } = 5;
    public int IntensityPercent => Intensity * 10;
    
    // Calculated values
    public double EstimatedCalories { get; set; }
    public double EstimatedDurationMinutes { get; set; }
    
    // Display helpers
    public string RepsDisplay => string.Join(" / ", Reps);
    public string CaloriesDisplay => $"~{EstimatedCalories:F0} kcal";
}
