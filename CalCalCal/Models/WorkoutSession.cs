using SQLite;

namespace CalCalCal.Models;

/// <summary>
/// Represents a workout session containing multiple exercise cards.
/// </summary>
public class WorkoutSession
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// Name of the workout session.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the workout session started.
    /// </summary>
    public DateTime SessionDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Total duration of all exercises in minutes.
    /// </summary>
    public int TotalDurationMinutes { get; set; }

    /// <summary>
    /// Total calories burned across all exercises.
    /// </summary>
    public double TotalCaloriesBurned { get; set; }

    /// <summary>
    /// Optional notes about the session.
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Whether the session is completed.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Date when the session was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date when the session was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Exercise cards in this session (not stored in DB, loaded separately).
    /// </summary>
    [Ignore]
    public List<ExerciseCard> ExerciseCards { get; set; } = new();
}
