# CalCalCal - Projektplan

Projektname: **CalCalCal**

## Inhaltsverzeichnis
- [Vision & Scope](#vision--scope)
- [Zielgruppe](#zielgruppe)
- [User Stories & Use Cases](#user-stories--use-cases)
- [UI/UX Designkonzept](#uiux-designkonzept)
- [Komponenten](#komponenten)
- [Datenmodell (Entities)](#datenmodell-entities)
- [Kalorien-Berechnungsmodul](#kalorien-berechnungsmodul)
- [App-Architektur](#app-architektur)
- [API / Sync](#api--sync)
- [Requirements](#requirements)
- [MVP & Roadmap](#mvp--roadmap)
- [Risiken & Entscheidungen](#risiken--entscheidungen)
- [Testing / CI/CD](#testing--cicd)
- [Dateien im Repo](#dateien-im-repo)
- [Projektstruktur](#projektstruktur)

## Vision & Scope
### Vision
CalCalCal ist eine mobile Fitness-App, die Workouts visuell und logisch in **Session Cards** organisiert. Diese funktionieren wie digitale Trainingszettel, die **gestapelt**, **bearbeitet** und **ausgeführt** werden können. Die App errechnet anhand des Nutzerprofils automatisch **Kalorienverbrauch** und **Trainingsdauer**.

### Scope (MVP)
- Nutzerprofil mit Alter, Gewicht, Größe, Geschlecht
- Dark Mode mit Neon-Akzenten
- Hauptscreen mit Session Cards als 3D-Kartenstapel (max. 3 sichtbar)
- Übungen auf Karten anlegen & konfigurieren (Reps, Sets, Pause, Intensität)
- Live-Kalorien- und Zeitberechnung
- Timer und Pausenfunktion
- Offlinefähig (lokale Speicherung)

## Zielgruppe
- 16–40 Jahre, fitnessaffin
- Fokus auf Krafttraining / funktionelles Training
- Tech-affin, minimalistisch orientiert
- Möchte offline arbeiten oder ohne Abo trainieren

## User Stories & Use Cases
### User Stories
- Als Nutzer möchte ich meine eigenen Trainingspläne erstellen.
- Als Nutzer will ich direkt beim Anlegen der Übungen sehen, wie viele Kalorien ich verbrenne.
- Als Nutzer will ich meine Sessions durch Wischgesten durchgehen.
- Als Nutzer will ich alles auf einem übersichtlichen Bildschirm erledigen können.

### Use Cases
- Neue Trainingseinheit erstellen
- Übungen hinzufügen
- Wiederholungen, Sets, Pausen & Intensität festlegen
- Kalorien & Dauer anzeigen
- Session starten und mit Timer tracken

## UI/UX Designkonzept
### Main Screen
- Schwarzer Hintergrund (Dark Mode)
- Sichtbare 3D-Session-Karten (max. 3 gleichzeitig sichtbar)
- Horizontal swipebar (Session-Stack)
- Floating „+“-Button zum Erstellen neuer Session Cards

### Session Card
- Enthält: Titel, Dauer, Kalorien
- Übungen als kleine Cards in 2-Spalten-Grid
- Mini-Footer zeigt: Gesamtkalorien, Gesamtdauer

### Exercise Card
- ⅔ Text: Übungsname, Intensität (Slider), Reps, Sets, Pause
- ⅓ rechts: Icon oder Bild der Übung
- Kalorienverbrauch direkt sichtbar

### Navigationsstruktur
- Floating Home Button
- Floating Add Session Button
- Settings über kleinen Footer-Link

## Komponenten
| Komponente | Beschreibung |
|---|---|
| SessionCard | Große Karte mit Übungen |
| ExerciseCard | Kompakte Übungseinheit |
| FAB | Floating Button zum Hinzufügen |
| TimerComponent | Start/Pause/Reset + Countdown-Funktion |
| FooterCard | Zeigt Kalorien- & Zeit-Summe |
| SettingsPanel | Profil & Farbanpassung |

## Datenmodell (Entities)
Siehe: `../../backend/docs/DATENMODELL.md`

## Kalorien-Berechnungsmodul
Siehe: `KALORIEN-BERECHNUNG.md`

## App-Architektur
### Muster
- MVVM
- Clean Architecture
- Modularer Aufbau

### Projekt-Module (Zielbild)
- `frontend/src/Core/`: Entities, Interfaces
- `frontend/src/Models/`: Datenstrukturen (DTOs/Records)
- `frontend/src/ViewModels/`: Bindings pro View
- `frontend/src/UI/`: Views + Komponenten
- `frontend/src/Services/`: `CalorieService`, `TimerService`, `ProfileService`
- `frontend/src/Storage/`: SQLite oder Preferences

## API / Sync
- MVP: **Offline First**
- Phase 2+: Firebase / Web API für Cloud-Sync
- Profil optional mit Login (JWT oder OAuth später)

## Requirements
### Functional
- Session erstellen
- Übungen hinzufügen & bearbeiten
- Timer starten & stoppen
- Kalorien live anzeigen
- Navigation via Swipe

### Non-Functional
- Hohe Performance
- Dark Mode
- Offlinefähigkeit
- Touch-first Bedienung

## MVP & Roadmap
Siehe: `ROADMAP.md`

## Risiken & Entscheidungen
| Risiko | Entscheidung / Lösung |
|---|---|
| UI zu komplex | Nur 1 Hauptscreen mit Layer-Stack |
| Offline-Modus ohne Sync | Daten lokal halten, später Export/Sync |
| Kalorien ungenau | Näherungsformel + Profildaten, später Feintuning |
| Plattformabhängigkeit | Cross-Platform mit MAUI |

## Testing / CI/CD
- Unit Tests für Kalorien- und Dauerberechnung
- UI Tests (z. B. MAUI TestKit)
- E2E Tests (Appium oder Playwright)
- CI/CD mit GitHub Actions (Android & iOS Build Pipelines)

## Dateien im Repo
- Repo Einstieg: `../../README.md`
- Frontend Einstieg: `../README.md`
- Backend Einstieg: `../../backend/README.md`

Frontend Doku:
- `APP_PLAN_CALCALCAL.md` — zentrale Projektdokumentation
- `ARCHITEKTUR.md` — Architektur (MVVM + Clean Architecture)
- `FEATURES.md` — Featureliste & Status
- `KALORIEN-BERECHNUNG.md` — Rechenlogik
- `ROADMAP.md` — Zeitplan & Phasen
- `ui-design/README.md` — UI Design Notes

Backend Doku:
- `../../backend/docs/DATENMODELL.md` — Entities + Storage/Sync Schema

## Projektstruktur
Aktuelle Struktur:
```text
CalCalCal/
  README.md
  CalCalCal.sln
  PROJEKTSTRUKTUR.md
  ARCHITECTURE.md
  START_HERE.md
  frontend/
    CalCalCal.App/      # MAUI App
    docs/
      APP_PLAN_CALCALCAL.md
      ARCHITEKTUR.md
      FEATURES.md
      KALORIEN-BERECHNUNG.md
      ROADMAP.md
      ui-design/
    assets/
      wireframes/
  backend/
    CalCalCal.Services/ # Business Logic
    CalCalCal.Storage/  # Datenzugriff
    docs/
      DATENMODELL.md
  shared/
    CalCalCal.Core/     # Kernfunktionalität
    CalCalCal.Models/   # Domain Models
    CalCalCal.Tests/    # Tests
```
