# CalCalCal (Working Title) — Spec

**Status:** Draft (ready for Spec Kit planning)  
**Last updated:** 2025-12-14  
**Target stack:** .NET 10 + .NET MAUI (Android-first), MVVM, offline-first

---

## 1. Product vision

A mobile app to **create workout sessions as cards** (pinboard style), execute them with timers/rests, and **estimate calories burned** per exercise and per session based on user profile and exercise parameters.

Primary user: solo user (no social in MVP).  
Primary platforms: **Android phones + tablets** first. iOS later.

---

## 2. MVP scope

### 2.1 Must-have (MVP)
- User profile (local): age, sex, height, weight
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
- `Core/` domain rules (calorie calc, duration calc)
- `Models/` entities
- `Storage/` repositories + migrations
- `Services/` app services (navigation, suggestions)
- `ViewModels/` state + commands
- `UI/` pages + reusable components

---

## 7. Implementation slices (Spec Kit planning input)

### Slice A — Skeleton + Navigation
- Create MAUI app
- Add DI container
- Setup navigation shell
- Add theme resources

### Slice B — Domain + Tests
- Implement DurationCalculator
- Implement CalorieCalculator
- Unit tests

### Slice C — Persistence
- SQLite setup
- Repositories for UserProfile, Session, ExerciseInSession

### Slice D — Session CRUD UI
- Session list + create/edit/delete

### Slice E — Exercise Cards UI
- Add/edit exercise card
- Reorder exercises
- Basic suggestions list

### Slice F — Execute mode
- Stepper flow
- Work/rest timers
- Live calorie totals

### Slice G — Polishing
- Validation, error handling
- Performance for large sessions

---

## 8. Appendix (source notes)

The following original docs were consolidated into this spec:
- frontend/docs/APP_PLAN_STACKFIT.md
- frontend/docs/FEATURES.md
- frontend/docs/ARCHITEKTUR.md
- frontend/docs/KALORIEN-BERECHNUNG.md
- backend/docs/DATENMODELL.md
- docs/Task.md
