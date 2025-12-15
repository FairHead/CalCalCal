# UI Design Notes

## Look & Feel
- Dark Mode (schwarzer Hintergrund) als Default
- Neon-Akzente (z. B. Cyan/Magenta/Grün) über Theme-Tokens
- Große Cards, klare Typografie, wenig Text

## Main Screen
- 3D-Kartenstapel, max. 3 Karten gleichzeitig rendern (Performance)
- Horizontaler Swipe für Sessions
- Floating „+“ zum Hinzufügen

## Components
- SessionCard: Titel, Totals (kcal/min), Exercise Grid (2 Spalten)
- ExerciseCard: Name, Slider, Reps/Sets/Pause + Icon/Bild
- Footer: Summen + schneller Link zu Settings

---

# UI Task List – CalCalCal

## UI Design – Verbindliche Vorgaben

### Design-Referenz (MUSS eingehalten werden)
- Das UI orientiert sich **visuell und strukturell** an der bereitgestellten Figma-/Screenshot-Referenz:
  - Dark Mode Default
  - Große, abgerundete Cards
  - Neon-Akzente (Blau / Gelb) **sparsam**
  - Klare Typografie, wenig Text
- Abweichungen vom Referenzdesign sind **nicht erlaubt**, außer sie sind explizit begründet (z. B. Performance).

---

## Screen: Main Page (Session Overview)

### Layout & Struktur
- Schwarzer / sehr dunkler Hintergrund
- Zentrale SessionCard als Fokus
- Horizontaler Swipe zwischen Sessions
- Maximal **3 SessionCards gleichzeitig rendern** (prev / current / next)
- Fake-3D-Effekt über:
  - Scale
  - Y-Offset
  - Shadow (keine echten 3D-Transforms)

---

## Component: SessionCard (Main Page)

### Header
- Session Title (z. B. „Back Day“)
- Meta-Info:  
  `X exercises • ~YY min`
- Typografie:
  - Title: groß, bold
  - Meta: klein, reduziert, sekundäre Farbe

### Exercise Overview
- Grid mit **2 Spalten**
- Anzeige von **ExercisePreviewCards** (keine Editoren!)
- Grid ist scrollfähig innerhalb der SessionCard

### Footer (SessionCard-intern oder global sticky)
- Anzeige:
  - Total Calories (dominant)
  - Einheit: `kcal`
  - Hinweis: `based on profile data (age • height • weight)`
- Footer ist sticky, safe-area aware

---

## Component: ExercisePreviewCard (Main Page)

### Zweck
- **Reine Übersicht**, keine komplexen Eingaben
- Schnelles Scannen der Session

### Inhalte (verpflichtend)
- Übungsbild (Thumbnail)
- Übungsname (max. 1–2 Zeilen)
- Sets / Reps kompakt (z. B. `12 / 15 / 15`)
- Intensity als:
  - Prozentwert
  - kleiner Balken (read-only)
- Geschätzte Calories (z. B. `~85 kcal`)

### Nicht erlaubt auf Main Page
- ❌ Slider
- ❌ Textfelder
- ❌ Editierbare Controls
- ❌ Lange Beschreibungen

---

## Interaction: Exercise Tap

- Tap auf ExercisePreviewCard öffnet:
  - BottomSheet oder DetailPage
- Kein Inline-Editing auf der Main Page

---

## Component: ExerciseEditorCard (Detail / BottomSheet)

### Inhalte
- Übungsname (fix)
- Intensity Slider (Primary Control)
- Sets / Reps / Pause editierbar
- Optional:
  - Tempo (`secondsPerRep`)
  - Gewicht / RPE
- Übungsbild größer anzeigen
- Änderungen wirken sich sofort auf:
  - Exercise Calories
  - Session Total Calories aus

---

## Component: Floating Action Button (FAB)

- Position: unten rechts
- Funktion:
  - Neue Session hinzufügen
- Kollidiert **nicht** mit Footer
- Neon-Farbe (Primary Accent)

---

## Farben & Tokens

### Grundfarben
- Background: sehr dunkel (`#0B0B0F` / ähnlich)
- Card Background: leicht heller als Background
- Text:
  - Primär: Off-White
  - Sekundär: gedämpftes Grau

### Akzentfarben
- Blau: Intensity / Slider / Progress
- Gelb: Calories (sparsam einsetzen)
- Maximal **eine dominante Akzentfarbe pro Screen**

---

## Animation & Performance

### Vorgaben
- Keine komplexen 3D-Animationen
- Shadows nur auf SessionCard
- ExerciseCards ohne Shadow
- Bilder:
  - Lazy Load
  - Caching aktiv

### Swipe Verhalten
- Snap-Paging
- Keine Slider-Interaktion während horizontalem Swipe

---

## Accessibility & Usability
- Touch Targets ≥ 44x44 px
- Kontraste Dark Mode geeignet
- Slider nicht zu fein (Schritte sinnvoll)

---

## Definition of Done (UI)
- UI entspricht visuell der Referenz
- Main Page bleibt übersichtlich bei ≥ 6 Exercises
- Keine Edit-Controls auf der SessionCard
- Session Totals sind immer sichtbar
- UI läuft flüssig auf Midrange-Android-Geräten
