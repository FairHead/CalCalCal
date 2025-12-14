# Task Roadmap / Checklist – StackFit (CalCalCal)

Diese Datei ist die **umsetzbare Schritt-für-Schritt Checkliste** für StackFit – von “leere MAUI App” bis “fertige MVP App”.

Ziel: **Offline-first** Fitness-App mit **Session Cards** (3D Stack), Live-**Kalorien** + **Dauer** Berechnung, **Timer/Pausen**, Dark Mode + Neon-Akzenten.

Referenz-Doku:
- Produktplan: `frontend/docs/APP_PLAN_STACKFIT.md`
- Architektur: `frontend/docs/ARCHITEKTUR.md`
- Features: `frontend/docs/FEATURES.md`
- Kalorienformeln: `frontend/docs/KALORIEN-BERECHNUNG.md`
- Roadmap (high-level): `frontend/docs/ROADMAP.md`
- Datenmodell (shared): `backend/docs/DATENMODELL.md`

---

## 0) Grundlagen & Arbeitsweise

### 0.1 Definition of Done (DoD) – für jede Task
- [ ] Code ist implementiert und verständlich (Naming, Struktur, kein Dead Code).
- [ ] Unit-Tests vorhanden (wo sinnvoll) oder bewusst begründet “kein Test”.
- [ ] Manuelle Smoke-Tests auf mindestens einem Zielgerät/Emulator.
- [ ] Doku/README aktualisiert, falls sich Verhalten/Setup geändert hat.

### 0.2 Konventionen (empfohlen)
- [ ] **Ordnerstruktur** beibehalten: `frontend/` für App, `backend/` für späteren Sync.
- [ ] **MVVM** strikt: Views = XAML + minimal Code-Behind; Logik in ViewModels/Services.
- [ ] **Reine Berechnungen** als “pure functions” (ohne UI/Storage Abhängigkeit) → sehr gut testbar.
- [ ] **Offline-first**: lokale DB ist “Source of Truth”.

### 0.3 Tools / Voraussetzungen (Windows)
- [ ] Visual Studio 2026 mit .NET MAUI 10 .
- [ ] Android SDK + Emulator oder physisches Android-Gerät.
- [ ] GitHub Account + Repo Zugriff.

---

## 1) Repository & Projekt-Setup (Startpunkt)

### 1.1 Repo Struktur verifizieren
- [ ] `frontend/` enthält nur Doku/Assets/Platzhalter (noch keine App-Dateien).
- [ ] `backend/` enthält nur Doku (noch keine Serverimplementierung).
- [ ] `.gitignore` ignoriert IDE/Build Artefakte (bin/obj/.vs/.idea).

### 1.2 Neues MAUI Projekt erzeugen (noch ohne Features)
Ziel: “Hello World” App läuft lokal, Clean Start.

- [ ] Im Ordner `frontend/` eine Solution erstellen (Variante A: minimal 1 Projekt).
  - [ ] `dotnet new maui` erstellen (z. B. `frontend/src/StackFit.App/`)
  - [ ] Optional: `frontend/StackFit.sln` anlegen und App Projekt hinzufügen
- [ ] (Empfohlen) Zusätzlich eine testbare Core Library (Variante B: Clean/Modular):
  - [ ] `frontend/src/StackFit.Core/` als `dotnet new classlib`
  - [ ] `frontend/tests/StackFit.Core.Tests/` als `dotnet new xunit`
  - [ ] Projekt-Referenzen setzen: App → Core, Tests → Core

Akzeptanzkriterien:
- [ ] App baut lokal (Windows + Android Build zumindest).
- [ ] Tests laufen lokal (Core Tests).
- [ ] Repo bleibt sauber (keine bin/obj committed).

### 1.3 “Platzhalter-Struktur” zu echter Struktur migrieren
Aktuell gibt es Platzhalter: `frontend/src/Core`, `Models`, `Services`, `Storage`, `UI`, `ViewModels`.

Entscheidung treffen:
- [ ] **Option 1 (ein Projekt)**: Platzhalter-Ordner werden zu Unterordnern im MAUI Projekt.
- [ ] **Option 2 (mehrere Projekte, empfohlen)**: Platzhalter-Ordner entfallen; Struktur liegt in Projekten:
  - `StackFit.Core` enthält Entities/Enums/Calculations
  - `StackFit.App` enthält UI/ViewModels
  - Storage als eigenes Projekt oder im App-Projekt unter `Storage/`

Aktion:
- [ ] Gewählte Option in `frontend/docs/ARCHITEKTUR.md` kurz festhalten.

---

## 2) Domain Model (Entities) + Validierung

Quelle: `backend/docs/DATENMODELL.md`

### 2.1 Enums & Basistypen
- [ ] `Gender` (male/female)
- [ ] `Intensity` (low/medium/high)
- [ ] `ActivityLevel` (low/medium/high) (optional für spätere Tageswerte)

### 2.2 Entities (MVP)
- [ ] `UserProfile` (age, weightKg, heightCm, gender, activityLevel?)
- [ ] `Session` (id, title, createdAt, exercises[])
- [ ] `Exercise` (id, sessionId oder direkte Liste, name, intensity, repetitions, sets, restTimeSec)

### 2.3 Validation Rules (MVP)
Implementiere klare Regeln + Fehlermeldungen:
- [ ] Age: 10–100
- [ ] Weight: 30–250 kg
- [ ] Height: 120–230 cm
- [ ] Sets/Reps/Rest: ≥ 0, sinnvoll begrenzen (z. B. reps <= 200)

Akzeptanzkriterien:
- [ ] Entities sind serialisierbar (für lokale Speicherung).
- [ ] Validierung ist zentral (nicht in 20 UI-Stellen verteilt).

---

## 3) Kalorien- & Dauerberechnung (Core Logik)

Quelle: `frontend/docs/KALORIEN-BERECHNUNG.md`

### 3.1 MET Mapping + Dauerformel
- [ ] MET Mapping (low=3, medium=6, high=8)
- [ ] Dauerberechnung (MVP-Formel aus Doku)
- [ ] Alternative Formel (“Rest nur zwischen Sets”) als Option dokumentieren
- [ ] Rundungsstrategie festlegen (kcal int, Dauer in Minuten/Sekunden)

### 3.2 CalorieService / DurationService (testbar)
- [ ] Reine Funktionen implementieren:
  - [ ] Exercise → duration
  - [ ] Exercise + profile → calories
  - [ ] Session(exercises) → totalDuration + totalCalories
- [ ] Edge Cases:
  - [ ] sets=0 oder reps=0 → 0
  - [ ] negative Eingaben → Validation Error

### 3.3 Unit Tests (Pflicht)
Tests, die die Doku-Beispiele abdecken:
- [ ] Push-Ups Beispiel (4x12, 60s, medium, 76kg) ≈ ~40–43 kcal
- [ ] 0 Sets / 0 Reps → 0 kcal
- [ ] Intensität Mapping korrekt
- [ ] (Optional) BMR (Mifflin-St. Jeor) Funktion testbar

Akzeptanzkriterien:
- [ ] Tests laufen deterministisch und schnell.
- [ ] Berechnung ist unabhängig von UI/DB.

---

## 4) Lokale Speicherung (Offline First)

### 4.1 Storage Technologie wählen
Empfehlung: SQLite (stabil für Listen/Beziehungen).
- [ ] Entscheidung: SQLite vs Preferences/JSON (kurz dokumentieren)

### 4.2 Datenbank-Schema + Migration
- [ ] Tabellen: user_profile, sessions, exercises
- [ ] Foreign Keys / Indizes (sessionId)
- [ ] `updatedAt` vorbereiten (für späteren Sync)
- [ ] (Optional) Soft Delete: `deletedAt`

### 4.3 Repositories / Interfaces
- [ ] `IProfileRepository` (Load/Save)
- [ ] `ISessionRepository` (CRUD Sessions)
- [ ] `IExerciseRepository` (CRUD Exercises, oder über SessionRepo)
- [ ] Implementierung in Storage Layer

### 4.4 Seed / Demo Data (nur Dev)
- [ ] “Push Day” Demo Session erstellen (optional, abschaltbar)

Akzeptanzkriterien:
- [ ] App startet ohne Netz.
- [ ] Daten bleiben nach App-Neustart erhalten.

---

## 5) UI/UX Grundsystem (Dark + Neon)

Quelle: `frontend/docs/ui-design/README.md`

### 5.1 Theme Tokens
- [ ] Dark Mode Default
- [ ] Neon Accent Farben definieren (z. B. Cyan/Magenta/Green)
- [ ] Typografie + Spacing Tokens (MVP)

### 5.2 Komponenten-Basis
Ziel: wiederverwendbare Bausteine statt copy/paste.
- [ ] SessionCard (Container)
- [ ] ExerciseCard (2-Spalten Grid)
- [ ] FooterCard (Totals)
- [ ] Floating Action Button (+)
- [ ] Settings Panel (Overlay/Sheet)

Akzeptanzkriterien:
- [ ] Konsistenter Style über alle Screens.
- [ ] Touch Targets groß genug (>= 44px).

---

## 6) Main Screen: Session Cards Stack (MVP Kern)

### 6.1 Navigation/Screen Strategie
MVP: 1 Hauptscreen + Settings.
- [ ] Shell/Navigation minimal halten (kein komplexes Menü)
- [ ] Swipe Navigation definieren (horizontaler Session-Stack)

### 6.2 Session Stack Rendering (Performance)
- [ ] Max. 3 Cards gleichzeitig gerendert (Top/Next/Third)
- [ ] Smooth Animation/Transforms (3D-Effekt)
- [ ] Card Tap → Edit Mode (oder Inline Edit)

### 6.3 Session CRUD (UI)
- [ ] Neue Session erstellen (FAB)
- [ ] Session Titel editieren
- [ ] Session löschen (Confirm)

Akzeptanzkriterien:
- [ ] Neue Session in <2 Sekunden anlegbar.
- [ ] UI bleibt flüssig (kein ruckeln beim Swipe).

---

## 7) Exercise Cards (Edit) + Live Totals

### 7.1 Exercise CRUD
- [ ] Exercise hinzufügen (in Session)
- [ ] Exercise bearbeiten: reps, sets, rest, intensity
- [ ] Exercise löschen

### 7.2 Live Berechnung
Bei jeder Änderung sofort:
- [ ] caloriesBurned pro Exercise sichtbar
- [ ] totalCalories + totalDuration im Session Footer sichtbar

### 7.3 Validierung/UX
- [ ] Eingaben validieren (Inline Fehler oder dezente Hinweise)
- [ ] Default Werte (z. B. sets=3, reps=10, rest=60, intensity=medium)

Akzeptanzkriterien:
- [ ] Keine “NaN/Crash” bei leeren Eingaben.
- [ ] Werte werden gespeichert (offline).

---

## 8) Timer + Session Ausführung (Workout Mode)

### 8.1 Timer Grundfunktionen
- [ ] Start/Pause/Reset
- [ ] Countdown Anzeige
- [ ] Pausen-Countdown (restTimeSec)

### 8.2 Ablaufmodell (MVP)
Entscheidung:
- [ ] “Pro Exercise Timer” (einfach)
- [ ] oder “Set-by-Set” (realistischer)

### 8.3 Hintergrund/Foreground Handling
- [ ] App in Background → Timer bleibt korrekt (Zeitdelta)
- [ ] Wieder öffnen → Timer State korrekt anzeigen

Akzeptanzkriterien:
- [ ] Timer driftet nicht signifikant.
- [ ] Nutzer kann jederzeit pausieren/fortsetzen.

---

## 9) Settings + UserProfile

### 9.1 Profil UI
- [ ] Age, Weight, Height, Gender
- [ ] (Optional) ActivityLevel
- [ ] Validierung + Speichern

### 9.2 Theme/Appearance
- [ ] Dark Mode default
- [ ] Optional: Accent Color Auswahl (später)

Akzeptanzkriterien:
- [ ] Profiländerung aktualisiert Berechnung sofort.

---

## 10) Qualität: Testing, Performance, Accessibility

### 10.1 Unit Tests erweitern
- [ ] Mehr Tests für Grenzfälle (große Werte, Nullwerte)
- [ ] Session Totals Tests

### 10.2 ViewModel Tests (wenn möglich)
- [ ] Commands setzen State korrekt
- [ ] Persist/Load Pfad

### 10.3 Accessibility
- [ ] Semantic Labels für Buttons/Inputs
- [ ] Kontrast prüfen (Neon auf Dark)

### 10.4 Performance
- [ ] Render Budget: nur 3 Cards
- [ ] Keine unnötigen Rebuilds bei jedem Keypress (Debounce optional)

---

## 11) CI/CD (GitHub Actions) – MVP

### 11.1 Build Pipeline
- [ ] Build Frontend (Android) on PR
- [ ] Run Unit Tests
- [ ] Optional: Code Coverage report

### 11.2 Release Pipeline
- [ ] Versioning Strategy (SemVer)
- [ ] Sign Android builds (Keystore Secrets)
- [ ] Upload artifacts (APK/AAB) als GitHub Release

Hinweis: iOS Builds benötigen macOS Runner + Signing.

---

## 12) “Fertigstellen” Kriterien für MVP v1.0

MVP ist fertig, wenn:
- [ ] Profil funktioniert + gespeichert
- [ ] Sessions/Exercises offline gespeichert
- [ ] Live calories + duration stimmt (Näherung) und ist getestet
- [ ] Timer ist zuverlässig
- [ ] Main Screen Session Stack ist benutzbar & performant
- [ ] Dark Mode + Neon Akzente konsistent
- [ ] CI baut Android + Tests laufen

---

## 13) Phase 2+ (Optional): Backend / Cloud Sync

### 13.1 Sync Konzept
- [ ] Konfliktstrategie definieren (last-write-wins vs field-merge)
- [ ] Datenmodell: updatedAt/deletedAt, Change Tracking

### 13.2 Auth (optional)
- [ ] JWT/OAuth Plan
- [ ] Anonymous Mode weiterhin möglich

### 13.3 Backend Implementierung
Optionen:
- [ ] Firebase (Firestore/Auth)
- [ ] Eigene Web API (ASP.NET Core)

Akzeptanzkriterien:
- [ ] Offline bleibt voll funktionsfähig.
- [ ] Sync ist robust bei Konflikten und schlechtem Netz.

---

## Umsetzungs-Roadmap (Kurz-Checkliste)

### Phase A – Setup (1–2 Tage)
- [ ] MAUI App scaffold + Git clean
- [ ] Core Library + Tests scaffold
- [ ] CI Build + Tests (Android)

### Phase B – Core & Storage (3–7 Tage)
- [ ] Entities + Validation
- [ ] Calorie/Duration Services + Unit Tests
- [ ] SQLite + Repositories

### Phase C – UI MVP (7–14 Tage)
- [ ] Theme (Dark + Neon)
- [ ] Session Stack UI (max 3 Cards)
- [ ] Exercise Grid + Live Totals
- [ ] Settings/Profile UI

### Phase D – Workout Mode (3–7 Tage)
- [ ] Timer + Pausen
- [ ] Background handling
- [ ] UX Polish

### Phase E – Stabilisierung (laufend, 3–7 Tage)
- [ ] Performance optimieren
- [ ] Accessibility/Usability fixes
- [ ] Bugfixing + Test Coverage

### Phase F – Release MVP (1–3 Tage)
- [ ] App Versioning
- [ ] Release Build Android (AAB)
- [ ] GitHub Release + Changelog



---
> Note: `docs/Task.md` is a legacy checklist. The canonical source is now `specs/stackfit/spec.md`.
