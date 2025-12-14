# Frontend (CalCalCal)

Die **CalCalCal.App** - eine .NET MAUI Cross-Platform App für Android, iOS, macOS und Windows.

## Struktur

```
CalCalCal.App/
├── Models/          # Domain Models (UserProfile, Session, Exercise)
├── ViewModels/      # MVVM ViewModels mit CommunityToolkit.Mvvm
├── Views/           # XAML Pages & UI (MainPage, SessionPage, etc.)
├── Services/        # Business Logic (CalorieCalculator, Timer, etc.)
├── Storage/         # SQLite Datenzugriff (Repositories)
├── Core/            # Interfaces, Enums, Helpers
├── Resources/       # Images, Fonts, Icons
└── Platforms/       # Platform-specific Code (Android, iOS, Windows)
```

## Doku
- Projektplan: `docs/APP_PLAN_CALCALCAL.md`
- Architektur: `docs/ARCHITEKTUR.md`
- Features & Status: `docs/FEATURES.md`
- Kalorien-Berechnung: `docs/KALORIEN-BERECHNUNG.md`
- Roadmap: `docs/ROADMAP.md`
- UI Design Notes: `docs/ui-design/README.md`
- Wireframes: `assets/wireframes/README.md`

## Build & Run
```bash
# Build
dotnet build CalCalCal.App/CalCalCal.App.csproj

# Run (Windows)
dotnet run --project CalCalCal.App/CalCalCal.App.csproj -f net10.0-windows10.0.19041.0

# Run (Android) - requires Android SDK
dotnet build CalCalCal.App/CalCalCal.App.csproj -f net10.0-android
```

## Packages
- **Microsoft.Maui** - Cross-Platform UI Framework
- **CommunityToolkit.Mvvm** - MVVM Helpers
- **SQLite** (später hinzufügen) - Lokale Datenbank
