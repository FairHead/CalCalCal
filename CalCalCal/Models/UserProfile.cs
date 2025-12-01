using SQLite;

namespace CalCalCal.Models;

/// <summary>
/// Represents a user profile with personal data used for calorie calculations.
/// </summary>
public class UserProfile
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// User's weight in kilograms.
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// User's height in centimeters.
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// User's age in years.
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// User's gender.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// User's name (optional).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Date when the profile was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date when the profile was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
