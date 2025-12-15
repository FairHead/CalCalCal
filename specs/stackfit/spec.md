# CalCalCal (Working Title) — Spec

**Status:** Draft (ready for Spec Kit planning)  
**Last updated:** 2025-12-15  
**Target stack:** .NET 10 + .NET MAUI (Android-first), MVVM, offline-first

---

## 1. Product vision

A mobile app to **create workout sessions as cards** (pinboard style), execute them with timers/rests, and **estimate calories burned** per exercise and per session based on user profile and exercise parameters.

Primary user: solo user (no social in MVP).  
Primary platforms: **Android phones + tablets** first. iOS later.

### 1.1 Core Concept (Original Idea)

> Eine Handy-App, in der man individuell Trainingspläne erstellen kann, welche mit den Profildaten (Alter, Größe, Gewicht, Geschlecht) berechnet, wie viele Kalorien die einzelne Übung und die ganze Session verbraucht.

**Key Features:**
- **Session Cards**:Session Cards (swipebar), die Trainingspläne repräsentieren (z.B. "Rückenplan")
- **Exercise Cards**: Kleine Karten ,welche auf  Sessionkarten mit Übungsname ,Intensität, Reps, Sets und Pausenzeit, stehen , und zusammen eine Trainingsession ergeben 
- **Live Kalorien**: Sofortige Berechnung basierend auf Profildaten(Tagesverbrauch) 
- **Session Totals**: Gesamtkalorien + geschätzte Dauer pro Session
- **Design**: Jung, hip, healthy – Clean UI mit Akzentfarben

---

## 2. MVP scope

### 2.1 Must-have (MVP)
- User profile (local): name ,age, sex, height, weight
- Session management:
  - Create / edit / delete sessions
  - Session = ordered list of exercises, Sessions are also presentetd as Cards , on which the exercise cards are placed and managed
- Exercise card:
  - Name, category/type, optional illustration (later)
  - Sets, reps, weight (optional), rest (seconds), intensity (1–10)
  - Optional: secondsPerRep (default 2.0) and secondsPerSetOverhead (default 10)
- Execute session mode:
  - Step through exercise cards
  - Timer for work + rest
  - Live calories estimate shown per exercise + total
- Offline-first local persistence (SQLite)
- Simple suggestions when typing exercise name (local list first; later online)

### 2.2 Nice-to-have (post-MVP)
- Cloud sync + login
- Online exercise database search
- Analytics, charts, goals, history
- Templates, sharing

### 2.3 Non-goals (MVP)
- Social features, leaderboards
- Medical-grade calorie accuracy (we provide *estimates*)

---

## 3. Key UX flows

### 3.1 Create session
1. Tap “+ Session”
2. Enter session name
3. Add exercise cards via “+ Exercise”
4. Reorder exercises
5. Save session

### 3.2 Execute session
1. Open session
2. Tap “Start”
3. For each exercise:
   - Show sets/reps/weight/intensity
   - Work timer (calculated time)
   - Rest timer
4. Finish -> show summary (total calories + per exercise)

---

## 4. Data model (canonical)

> This is the source of truth for MVP. (Backend model can mirror it later.)

### 4.1 Entities

#### UserProfile
- id (guid)
- createdAt, updatedAt
- sex: `male | female | other | unknown`
- ageYears: int
- heightCm: int
- weightKg: float

#### Session
- id (guid)
- createdAt, updatedAt
- name: string
- notes?: string
- exercises: List<ExerciseInSession>
- isDeleted: bool (soft delete, for future sync)

#### ExerciseInSession
- id (guid)
- sessionId (guid)
- orderIndex: int
- name: string
- type: `strength | cardio | mobility | other`
- intensity: int (1..10)
- sets: int
- reps: int
- weightKg?: float
- restSeconds: int
- secondsPerRep: float (default 2.0)
- secondsPerSetOverhead: float (default 10.0)  // setup/transition time
- customDurationSeconds?: int                   // if set, overrides computed duration
- isDeleted: bool

---

## 5. Calorie estimation (MVP algorithm)

### 5.1 Approach
Use **MET-based estimation**:

**kcal = MET × 3.5 × weightKg / 200 × minutes**

We approximate duration from exercise parameters (unless overridden).

### 5.2 Duration model

If `customDurationSeconds` is set:
- `durationSeconds = customDurationSeconds`

Else:
- `workSecondsPerSet = reps × secondsPerRep + secondsPerSetOverhead`
- `durationSeconds = sets × workSecondsPerSet + (sets - 1) × restSeconds`

> Rest time is included in MVP session time & kcal estimate (configurable later).

### 5.3 Intensity → MET mapping (defaults)

For **strength**:
- intensity 1–3: MET = 3.5
- intensity 4–6: MET = 5.0
- intensity 7–8: MET = 6.0
- intensity 9–10: MET = 8.0

For **cardio**:
- intensity 1–3: MET = 5.0
- intensity 4–6: MET = 7.0
- intensity 7–8: MET = 9.0
- intensity 9–10: MET = 11.0

For **mobility/other**:
- default MET = 2.5 to 4.0 (use 3.0)

### 5.4 Output
- Per exercise: `kcalExercise`
- Per session: sum of all exercises

### 5.5 Testing
Provide deterministic unit tests:
- Given profile 84kg and exercise parameters -> expect kcal within rounding tolerance (e.g., ±0.1)

---

## 6. Architecture (MVP)

### 6.1 MAUI app structure
- Presentation: Pages (Views) + ViewModels (MVVM)
- Domain: models + calorie calculation service
- Data: repository layer + SQLite

Recommended packages:
- CommunityToolkit.Mvvm (MVVM)
- sqlite-net-pcl **or** EF Core SQLite (choose one; default below)

**Default decision for MVP:** `sqlite-net-pcl` (simple, fast to set up in MAUI).

### 6.2 Layers
- `CalCalCal.App.Core/` domain rules (calorie calc, duration calc), interfaces, enums
- `CalCalCal.App.Models/` entities (UserProfile, Session, ExerciseInSession)
- `CalCalCal.App.Storage/` repositories + migrations
- `CalCalCal.App.Services/` app services (navigation, suggestions, calculators)
- `CalCalCal.App.ViewModels/` state + commands
- `CalCalCal.App.Views/` pages + reusable components

---

## 7. Implementation slices (Spec Kit planning input)

### Slice A — Skeleton + Navigation ✅ DONE
- [x] Create MAUI app (`frontend/CalCalCal.App/`)
- [x] Add DI container (built-in MAUI DI)
- [x] Setup navigation shell (`AppShell.xaml`)
- [x] Add theme resources (`DarkTheme.xaml`)
- [x] Add CommunityToolkit.Mvvm package
- [x] Basic MainPage with SessionCard layout
- [x] Floating Action Button (FAB)

### Slice B — Domain + Tests 🔄 IN PROGRESS
- [ ] Create Enums (see 7.1)
- [ ] Create/Update Models (see 7.2)
- [ ] Implement DurationCalculator (see 7.3)
- [ ] Implement CalorieCalculator (see 7.3)
- [ ] Unit tests (see 7.4)

### Slice C — Persistence
- [ ] Add sqlite-net-pcl package
- [ ] SQLite database setup
- [ ] Repositories for UserProfile, Session, Exercise
- [ ] Seed demo data (optional)

### Slice D — Session CRUD UI
- [ ] Session list view (CarouselView for swipe)
- [ ] Create new session
- [ ] Edit session name
- [ ] Delete session (with confirmation)
- [ ] Session stack with 3D effect (max 3 cards rendered)

### Slice E — Exercise Cards UI
- [ ] Add exercise to session
- [ ] Edit exercise (reps, sets, rest, intensity)
- [ ] Delete exercise
- [ ] Reorder exercises (drag & drop)
- [ ] Exercise suggestions (local list)

### Slice F — Execute mode
- [ ] Workout stepper flow
- [ ] Work timer (calculated duration)
- [ ] Rest timer (countdown)
- [ ] Live calorie totals during workout
- [ ] Session summary after completion

### Slice G — Polishing
- [ ] Input validation with error messages
- [ ] Performance optimization (virtualization)
- [ ] Accessibility (semantic labels, contrast)
- [ ] Error handling & edge cases

---

## 7.1 Task: Create Enums

**Location:** `frontend/CalCalCal.App/Core/Enums/`

#### Gender.cs
```csharp
namespace CalCalCal.App.Core.Enums;

public enum Gender
{
    Male,
    Female,
    Other,
    Unknown
}
```

#### ExerciseType.cs
```csharp
namespace CalCalCal.App.Core.Enums;

public enum ExerciseType
{
    Strength,
    Cardio,
    Mobility,
    Other
}
```

---

## 7.2 Task: Create/Update Models

**Location:** `frontend/CalCalCal.App/Models/`

#### UserProfile.cs (NEW - MISSING)
```csharp
namespace CalCalCal.App.Models;

public class UserProfile
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Profile data
    public string Name { get; set; } = string.Empty;
    public Gender Sex { get; set; } = Gender.Unknown;
    public int AgeYears { get; set; }
    public int HeightCm { get; set; }
    public float WeightKg { get; set; }
    
    // Validation
    public bool IsValid => 
        AgeYears >= 10 && AgeYears <= 100 &&
        HeightCm >= 120 && HeightCm <= 230 &&
        WeightKg >= 30 && WeightKg <= 250;
}
```

#### Exercise.cs (UPDATE REQUIRED)
Current model needs these additions:
- `SessionId` (guid)
- `OrderIndex` (int)
- `Type` (ExerciseType enum)
- `WeightKg` (float?, optional)
- `SecondsPerSetOverhead` (float, default 10.0)
- `CustomDurationSeconds` (int?, optional)
- `IsDeleted` (bool)

#### Session.cs (UPDATE REQUIRED)
Current model needs these additions:
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime)
- `Notes` (string?, optional)
- `IsDeleted` (bool)

---

## 7.3 Task: Create Calculator Services

**Location:** `frontend/CalCalCal.App/Services/`

#### IDurationCalculator.cs (Interface)
```csharp
namespace CalCalCal.App.Core.Interfaces;

public interface IDurationCalculator
{
    /// <summary>
    /// Calculate exercise duration in seconds
    /// </summary>
    int CalculateDurationSeconds(Exercise exercise);
    
    /// <summary>
    /// Calculate total session duration in minutes
    /// </summary>
    double CalculateSessionDurationMinutes(Session session);
}
```

#### ICalorieCalculator.cs (Interface)
```csharp
namespace CalCalCal.App.Core.Interfaces;

public interface ICalorieCalculator
{
    /// <summary>
    /// Calculate calories for a single exercise
    /// </summary>
    double CalculateExerciseCalories(Exercise exercise, UserProfile profile);
    
    /// <summary>
    /// Calculate total session calories
    /// </summary>
    double CalculateSessionCalories(Session session, UserProfile profile);
    
    /// <summary>
    /// Get MET value based on exercise type and intensity
    /// </summary>
    double GetMet(ExerciseType type, int intensity);
}
```

#### DurationCalculator.cs (Implementation)
```csharp
namespace CalCalCal.App.Services;

public class DurationCalculator : IDurationCalculator
{
    public int CalculateDurationSeconds(Exercise exercise)
    {
        if (exercise.CustomDurationSeconds.HasValue)
            return exercise.CustomDurationSeconds.Value;
        
        // workSecondsPerSet = reps × secondsPerRep + secondsPerSetOverhead
        double workSecondsPerSet = exercise.Reps * exercise.SecondsPerRep 
                                 + exercise.SecondsPerSetOverhead;
        
        // durationSeconds = sets × workSecondsPerSet + (sets - 1) × restSeconds
        int durationSeconds = (int)(exercise.Sets * workSecondsPerSet 
                            + Math.Max(0, exercise.Sets - 1) * exercise.RestSeconds);
        
        return durationSeconds;
    }
    
    public double CalculateSessionDurationMinutes(Session session)
    {
        int totalSeconds = session.Exercises
            .Where(e => !e.IsDeleted)
            .Sum(e => CalculateDurationSeconds(e));
        
        return totalSeconds / 60.0;
    }
}
```

#### CalorieCalculator.cs (Implementation)
```csharp
namespace CalCalCal.App.Services;

public class CalorieCalculator : ICalorieCalculator
{
    private readonly IDurationCalculator _durationCalculator;
    
    public CalorieCalculator(IDurationCalculator durationCalculator)
    {
        _durationCalculator = durationCalculator;
    }
    
    public double CalculateExerciseCalories(Exercise exercise, UserProfile profile)
    {
        double met = GetMet(exercise.Type, exercise.Intensity);
        int durationSeconds = _durationCalculator.CalculateDurationSeconds(exercise);
        double minutes = durationSeconds / 60.0;
        
        // kcal = MET × 3.5 × weightKg / 200 × minutes
        double kcal = met * 3.5 * profile.WeightKg / 200.0 * minutes;
        
        return Math.Round(kcal, 1);
    }
    
    public double CalculateSessionCalories(Session session, UserProfile profile)
    {
        return session.Exercises
            .Where(e => !e.IsDeleted)
            .Sum(e => CalculateExerciseCalories(e, profile));
    }
    
    public double GetMet(ExerciseType type, int intensity)
    {
        return type switch
        {
            ExerciseType.Strength => intensity switch
            {
                <= 3 => 3.5,
                <= 6 => 5.0,
                <= 8 => 6.0,
                _ => 8.0
            },
            ExerciseType.Cardio => intensity switch
            {
                <= 3 => 5.0,
                <= 6 => 7.0,
                <= 8 => 9.0,
                _ => 11.0
            },
            _ => 3.0 // Mobility/Other
        };
    }
}
```

---

## 7.4 Task: Unit Tests

**Location:** `backend/CalCalCal.Tests/`

#### DurationCalculatorTests.cs
```csharp
namespace CalCalCal.Tests;

public class DurationCalculatorTests
{
    private readonly DurationCalculator _calculator = new();
    
    [Fact]
    public void CalculateDuration_StandardExercise_ReturnsCorrectSeconds()
    {
        // Arrange: 4 sets, 12 reps, 2s per rep, 10s overhead, 60s rest
        var exercise = new Exercise
        {
            Sets = 4,
            Reps = 12,
            SecondsPerRep = 2.0,
            SecondsPerSetOverhead = 10.0,
            RestSeconds = 60
        };
        
        // Act
        int duration = _calculator.CalculateDurationSeconds(exercise);
        
        // Assert: 4 × (12×2 + 10) + 3 × 60 = 4 × 34 + 180 = 136 + 180 = 316s
        Assert.Equal(316, duration);
    }
    
    [Fact]
    public void CalculateDuration_ZeroSets_ReturnsZero()
    {
        var exercise = new Exercise { Sets = 0, Reps = 10 };
        Assert.Equal(0, _calculator.CalculateDurationSeconds(exercise));
    }
    
    [Fact]
    public void CalculateDuration_CustomDuration_OverridesCalculation()
    {
        var exercise = new Exercise
        {
            Sets = 4,
            Reps = 12,
            CustomDurationSeconds = 300
        };
        
        Assert.Equal(300, _calculator.CalculateDurationSeconds(exercise));
    }
}
```

#### CalorieCalculatorTests.cs
```csharp
namespace CalCalCal.Tests;

public class CalorieCalculatorTests
{
    private readonly CalorieCalculator _calculator;
    private readonly UserProfile _profile;
    
    public CalorieCalculatorTests()
    {
        _calculator = new CalorieCalculator(new DurationCalculator());
        _profile = new UserProfile { WeightKg = 84 };
    }
    
    [Fact]
    public void CalculateCalories_StrengthMediumIntensity_ReturnsExpectedKcal()
    {
        // Arrange: 84kg, strength, intensity 5 (MET 5.0), ~5 min
        var exercise = new Exercise
        {
            Type = ExerciseType.Strength,
            Intensity = 5,
            Sets = 4,
            Reps = 12,
            SecondsPerRep = 2.0,
            SecondsPerSetOverhead = 10.0,
            RestSeconds = 60
        };
        
        // Act
        double kcal = _calculator.CalculateExerciseCalories(exercise, _profile);
        
        // Assert: MET 5.0 × 3.5 × 84 / 200 × 5.27 min ≈ 38.8 kcal
        Assert.InRange(kcal, 38.0, 40.0);
    }
    
    [Theory]
    [InlineData(ExerciseType.Strength, 1, 3.5)]
    [InlineData(ExerciseType.Strength, 5, 5.0)]
    [InlineData(ExerciseType.Strength, 8, 6.0)]
    [InlineData(ExerciseType.Strength, 10, 8.0)]
    [InlineData(ExerciseType.Cardio, 3, 5.0)]
    [InlineData(ExerciseType.Cardio, 6, 7.0)]
    [InlineData(ExerciseType.Cardio, 8, 9.0)]
    [InlineData(ExerciseType.Cardio, 10, 11.0)]
    [InlineData(ExerciseType.Mobility, 5, 3.0)]
    public void GetMet_ReturnsCorrectValue(ExerciseType type, int intensity, double expectedMet)
    {
        Assert.Equal(expectedMet, _calculator.GetMet(type, intensity));
    }
}
```

---

## 7.5 Project Status Overview

### Files that EXIST ✅
| File | Status |
|------|--------|
| `CalCalCal.App.csproj` | ✅ Complete |
| `App.xaml` / `App.xaml.cs` | ✅ Complete |
| `AppShell.xaml` | ✅ Complete |
| `MainPage.xaml` | ✅ Basic layout |
| `DarkTheme.xaml` | ✅ Complete |
| `Models/Session.cs` | ⚠️ Needs update |
| `Models/Exercise.cs` | ⚠️ Needs update |
| `.github/copilot.md` | ✅ Complete |
| `.github/workflows/ci.yml` | ✅ Complete |

### Files that are MISSING ❌
| File | Priority | Slice |
|------|----------|-------|
| `Core/Enums/Gender.cs` | 🔴 High | B |
| `Core/Enums/ExerciseType.cs` | 🔴 High | B |
| `Core/Interfaces/IDurationCalculator.cs` | 🔴 High | B |
| `Core/Interfaces/ICalorieCalculator.cs` | 🔴 High | B |
| `Models/UserProfile.cs` | 🔴 High | B |
| `Services/DurationCalculator.cs` | 🔴 High | B |
| `Services/CalorieCalculator.cs` | 🔴 High | B |
| `Storage/DatabaseService.cs` | 🟡 Medium | C |
| `Storage/Repositories/*.cs` | 🟡 Medium | C |
| `ViewModels/SessionViewModel.cs` | 🟡 Medium | D |
| `ViewModels/ProfileViewModel.cs` | 🟡 Medium | D |
| `Views/SessionListPage.xaml` | 🟡 Medium | D |
| `Views/ProfilePage.xaml` | 🟡 Medium | D |
| `Services/TimerService.cs` | 🟢 Low | F |

---

## 7.6 Implementation Order (Recommended)

```
Week 1: Slice B (Domain + Tests)
├── Day 1: Create Enums + UserProfile model
├── Day 2: Update Exercise + Session models
├── Day 3: Implement DurationCalculator + tests
├── Day 4: Implement CalorieCalculator + tests
└── Day 5: Integration, verify all tests pass

Week 2: Slice C (Persistence)
├── Day 1: Add sqlite-net-pcl, create DatabaseService
├── Day 2: Create IRepository interfaces
├── Day 3: Implement ProfileRepository
├── Day 4: Implement SessionRepository
└── Day 5: Implement ExerciseRepository + tests

Week 3-4: Slice D + E (UI)
├── Session list with CarouselView
├── Session CRUD operations
├── Exercise cards UI
├── Live calorie updates
└── Swipe navigation

Week 5: Slice F (Execute Mode)
├── Workout flow
├── Timers
└── Summary screen

Week 6: Slice G (Polish)
├── Validation
├── Error handling
├── Performance
└── Accessibility
```

---

## 8. Appendix (source notes)

The following original docs were consolidated into this spec:
- frontend/docs/APP_PLAN_STACKFIT.md
- frontend/docs/FEATURES.md
- frontend/docs/ARCHITEKTUR.md
- frontend/docs/KALORIEN-BERECHNUNG.md
- backend/docs/DATENMODELL.md
- docs/Task.md

---

## 9. Quick Reference

### Namespaces
```
CalCalCal.App              # Root
CalCalCal.App.Core         # Interfaces, Enums
CalCalCal.App.Models       # Entities
CalCalCal.App.Services     # Business Logic
CalCalCal.App.Storage      # Repositories
CalCalCal.App.ViewModels   # MVVM
CalCalCal.App.Views        # Pages
```

### Key Formulas
```
Duration (seconds):
  workPerSet = reps × secondsPerRep + secondsPerSetOverhead
  total = sets × workPerSet + (sets - 1) × restSeconds

Calories (kcal):
  kcal = MET × 3.5 × weightKg / 200 × minutes

MET Values:
  Strength: 3.5 (low) → 5.0 (med) → 6.0 (high) → 8.0 (max)
  Cardio:   5.0 (low) → 7.0 (med) → 9.0 (high) → 11.0 (max)
  Other:    3.0 (fixed)
```

### Commands
```bash
# Build
dotnet build CalCalCal.sln

# Run tests
dotnet test backend/CalCalCal.Tests/CalCalCal.Tests.csproj

# Run app (Windows)
dotnet run --project frontend/CalCalCal.App -f net10.0-windows10.0.19041.0

# Run app (Android emulator)
dotnet build frontend/CalCalCal.App -f net10.0-android -t:Run
