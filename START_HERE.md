# Start here (MAUI bootstrap)

**Status: ✅ FERTIG! Projekt ist bereit.**

Die Projektstruktur ist bereits erstellt. Du kannst direkt loslegen!

## Aktuelle Struktur

```
CalCalCal/
├── frontend/
│   └── CalCalCal.App/      # MAUI App - alles drin!
│       ├── Models/         # Bereit für deine Domain Models
│       ├── ViewModels/     # Bereit für MVVM Logic
│       ├── Services/       # Bereit für Berechnungen
│       ├── Storage/        # Bereit für SQLite
│       └── Core/           # Bereit für Interfaces & Helpers
├── backend/
│   ├── CalCalCal.Backend/  # Für später (Cloud-Sync)
│   └── CalCalCal.Tests/    # Tests (xUnit)
└── CalCalCal.sln           # 3 Projekte in Solution
```

## Nächste Schritte

### 1. Spec lesen
Öffne `specs/stackfit/spec.md` und verstehe die Feature-Slices A-G.

### 2. Models erstellen
Erstelle in `frontend/CalCalCal.App/Models/`:
- `UserProfile.cs`
- `Session.cs`
- `Exercise.cs`

### 3. Interfaces definieren
Erstelle in `frontend/CalCalCal.App/Core/Interfaces/`:
- `ICalorieCalculator.cs`
- `ISessionRepository.cs`

### 4. Services implementieren
Erstelle in `frontend/CalCalCal.App/Services/`:
- `CalorieCalculator.cs`
- `DurationCalculator.cs`

### 5. UI bauen
Erstelle Pages und ViewModels:
- `Views/SessionPage.xaml`
- `ViewModels/SessionViewModel.cs`

## Build & Run

```bash
# Build
dotnet build CalCalCal.sln

# Run (Windows)
dotnet run --project frontend/CalCalCal.App/CalCalCal.App.csproj -f net10.0-windows10.0.19041.0
```

## Dokumentation

- 📖 [Projektstruktur](PROJEKTSTRUKTUR.md) - Übersicht
- 📖 [Architektur](ARCHITECTURE.md) - Design Principles
- 📖 [App Plan](frontend/docs/APP_PLAN_CALCALCAL.md) - Detaillierter Plan
- 📖 [Spec](specs/stackfit/spec.md) - Feature Specs A-G
- 📖 [Kalorien-Berechnung](frontend/docs/KALORIEN-BERECHNUNG.md) - Formeln

---

## Was bereits gemacht wurde ✅

1. ✅ MAUI App erstellt (`dotnet new maui -n CalCalCal.App`)
2. ✅ Solution erstellt (`CalCalCal.sln`)
3. ✅ MVVM Package hinzugefügt (`CommunityToolkit.Mvvm`)
4. ✅ Ordnerstruktur erstellt (Models, ViewModels, Services, etc.)
5. ✅ Backend-Projekt für später vorbereitet
6. ✅ Test-Projekt eingerichtet (xUnit)
7. ✅ Alles vereinfacht - keine überflüssigen Projekte!
8. ✅ Build erfolgreich getestet

**Du kannst jetzt mit der Implementierung anfangen! 🚀**
