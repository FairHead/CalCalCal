# StackFit – Projektplan (Repo: CalCalCal)

Projektname: **StackFit**  
Alternativer/Repo-Name: **CalCalCal**

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
StackFit ist eine mobile Fitness-App, die Workouts visuell und logisch in **Session Cards** organisiert. Diese funktionieren wie digitale Trainingszettel, die **gestapelt**, **bearbeitet** und **ausgeführt** werden können. Die App errechnet anhand des Nutzerprofils automatisch **Kalorienverbrauch** und **Trainingsdauer**.

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

### Session
```json
{
  "id": "uuid",
  "title": "Push Day",
  "createdAt": "timestamp",
  "exercises": ["Exercise"],
  "totalCalories": 420,
  "estimatedDuration": 47
}
```

### Exercise
```json
{
  "id": "uuid",
  "name": "Push-Ups",
  "intensity": "high",
  "repetitions": 12,
  "sets": 4,
  "restTimeSec": 60,
  "caloriesBurned": 52
}
```

## Kalorien-Berechnungsmodul
### Formel: Mifflin-St. Jeor (Grundumsatz)
- Männer: `10 * Gewicht + 6.25 * Größe – 5 * Alter + 5`
- Frauen: `10 * Gewicht + 6.25 * Größe – 5 * Alter – 161`

### Übungs-Kalorienverbrauch (MET)
`Kalorien = MET * Gewicht(kg) * Dauer(h)`

| Intensität | MET-Wert |
|---|---:|
| low | 3 |
| medium | 6 |
| high | 8 |

Dauer (h) (Näherung):
`((Sätze × (Reps × 2s + Pause)) / 60) / 60`

Beispiel: Push-Ups, 4 Sätze, 12 Wiederholungen, 60s Pause  
→ ~40 kcal bei mittlerer Intensität, 76 kg

## App-Architektur
### Muster
- MVVM
- Clean Architecture (klar getrennte Verantwortlichkeiten)
- Modularer Aufbau

### Projekt-Module (Zielbild)
- `Core/`: Entities, Interfaces
- `Models/`: Datenstrukturen (DTOs/Records)
- `ViewModels/`: Bindings pro View
- `UI/`: Views + Komponenten
- `Services/`: `CalorieService`, `TimerService`, `ProfileService`
- `Storage/`: SQLite oder Preferences

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
### MVP (v1.0)
- Nutzerprofil
- Session Stack mit 3D Cards
- Übungen als Cards mit Live-Daten
- Kalorienberechnung
- Timer / Pause
- Speicher lokal

### v1.1+
- Cloud Sync
- Wearable Integration
- Trainingsdaten exportieren
- Barcode Scanner
- Social Sharing

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
Empfohlene zentrale Dokumentation:
- `README.md` — Einstieg & Überblick
- `ARCHITEKTUR.md` — technisches Setup
- `FEATURES.md` — Featureliste & Status
- `DATENMODELL.md` — JSON-Modelle
- `KALORIEN-BERECHNUNG.md` — Rechenlogik
- `ROADMAP.md` — Zeitplan & Phasen
- `APP_PLAN_STACKFIT.md` — zentrale Gesamtdoku (dieses Dokument)

## Projektstruktur
Geplante Struktur (VS Code + Copilot freundlich):
```
CalCalCal/
├── README.md
├── ARCHITEKTUR.md
├── FEATURES.md
├── DATENMODELL.md
├── KALORIEN-BERECHNUNG.md
├── ROADMAP.md
├── APP_PLAN_STACKFIT.md
├── .gitignore
├── .editorconfig
├── src/
│   ├── Core/
│   ├── Services/
│   ├── Models/
│   ├── ViewModels/
│   ├── UI/
│   └── Storage/
├── assets/
│   └── wireframes/
└── docs/
    └── ui-design/
```
