# Copilot instructions

You are working in a .NET 10 / C# 13 .NET MAUI app.

## Golden rules
- Follow the canonical spec: `specs/stackfit/spec.md`
- Prefer MVVM (CommunityToolkit.Mvvm)
- Keep domain logic pure and testable (no UI dependencies)
- Use async/await everywhere IO happens
- Add unit tests for calculators and repositories

## Project conventions (planned)
Namespaces:
- `StackFit.Core` — domain logic
- `StackFit.Models` — entities
- `StackFit.Storage` — persistence
- `StackFit.Services` — app services
- `StackFit.ViewModels` — MVVM
- `StackFit.UI` — Views

## Work slicing
Implement Slice A..G as defined in spec. Finish one slice before starting the next.
