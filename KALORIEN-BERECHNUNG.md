# Kalorien- & Dauerberechnung (StackFit / CalCalCal)

## Ziele
- Live‑Feedback beim Konfigurieren von Übungen
- Einfache, erklärbare Näherung (MVP)
- Testbar (deterministisch, ohne UI-Abhängigkeiten)

## Grundumsatz (Mifflin‑St. Jeor)
Männer:
`BMR = 10 * weightKg + 6.25 * heightCm - 5 * age + 5`

Frauen:
`BMR = 10 * weightKg + 6.25 * heightCm - 5 * age - 161`

Hinweis: Im MVP ist der BMR optional (z. B. für spätere Tages-/Zielwerte). Für Workout‑Kalorien reicht Gewicht + MET.

## Workout‑Kalorien (MET)
Formel:
`kcal = MET * weightKg * durationHours`

MET‑Mapping (MVP):
| Intensität | MET |
|---|---:|
| low | 3 |
| medium | 6 |
| high | 8 |

## Dauerberechnung (Näherung)
Konstante Annahme:
- `workSecondsPerRep = 2`

Näherung (wie im Konzept):
`durationHours = ((sets * (reps * workSecondsPerRep + restTimeSec)) / 60) / 60`

Alternative (realistischer, Pausen nur zwischen Sätzen):
`durationSec = sets * reps * workSecondsPerRep + max(0, sets - 1) * restTimeSec`

## Beispiel (Push‑Ups)
Input:
- Gewicht: 76 kg
- Intensität: medium (MET 6)
- 4 Sätze, 12 Reps, 60s Pause

Näherung:
- Dauer: `4 * (12*2 + 60) = 336s = 0.0933h`
- Kalorien: `6 * 76 * 0.0933 ≈ 42.5 kcal` (~40 kcal)

## Rundung (Vorschlag)
- Kalorien: auf ganze kcal runden
- Dauer: Minuten als Integer (auf-/abrunden je nach UX)
