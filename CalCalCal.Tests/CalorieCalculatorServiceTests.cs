using CalCalCal.Models;
using CalCalCal.Services;
using Xunit;

namespace CalCalCal.Tests;

public class CalorieCalculatorServiceTests
{
    private readonly CalorieCalculatorService _service;

    public CalorieCalculatorServiceTests()
    {
        _service = new CalorieCalculatorService();
    }

    [Fact]
    public void CalculateCalories_WithNullProfile_ReturnsZero()
    {
        // Arrange & Act
        var result = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Moderate, 30, null!);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateCalories_WithZeroWeight_ReturnsZero()
    {
        // Arrange
        var profile = new UserProfile { Weight = 0, Height = 170, Age = 30, Gender = Gender.Male };

        // Act
        var result = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Moderate, 30, profile);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateCalories_WithZeroDuration_ReturnsZero()
    {
        // Arrange
        var profile = new UserProfile { Weight = 70, Height = 170, Age = 30, Gender = Gender.Male };

        // Act
        var result = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Moderate, 0, profile);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateCalories_Running_Moderate_ReturnsExpectedCalories()
    {
        // Arrange
        var profile = new UserProfile { Weight = 70, Height = 170, Age = 30, Gender = Gender.Male };
        // MET for running moderate = 8.3
        // Expected: 8.3 * 70 * 0.5 (30 min = 0.5 hours) * 1.0 (age factor) * 1.05 (male factor) = 305.025

        // Act
        var result = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Moderate, 30, profile);

        // Assert
        Assert.True(result > 250 && result < 350, $"Expected between 250-350, got {result}");
    }

    [Fact]
    public void CalculateCalories_Walking_Light_ReturnsLowerThanRunning()
    {
        // Arrange
        var profile = new UserProfile { Weight = 70, Height = 170, Age = 30, Gender = Gender.Male };

        // Act
        var walkingCalories = _service.CalculateCalories(ExerciseType.Walking, IntensityLevel.Light, 30, profile);
        var runningCalories = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Light, 30, profile);

        // Assert
        Assert.True(walkingCalories < runningCalories, "Walking should burn fewer calories than running");
    }

    [Fact]
    public void CalculateCalories_HigherIntensity_ReturnMoreCalories()
    {
        // Arrange
        var profile = new UserProfile { Weight = 70, Height = 170, Age = 30, Gender = Gender.Male };

        // Act
        var lightCalories = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Light, 30, profile);
        var moderateCalories = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Moderate, 30, profile);
        var vigorousCalories = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Vigorous, 30, profile);

        // Assert
        Assert.True(lightCalories < moderateCalories, "Light intensity should burn fewer calories than moderate");
        Assert.True(moderateCalories < vigorousCalories, "Moderate intensity should burn fewer calories than vigorous");
    }

    [Fact]
    public void CalculateCalories_LongerDuration_ReturnsMoreCalories()
    {
        // Arrange
        var profile = new UserProfile { Weight = 70, Height = 170, Age = 30, Gender = Gender.Male };

        // Act
        var shortDuration = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Moderate, 15, profile);
        var longDuration = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Moderate, 60, profile);

        // Assert
        Assert.True(longDuration > shortDuration * 3, "60 min should burn more than 3x of 15 min");
    }

    [Fact]
    public void CalculateCalories_HeavierWeight_ReturnsMoreCalories()
    {
        // Arrange
        var lightProfile = new UserProfile { Weight = 60, Height = 170, Age = 30, Gender = Gender.Male };
        var heavyProfile = new UserProfile { Weight = 90, Height = 170, Age = 30, Gender = Gender.Male };

        // Act
        var lightCalories = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Moderate, 30, lightProfile);
        var heavyCalories = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Moderate, 30, heavyProfile);

        // Assert
        Assert.True(heavyCalories > lightCalories, "Heavier person should burn more calories");
    }

    [Fact]
    public void CalculateCalories_Male_ReturnsSlightlyMoreThanFemale()
    {
        // Arrange
        var maleProfile = new UserProfile { Weight = 70, Height = 170, Age = 30, Gender = Gender.Male };
        var femaleProfile = new UserProfile { Weight = 70, Height = 170, Age = 30, Gender = Gender.Female };

        // Act
        var maleCalories = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Moderate, 30, maleProfile);
        var femaleCalories = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Moderate, 30, femaleProfile);

        // Assert
        Assert.True(maleCalories > femaleCalories, "Male should burn slightly more calories due to higher metabolic rate");
    }

    [Fact]
    public void CalculateCalories_OlderAge_ReturnsLessCalories()
    {
        // Arrange
        var youngProfile = new UserProfile { Weight = 70, Height = 170, Age = 25, Gender = Gender.Male };
        var oldProfile = new UserProfile { Weight = 70, Height = 170, Age = 65, Gender = Gender.Male };

        // Act
        var youngCalories = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Moderate, 30, youngProfile);
        var oldCalories = _service.CalculateCalories(ExerciseType.Running, IntensityLevel.Moderate, 30, oldProfile);

        // Assert
        Assert.True(youngCalories > oldCalories, "Younger person should burn more calories");
    }

    [Theory]
    [InlineData(ExerciseType.Running)]
    [InlineData(ExerciseType.Walking)]
    [InlineData(ExerciseType.Cycling)]
    [InlineData(ExerciseType.Swimming)]
    [InlineData(ExerciseType.WeightTraining)]
    [InlineData(ExerciseType.Yoga)]
    [InlineData(ExerciseType.HIIT)]
    [InlineData(ExerciseType.Rowing)]
    [InlineData(ExerciseType.JumpRope)]
    [InlineData(ExerciseType.Stretching)]
    [InlineData(ExerciseType.Other)]
    public void CalculateCalories_AllExerciseTypes_ReturnsPositiveCalories(ExerciseType exerciseType)
    {
        // Arrange
        var profile = new UserProfile { Weight = 70, Height = 170, Age = 30, Gender = Gender.Male };

        // Act
        var result = _service.CalculateCalories(exerciseType, IntensityLevel.Moderate, 30, profile);

        // Assert
        Assert.True(result > 0, $"Exercise type {exerciseType} should return positive calories");
    }
}
