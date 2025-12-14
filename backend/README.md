# Backend (CalCalCal)

Backend-Bereich für Tests und später Server/Cloud-Services.

## Projekte

### CalCalCal.Backend
**Aktuell leer** - bereit für Phase 2+:
- Cloud Sync (Firebase / REST API)
- Server-Side Logic
- Optional: Authentication (JWT/OAuth)
- Konflikt-/Merge-Strategie

### CalCalCal.Tests
**xUnit Test-Projekt:**
- Unit Tests für Business Logic
- Tests für Berechnungen (Kalorien, Dauer)
- Später: Integration Tests

## Doku
- Datenmodell: `docs/DATENMODELL.md`
- Gesamtkonzept: `../frontend/docs/APP_PLAN_CALCALCAL.md`

## Hinweis
**MVP ist offline-first!** 

Die CalCalCal.App enthält aktuell ALLE benötigte Logik:
- Models in `frontend/CalCalCal.App/Models/`
- Services in `frontend/CalCalCal.App/Services/`
- Storage in `frontend/CalCalCal.App/Storage/`

Dieser Backend-Ordner ist nur für zukünftige Cloud-Features.
