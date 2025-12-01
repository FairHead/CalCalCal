using SQLite;

namespace CalCalCal.Models;

/// <summary>
/// Represents an individual exercise card with details about the exercise
/// and calculated calories burned.
/// </summary>
public class ExerciseCard
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to the workout session.
    /// </summary>
    [Indexed]
    public int WorkoutSessionId { get; set; }

    /// <summary>
    /// Type of exercise performed.
    /// </summary>
    public ExerciseType ExerciseType { get; set; }

    /// <summary>
    /// Duration of the exercise in minutes.
    /// </summary>
    public int DurationMinutes { get; set; }

    /// <summary>
    /// Intensity level of the exercise.
    /// </summary>
    public IntensityLevel Intensity { get; set; }

    /// <summary>
    /// Estimated calories burned during this exercise.
    /// </summary>
    public double CaloriesBurned { get; set; }

    /// <summary>
    /// Optional notes about the exercise.
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Order of the exercise within the session.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Date when the exercise card was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
