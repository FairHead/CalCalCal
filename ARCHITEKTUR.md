# Architektur (StackFit / CalCalCal)

## Zielbild
StackFit ist eine Offline‑First Fitness‑App (MAUI), die Workouts über **Session Cards** abbildet. Kernziele:
- **Schnell & touch-first**: 1 Hauptscreen, Wischen statt Menüs.
- **Offlinefähig**: lokale Speicherung, Sync optional später.
- **Berechnungen in Echtzeit**: Kalorien + Dauer sofort sichtbar.
- **Testbar**: Berechnungslogik als reine, deterministische Services.

## Architekturprinzipien
- **MVVM** für UI-Logik und Bindings
- **Clean Architecture** zur Trennung von Domain/Use‑Cases/Infra/UI
- **Dependency Inversion**: UI kennt nur Interfaces, nicht Implementierungen
- **Offline‑First**: lokale Datenquelle ist „Source of Truth“

## Schichten (empfohlen)
### Core (Domain)
- Entities: `UserProfile`, `Session`, `Exercise`
- Value Objects / Enums: `Gender`, `Intensity`, `ActivityLevel`
- Interfaces: `ISessionRepository`, `IProfileRepository`, `ICalorieCalculator`, `ITimerService`

### Application (Use Cases)
- Use‑Cases/Services: Berechnung, Timersteuerung, Session‑Editing, Validierung
- Orchestrierung: lädt Daten, führt Berechnung aus, schreibt zurück

### Infrastructure (Storage / Sync)
- SQLite: Sessions/Exercises/Profile
- Preferences: kleine Settings (Theme, letzter Screen)
- Später: Sync‑Adapter (Firebase/Web API) mit Konfliktstrategie

### UI (Presentation)
- Views (Pages), Components (Cards), Styles/Themes
- ViewModels als einzige UI‑Logik

## Datenfluss (vereinfacht)
1. User ändert Übung (Reps/Sets/Intensität)
2. ViewModel aktualisiert `Exercise`-State
3. `CalorieService` berechnet Dauer + Kalorien (pure functions)
4. ViewModel zeigt Live‑Totals (Card Footer) an
5. Repository persistiert Änderungen lokal

## Storage (MVP)
Empfehlung: SQLite als primäre Quelle.
- Tabellen: `UserProfile`, `Session`, `Exercise`
- Beziehung: `Session (1) -> (n) Exercise`

Alternative (nur MVP klein): Preferences/JSON — aber SQLite ist robuster für Listen/Beziehungen.

## Sync (Phase 2+)
Offline‑First Sync-Ansatz:
- Lokale Daten bleiben führend (Source of Truth).
- Jede Entity hat: `id`, `updatedAt`, optional `deletedAt` (Soft‑Delete).
- Konflikte: „last write wins“ (MVP) oder Merge-Regeln pro Feld (später).

## UI/UX technische Notizen
- Dark Mode als Default, Neon‑Akzentfarben über Theme‑Tokens
- Session Stack: max. 3 Karten gerendert (Performance)
- Gesten: horizontaler Swipe für Sessions, Tap/Drag für Edit

## Testing
Priorität:
- Unit‑Tests für `CalorieService` und Dauerberechnung
- ViewModel‑Tests (State, Commands)
- UI/E2E später (Appium/Playwright)
