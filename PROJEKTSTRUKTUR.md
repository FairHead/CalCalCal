# CalCalCal Projektstruktur

## Übersicht - Vereinfachte Struktur

Das Projekt ist **einfach** in zwei Hauptbereiche aufgeteilt:

```
CalCalCal/
├── frontend/
│   └── CalCalCal.App/          # MAUI App - ALLES Frontend hier!
│       ├── Models/             # Domain Models (UserProfile, Session, Exercise)
│       ├── ViewModels/         # MVVM ViewModels
│       ├── Views/              # Pages & UI (XAML)
│       ├── Services/           # Business Logic (Berechnungen, Timer)
│       ├── Storage/            # Datenzugriff (SQLite)
│       ├── Core/               # Utilities, Interfaces, Enums
│       └── Resources/          # Images, Fonts, etc.
│
├── backend/
│   ├── CalCalCal.Backend/      # Später: Server/API (Phase 2+)
│   └── CalCalCal.Tests/        # Unit Tests (xUnit)
│
└── CalCalCal.sln               # Visual Studio Solution (3 Projekte)
```

## Warum so einfach?

✅ **Alles Frontend in einem Projekt** - keine komplizierten Referenzen  
✅ **Schneller Start** - direkt loslegen mit der App  
✅ **Backend vorbereitet** - für später wenn Cloud-Sync kommt  
✅ **Tests getrennt** - saubere Test-Struktur

## Im Vergleich zu vorher

### ❌ Vorher (zu komplex):
```
6 Projekte: App, Core, Models, Services, Storage, Tests
shared/ Ordner mit 3 Projekten
Viele ProjectReferences
```

### ✅ Jetzt (einfach):
```
3 Projekte: App, Backend, Tests
Alles in CalCalCal.App als Ordner
Keine Dependencies außer MAUI Packages
```

## Projekt-Details

### Frontend: CalCalCal.App
**Eine MAUI App mit allem drin:**
- **Models/**: Domain Entities (UserProfile, Session, Exercise)
- **ViewModels/**: MVVM Logic mit CommunityToolkit.Mvvm
- **Views/**: XAML Pages und UI
- **Services/**: Berechnungen (Kalorien, Dauer, Timer)
- **Storage/**: SQLite Datenzugriff
- **Core/**: Interfaces, Enums, Helpers

**Plattformen:** Android, iOS, macOS, Windows

### Backend: CalCalCal.Backend
**Aktuell leer** - vorbereitet für Phase 2:
- Cloud Sync (Firebase/REST API)
- Server-Side Business Logic
- Optional: Login/Auth

### Tests: CalCalCal.Tests
**xUnit Test-Projekt:**
- Tests für Berechnungen
- Tests für Business Logic
- Später: Integration Tests

## Build

```bash
# Komplette Solution bauen
dotnet build CalCalCal.sln

# Nur Frontend
dotnet build frontend/CalCalCal.App/CalCalCal.App.csproj

# App starten (Windows)
dotnet run --project frontend/CalCalCal.App/CalCalCal.App.csproj -f net10.0-windows10.0.19041.0
```

## Nächste Schritte

Siehe [START_HERE.md](START_HERE.md) für die Bootstrap-Anleitung und [specs/stackfit/spec.md](specs/stackfit/spec.md) für die Implementierung der Feature-Slices A-G.
