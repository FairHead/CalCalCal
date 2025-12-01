using SQLite;
using CalCalCal.Models;

namespace CalCalCal.Services;

/// <summary>
/// Service for local database operations using SQLite.
/// </summary>
public class DatabaseService
{
    private SQLiteAsyncConnection? _database;
    private readonly string _dbPath;

    public DatabaseService()
    {
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "calcalcal.db3");
    }

    private async Task InitAsync()
    {
        if (_database != null)
            return;

        _database = new SQLiteAsyncConnection(_dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

        await _database.CreateTableAsync<UserProfile>();
        await _database.CreateTableAsync<WorkoutSession>();
        await _database.CreateTableAsync<ExerciseCard>();
    }

    // User Profile Operations
    public async Task<UserProfile?> GetUserProfileAsync()
    {
        await InitAsync();
        return await _database!.Table<UserProfile>().FirstOrDefaultAsync();
    }

    public async Task<int> SaveUserProfileAsync(UserProfile profile)
    {
        await InitAsync();
        profile.UpdatedAt = DateTime.UtcNow;

        if (profile.Id != 0)
        {
            return await _database!.UpdateAsync(profile);
        }
        else
        {
            profile.CreatedAt = DateTime.UtcNow;
            return await _database!.InsertAsync(profile);
        }
    }

    // Workout Session Operations
    public async Task<List<WorkoutSession>> GetWorkoutSessionsAsync()
    {
        await InitAsync();
        return await _database!.Table<WorkoutSession>()
            .OrderByDescending(s => s.SessionDate)
            .ToListAsync();
    }

    public async Task<WorkoutSession?> GetWorkoutSessionAsync(int id)
    {
        await InitAsync();
        var session = await _database!.Table<WorkoutSession>()
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();

        if (session != null)
        {
            session.ExerciseCards = await GetExerciseCardsAsync(session.Id);
        }

        return session;
    }

    public async Task<int> SaveWorkoutSessionAsync(WorkoutSession session)
    {
        await InitAsync();
        session.UpdatedAt = DateTime.UtcNow;

        if (session.Id != 0)
        {
            return await _database!.UpdateAsync(session);
        }
        else
        {
            session.CreatedAt = DateTime.UtcNow;
            return await _database!.InsertAsync(session);
        }
    }

    public async Task<int> DeleteWorkoutSessionAsync(WorkoutSession session)
    {
        await InitAsync();
        // Delete all exercise cards first
        await _database!.ExecuteAsync(
            "DELETE FROM ExerciseCard WHERE WorkoutSessionId = ?", session.Id);
        return await _database.DeleteAsync(session);
    }

    // Exercise Card Operations
    public async Task<List<ExerciseCard>> GetExerciseCardsAsync(int sessionId)
    {
        await InitAsync();
        return await _database!.Table<ExerciseCard>()
            .Where(e => e.WorkoutSessionId == sessionId)
            .OrderBy(e => e.Order)
            .ToListAsync();
    }

    public async Task<ExerciseCard?> GetExerciseCardAsync(int id)
    {
        await InitAsync();
        return await _database!.Table<ExerciseCard>()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> SaveExerciseCardAsync(ExerciseCard card)
    {
        await InitAsync();

        if (card.Id != 0)
        {
            return await _database!.UpdateAsync(card);
        }
        else
        {
            card.CreatedAt = DateTime.UtcNow;
            return await _database!.InsertAsync(card);
        }
    }

    public async Task<int> DeleteExerciseCardAsync(ExerciseCard card)
    {
        await InitAsync();
        return await _database!.DeleteAsync(card);
    }

    /// <summary>
    /// Updates the workout session totals based on its exercise cards.
    /// </summary>
    public async Task UpdateSessionTotalsAsync(int sessionId)
    {
        await InitAsync();
        var cards = await GetExerciseCardsAsync(sessionId);
        var session = await _database!.Table<WorkoutSession>()
            .Where(s => s.Id == sessionId)
            .FirstOrDefaultAsync();

        if (session != null)
        {
            session.TotalDurationMinutes = cards.Sum(c => c.DurationMinutes);
            session.TotalCaloriesBurned = cards.Sum(c => c.CaloriesBurned);
            session.UpdatedAt = DateTime.UtcNow;
            await _database.UpdateAsync(session);
        }
    }
}
