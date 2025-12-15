# Datenmodell (CalCalCal)

> Hinweis: Dieses Datenmodell ist **shared**: es wird vom Frontend (lokale Speicherung) und später vom Backend (Sync/API) genutzt.

## Überblick
Die App speichert lokal:
- **UserProfile** (genau 1)
- **Sessions** (0..n)
- **Exercises** (0..n pro Session)

IDs sind UUIDs als String.

## Enums (Vorschlag)
### Gender
- `male`
- `female`

### ActivityLevel (optional, später für Tagesumsatz)
- `low`
- `medium`
- `high`

### Intensity (Workout)
- `low`
- `medium`
- `high`

## Entities
### UserProfile
```json
{
  "age": 27,
  "weight": 76,
  "height": 178,
  "gender": "male",
  "activityLevel": "medium"
}
```

Validierung (MVP, Vorschlag):
- `age`: 10–100
- `weight`: 30–250 (kg)
- `height`: 120–230 (cm)

### Session
```json
{
  "id": "uuid",
  "title": "Push Day",
  "createdAt": "timestamp",
  "totalCalories": 420,
  "estimatedDuration": 47
}
```

Hinweis: `totalCalories` und `estimatedDuration` sind abgeleitet und werden beim Ändern von Exercises neu berechnet.

### Exercise
```json
{
  "id": "uuid",
  "sessionId": "uuid",
  "name": "Push-Ups",
  "intensity": "medium",
  "repetitions": 12,
  "sets": 4,
  "restTimeSec": 60,
  "caloriesBurned": 42
}
```

## SQLite-Schema (MVP Vorschlag)
### user_profile
- `id` (TEXT, PK, constant e.g. `profile`)
- `age` (INTEGER)
- `weightKg` (REAL)
- `heightCm` (REAL)
- `gender` (TEXT)
- `activityLevel` (TEXT)
- `updatedAt` (INTEGER)

### sessions
- `id` (TEXT, PK)
- `title` (TEXT)
- `createdAt` (INTEGER)
- `updatedAt` (INTEGER)

### exercises
- `id` (TEXT, PK)
- `sessionId` (TEXT, FK -> sessions.id)
- `name` (TEXT)
- `intensity` (TEXT)
- `repetitions` (INTEGER)
- `sets` (INTEGER)
- `restTimeSec` (INTEGER)
- `updatedAt` (INTEGER)

## Derived Values (nicht persistieren oder nur cachen)
- `Exercise.caloriesBurned`
- `Session.totalCalories`
- `Session.estimatedDuration`

Empfehlung: im MVP optional cachen, aber immer aus Basisfeldern reproduzierbar halten.
