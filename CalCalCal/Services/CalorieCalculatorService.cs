using CalCalCal.Models;

namespace CalCalCal.Services;

/// <summary>
/// Service for calculating calories burned based on exercise parameters and user profile.
/// Uses MET (Metabolic Equivalent of Task) values for estimation.
/// </summary>
public class CalorieCalculatorService
{
    /// <summary>
    /// Gets the MET value for a specific exercise type and intensity level.
    /// MET values are based on the Compendium of Physical Activities.
    /// </summary>
    private static double GetMetValue(ExerciseType exerciseType, IntensityLevel intensity)
    {
        return (exerciseType, intensity) switch
        {
            // Running
            (ExerciseType.Running, IntensityLevel.Light) => 6.0,        // Light jogging
            (ExerciseType.Running, IntensityLevel.Moderate) => 8.3,     // 5 mph
            (ExerciseType.Running, IntensityLevel.Vigorous) => 11.0,    // 7 mph
            (ExerciseType.Running, IntensityLevel.VeryVigorous) => 14.5, // 9 mph

            // Walking
            (ExerciseType.Walking, IntensityLevel.Light) => 2.5,        // Slow walk
            (ExerciseType.Walking, IntensityLevel.Moderate) => 3.5,     // Brisk walk
            (ExerciseType.Walking, IntensityLevel.Vigorous) => 5.0,     // Very brisk
            (ExerciseType.Walking, IntensityLevel.VeryVigorous) => 6.3, // Race walking

            // Cycling
            (ExerciseType.Cycling, IntensityLevel.Light) => 4.0,        // Leisure cycling
            (ExerciseType.Cycling, IntensityLevel.Moderate) => 6.8,     // Moderate effort
            (ExerciseType.Cycling, IntensityLevel.Vigorous) => 10.0,    // Fast cycling
            (ExerciseType.Cycling, IntensityLevel.VeryVigorous) => 16.0, // Racing

            // Swimming
            (ExerciseType.Swimming, IntensityLevel.Light) => 4.5,       // Leisurely
            (ExerciseType.Swimming, IntensityLevel.Moderate) => 5.8,    // Moderate laps
            (ExerciseType.Swimming, IntensityLevel.Vigorous) => 8.3,    // Fast laps
            (ExerciseType.Swimming, IntensityLevel.VeryVigorous) => 11.0, // Competitive

            // Weight Training
            (ExerciseType.WeightTraining, IntensityLevel.Light) => 3.0,
            (ExerciseType.WeightTraining, IntensityLevel.Moderate) => 5.0,
            (ExerciseType.WeightTraining, IntensityLevel.Vigorous) => 6.0,
            (ExerciseType.WeightTraining, IntensityLevel.VeryVigorous) => 8.0,

            // Yoga
            (ExerciseType.Yoga, IntensityLevel.Light) => 2.0,           // Gentle yoga
            (ExerciseType.Yoga, IntensityLevel.Moderate) => 3.0,        // Hatha yoga
            (ExerciseType.Yoga, IntensityLevel.Vigorous) => 4.0,        // Power yoga
            (ExerciseType.Yoga, IntensityLevel.VeryVigorous) => 5.0,    // Hot yoga

            // HIIT
            (ExerciseType.HIIT, IntensityLevel.Light) => 5.0,
            (ExerciseType.HIIT, IntensityLevel.Moderate) => 8.0,
            (ExerciseType.HIIT, IntensityLevel.Vigorous) => 12.0,
            (ExerciseType.HIIT, IntensityLevel.VeryVigorous) => 15.0,

            // Rowing
            (ExerciseType.Rowing, IntensityLevel.Light) => 4.8,
            (ExerciseType.Rowing, IntensityLevel.Moderate) => 7.0,
            (ExerciseType.Rowing, IntensityLevel.Vigorous) => 8.5,
            (ExerciseType.Rowing, IntensityLevel.VeryVigorous) => 12.0,

            // Jump Rope
            (ExerciseType.JumpRope, IntensityLevel.Light) => 8.8,
            (ExerciseType.JumpRope, IntensityLevel.Moderate) => 11.0,
            (ExerciseType.JumpRope, IntensityLevel.Vigorous) => 12.0,
            (ExerciseType.JumpRope, IntensityLevel.VeryVigorous) => 14.0,

            // Stretching
            (ExerciseType.Stretching, IntensityLevel.Light) => 2.0,
            (ExerciseType.Stretching, IntensityLevel.Moderate) => 2.5,
            (ExerciseType.Stretching, IntensityLevel.Vigorous) => 3.0,
            (ExerciseType.Stretching, IntensityLevel.VeryVigorous) => 4.0,

            // Other - default values
            (ExerciseType.Other, IntensityLevel.Light) => 3.0,
            (ExerciseType.Other, IntensityLevel.Moderate) => 5.0,
            (ExerciseType.Other, IntensityLevel.Vigorous) => 7.0,
            (ExerciseType.Other, IntensityLevel.VeryVigorous) => 9.0,

            _ => 5.0 // Default MET value
        };
    }

    /// <summary>
    /// Calculates calories burned using the MET formula.
    /// Formula: Calories = MET × Weight (kg) × Duration (hours)
    /// </summary>
    /// <param name="exerciseType">Type of exercise</param>
    /// <param name="intensity">Intensity level</param>
    /// <param name="durationMinutes">Duration in minutes</param>
    /// <param name="userProfile">User profile with weight information</param>
    /// <returns>Estimated calories burned</returns>
    public double CalculateCalories(
        ExerciseType exerciseType,
        IntensityLevel intensity,
        int durationMinutes,
        UserProfile userProfile)
    {
        if (userProfile == null || userProfile.Weight <= 0 || durationMinutes <= 0)
        {
            return 0;
        }

        double met = GetMetValue(exerciseType, intensity);
        double durationHours = durationMinutes / 60.0;

        // Basic MET formula: Calories = MET × Weight (kg) × Duration (hours)
        double calories = met * userProfile.Weight * durationHours;

        // Apply a slight adjustment based on age and gender
        // Younger people and males typically have higher metabolic rates
        double ageFactor = GetAgeFactor(userProfile.Age);
        double genderFactor = GetGenderFactor(userProfile.Gender);

        return Math.Round(calories * ageFactor * genderFactor, 1);
    }

    private static double GetAgeFactor(int age)
    {
        return age switch
        {
            < 25 => 1.05,
            < 35 => 1.0,
            < 45 => 0.98,
            < 55 => 0.95,
            < 65 => 0.92,
            _ => 0.88
        };
    }

    private static double GetGenderFactor(Gender gender)
    {
        return gender switch
        {
            Gender.Male => 1.05,
            Gender.Female => 0.95,
            Gender.Other => 1.0,
            _ => 1.0
        };
    }
}
