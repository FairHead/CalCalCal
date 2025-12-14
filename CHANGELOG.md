# CalCalCal - Änderungsprotokoll

**Datum:** 14. Dezember 2025

## Durchgeführte Änderungen

### 1. Komplette Umbenennung von StackFit zu CalCalCal

#### Projekte umbenannt:
- ✅ `StackFit.App` → `CalCalCal.App`
- ✅ `StackFit.Services` → `CalCalCal.Services`
- ✅ `StackFit.Storage` → `CalCalCal.Storage`
- ✅ `StackFit.Core` → `CalCalCal.Core`
- ✅ `StackFit.Models` → `CalCalCal.Models`
- ✅ `StackFit.Tests` → `CalCalCal.Tests`

#### Solution umbenannt:
- ✅ `StackFit.sln` → `CalCalCal.sln`

#### Namespaces geändert:
- ✅ Alle `.cs` Dateien: `namespace StackFit.*` → `namespace CalCalCal.*`
- ✅ Alle `.xaml` Dateien: XAML-Namespace-Deklarationen aktualisiert
- ✅ Alle `.csproj` Dateien: RootNamespace, ApplicationTitle, ApplicationId aktualisiert

### 2. Dokumentation aktualisiert

#### Hauptdokumente:
- ✅ [README.md](README.md) - Haupttitel und Links aktualisiert
- ✅ [START_HERE.md](START_HERE.md) - Alle Befehle mit CalCalCal Namen
- ✅ [ARCHITECTURE.md](ARCHITECTURE.md) - Titel angepasst
- ✅ [PROJEKTSTRUKTUR.md](PROJEKTSTRUKTUR.md) - Komplette Projektstruktur aktualisiert

#### Frontend-Dokumentation:
- ✅ [APP_PLAN_STACKFIT.md](frontend/docs/APP_PLAN_STACKFIT.md) → [APP_PLAN_CALCALCAL.md](frontend/docs/APP_PLAN_CALCALCAL.md)
- ✅ [frontend/README.md](frontend/README.md) - Projektverweise aktualisiert
- ✅ Alle Verweise in Frontend-Docs angepasst

#### Backend-Dokumentation:
- ✅ [backend/README.md](backend/README.md) - Projektbeschreibung erweitert

#### Specs:
- ✅ [specs/stackfit/spec.md](specs/stackfit/spec.md) - Titel zu CalCalCal geändert

#### Neue Dokumentation:
- ✅ [shared/README.md](shared/README.md) - Neu erstellt für Shared-Projekte

### 3. Projektstruktur

Die finale Struktur ist nun:

```
CalCalCal/
├── frontend/
│   └── CalCalCal.App/          # .NET MAUI App (Android, iOS, macOS, Windows)
├── backend/
│   ├── CalCalCal.Services/     # Business Logic
│   └── CalCalCal.Storage/      # Datenpersistierung
├── shared/
│   ├── CalCalCal.Core/         # Kernfunktionalität
│   ├── CalCalCal.Models/       # Domain Models
│   └── CalCalCal.Tests/        # Tests
└── CalCalCal.sln               # Solution
```

### 4. Build-Status

✅ **Build erfolgreich** - Alle Projekte kompilieren ohne Fehler

```bash
# Build-Befehl
dotnet build CalCalCal.sln

# Ergebnis: Erfolg in ~52s
```

### 5. Konsistenz

- ✅ Keine "StackFit"-Referenzen mehr im Code
- ✅ Alle Namespaces konsistent als `CalCalCal.*`
- ✅ Alle Dokumentations-Links funktionieren
- ✅ Application ID: `com.companyname.calcalcal`
- ✅ App-Titel: "CalCalCal"

## Nächste Schritte

1. **Feature-Implementierung**: Spec Slices A-G umsetzen (siehe [spec.md](specs/stackfit/spec.md))
2. **Models definieren**: UserProfile, Session, Exercise in `CalCalCal.Models`
3. **Services implementieren**: CalorieCalculator, Timer in `CalCalCal.Services`
4. **UI erstellen**: MAUI Pages und ViewModels in `CalCalCal.App`
5. **Storage**: SQLite Repository Pattern in `CalCalCal.Storage`

## Referenzen

- 📖 [Projektstruktur](PROJEKTSTRUKTUR.md)
- 📖 [Architektur](ARCHITECTURE.md)
- 📖 [App Plan](frontend/docs/APP_PLAN_CALCALCAL.md)
- 📖 [Spec](specs/stackfit/spec.md)
- 📖 [Getting Started](START_HERE.md)
