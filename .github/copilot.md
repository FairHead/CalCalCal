# Copilot Instructions for CalCalCal

You are working in a **.NET 10 / C# 13 .NET MAUI** fitness app.

---

## 🔴 Golden Rules (MUST FOLLOW)

1. **Follow the canonical spec**: `specs/stackfit/spec.md` is the SINGLE SOURCE OF TRUTH
2. **MVVM Pattern**: Use `CommunityToolkit.Mvvm` for all ViewModels
3. **Pure Domain Logic**: Keep calculators and models free of UI/Storage dependencies
4. **Async/Await**: Use for ALL IO operations (database, file, network)
5. **Unit Tests**: Required for `CalorieCalculator`, `DurationCalculator`, and Repositories
6. **Nullable Safety**: Project uses `<Nullable>enable</Nullable>` - handle nulls explicitly

---

## 📁 Namespace Conventions

```
CalCalCal.App                    # Root namespace
CalCalCal.App.Core               # Interfaces, Enums, Constants
CalCalCal.App.Core.Enums         # Gender, ExerciseType
CalCalCal.App.Core.Interfaces    # ICalorieCalculator, IDurationCalculator, IRepository<T>
CalCalCal.App.Models             # Entities: UserProfile, Session, Exercise
CalCalCal.App.Services           # CalorieCalculator, DurationCalculator, TimerService
CalCalCal.App.Storage            # DatabaseService, Repositories
CalCalCal.App.ViewModels         # SessionViewModel, ProfileViewModel, etc.
CalCalCal.App.Views              # XAML Pages and Controls
```

---

## 🎨 Naming Conventions

### C# Code
| Element | Convention | Example |
|---------|------------|---------|
| Classes | PascalCase | `CalorieCalculator` |
| Interfaces | I + PascalCase | `ICalorieCalculator` |
| Methods | PascalCase | `CalculateCalories()` |
| Properties | PascalCase | `WeightKg` |
| Private fields | _camelCase | `_durationCalculator` |
| Parameters | camelCase | `exercise`, `userProfile` |
| Constants | PascalCase | `DefaultSecondsPerRep` |
| Enums | PascalCase (singular) | `ExerciseType.Strength` |

### XAML
| Element | Convention | Example |
|---------|------------|---------|
| x:Name | PascalCase | `SessionTitle` |
| StaticResource | PascalCase | `{StaticResource AccentBlue}` |
| Binding | PascalCase property | `{Binding TotalCalories}` |

### Tests
| Pattern | Example |
|---------|---------|
| `MethodName_Scenario_ExpectedResult` | `CalculateCalories_ZeroSets_ReturnsZero` |

---

## 📐 Code Style Rules

### Prefer
```csharp
// ✅ Expression-bodied for simple members
public double TotalCalories => Exercises.Sum(e => e.EstimatedCalories);

// ✅ Target-typed new
private readonly List<Exercise> _exercises = new();

// ✅ Pattern matching
return type switch
{
    ExerciseType.Strength => 5.0,
    ExerciseType.Cardio => 7.0,
    _ => 3.0
};

// ✅ Nullable with explicit handling
public string? Notes { get; set; }
if (exercise.CustomDurationSeconds is int customDuration)
    return customDuration;

// ✅ Primary constructors (C# 12+)
public class CalorieCalculator(IDurationCalculator durationCalculator) : ICalorieCalculator
```

### Avoid
```csharp
// ❌ Avoid null-forgiving without reason
var name = exercise.Name!;  // Bad

// ❌ Avoid magic numbers
double met = 5.0;  // Bad - use named constant or method

// ❌ Avoid large methods (> 30 lines, extract into smaller methods)
```

---

## 🧮 Calorie Calculation (MET-Based) - SCIENTIFIC REFERENCE

### Source: ACSM (American College of Sports Medicine) + Compendium of Physical Activities

### What is MET?
- **MET** = Metabolic Equivalent of Task
- **1 MET** = energy expenditure at rest = 3.5 ml O₂/kg/min
- Higher MET = more intense exercise

### Official Formula (ACSM)
```
Calories per minute = (MET × 3.5 × weightKg) / 200

Total Calories = Calories per minute × duration in minutes
```

**Combined Formula:**
```
kcal = MET × 3.5 × weightKg / 200 × minutes
```

**Alternative (simplified):**
```
kcal = MET × weightKg × hours
```

### Duration Calculation for Strength Training
```
workSecondsPerSet = (reps × secondsPerRep) + secondsPerSetOverhead
durationSeconds = (sets × workSecondsPerSet) + ((sets - 1) × restSeconds)
```

**Default values:**
- `secondsPerRep` = 2.0 (time under tension)
- `secondsPerSetOverhead` = 10.0 (setup/transition time)

---

## 📊 MET Values (from Compendium of Physical Activities)

### Strength Training
| Intensity | MET | Description |
|-----------|-----|-------------|
| 1-3 (Low) | 3.5 | Light weights, easy effort |
| 4-6 (Medium) | 5.0 | Moderate weights, moderate effort |
| 7-8 (High) | 6.0 | Heavy weights, vigorous effort |
| 9-10 (Max) | 8.0 | Very heavy, near-max effort |

### Cardio
| Intensity | MET | Description |
|-----------|-----|-------------|
| 1-3 (Low) | 5.0 | Light jogging, slow cycling |
| 4-6 (Medium) | 7.0 | Jogging, moderate cycling |
| 7-8 (High) | 9.0 | Running, fast cycling |
| 9-10 (Max) | 11.0 | Sprinting, very fast cycling |

### Mobility/Other
| Type | MET |
|------|-----|
| Stretching | 2.3 |
| Yoga (Hatha) | 2.5 |
| Yoga (Power) | 4.0 |
| **Default** | **3.0** |

---

## ✅ Validation Example

**Input:**
- Person: 84 kg
- Exercise: Bench Press
- 4 sets × 12 reps
- 60 seconds rest
- Intensity: 5 (Medium) → MET 5.0

**Duration Calculation:**
```
workPerSet = 12 × 2.0 + 10.0 = 34 seconds
totalDuration = 4 × 34 + 3 × 60 = 136 + 180 = 316 seconds
minutes = 316 / 60 = 5.27 minutes
```

**Calorie Calculation:**
```
kcal = MET × 3.5 × weightKg / 200 × minutes
kcal = 5.0 × 3.5 × 84 / 200 × 5.27
kcal = 1470 / 200 × 5.27
kcal = 7.35 × 5.27
kcal ≈ 38.7 kcal
```

---

## 🗄️ Repository Pattern

### Interface
```csharp
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> SaveAsync(T entity);
    Task<bool> DeleteAsync(string id);
}
```

### Usage
```csharp
// ✅ Always use interface, not implementation
public class SessionViewModel(IRepository<Session> sessionRepository) : ObservableObject
```

---

## 💉 Dependency Injection

Register services in `MauiProgram.cs`:
```csharp
builder.Services.AddSingleton<IDurationCalculator, DurationCalculator>();
builder.Services.AddSingleton<ICalorieCalculator, CalorieCalculator>();
builder.Services.AddSingleton<IRepository<Session>, SessionRepository>();
builder.Services.AddTransient<SessionViewModel>();
```

---

## ⚠️ Error Handling

```csharp
// ✅ Validate early, fail fast
public double CalculateCalories(Exercise exercise, UserProfile profile)
{
    ArgumentNullException.ThrowIfNull(exercise);
    ArgumentNullException.ThrowIfNull(profile);

    if (profile.WeightKg <= 0)
        throw new ArgumentException("Weight must be positive", nameof(profile));

    // ... calculation
}
```

---

## ✅ Input Validation Rules

| Field | Min | Max | Unit |
|-------|-----|-----|------|
| Age | 10 | 100 | years |
| Weight | 30 | 250 | kg |
| Height | 120 | 230 | cm |
| Sets | 0 | 50 | count |
| Reps | 0 | 200 | count |
| Rest | 0 | 600 | seconds |
| Intensity | 1 | 10 | scale |
| SecondsPerRep | 0.5 | 10.0 | seconds |

---

## 🔄 Work Slicing

Implement in order (finish one before starting next):

1. **Slice A** ✅ Skeleton + Navigation
2. **Slice B** 🔄 Domain + Tests (Enums, Models, Calculators)
3. **Slice C** ⬜ Persistence (SQLite, Repositories)
4. **Slice D** ⬜ Session CRUD UI
5. **Slice E** ⬜ Exercise Cards UI
6. **Slice F** ⬜ Execute Mode (Timers)
7. **Slice G** ⬜ Polishing

---

## 📚 Reference Documentation

| Document | Path | Content |
|----------|------|---------|
| **Canonical Spec** | `specs/stackfit/spec.md` | Single Source of Truth |
| **Data Model** | `backend/docs/DATENMODELL.md` | Entity definitions |
| **Calorie Formulas** | `frontend/docs/KALORIEN-BERECHNUNG.md` | Algorithm details |
| **UI Design** | `frontend/docs/ui-design/README.md` | Visual specifications |
| **Architecture** | `frontend/docs/ARCHITEKTUR.md` | Layer structure |

---

## 🔬 Scientific Sources

- **ACSM** - American College of Sports Medicine Guidelines
- **Compendium of Physical Activities** (Ainsworth et al., 2011)
- **ACE Fitness** - Calorie calculation methodology
