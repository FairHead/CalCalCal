# Spec Kit workflow for this repo

This repo is organized so Spec Kit can turn specs into an executable plan and tasks.

## Where to edit the truth
- `specs/stackfit/spec.md` is the **single source of truth** for MVP scope, data model, and algorithms.

## Suggested Spec Kit flow
1. Run Spec Kit **Specify** on `specs/stackfit/spec.md` to refine acceptance criteria.
2. Run Spec Kit **Plan** to generate an ordered implementation plan (vertical slices).
3. Convert plan into tasks/issues (Slice A..G).
4. Implement slice-by-slice in MAUI with tests first for core logic.

## Output files
- Generated planning artifacts should be stored under `specs/stackfit/output/` (kept separate from the canonical spec).
