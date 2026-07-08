# Orange-you-curious 🎮
### A first-person puzzle game

---

## 🎯 Game Concept

Rico-Chet is a first-person platformer-puzzle hybrid set above a sea of lava. The player navigates Mario-style platforming challenges using a single mechanic: a toy dart gun that activates the environment. Buttons open doors, extend platforms, and trigger timed sequences. The gun is your only verb — everything else is movement and timing.

---

## 🕹️ Controls

| Input | Action |
|---|---|
| WASD | Move |
| Mouse | Look |
| Space | Jump |
| Left Click | Shoot dart |
| Escape | Pause |
| R (win screen) | Restart |
| M (win screen) | Main Menu |

---

## 📐 SOLID Principles

Every script in this project was written with SOLID in mind:

**S — Single Responsibility**
Each script has exactly one job.
- `PuzzleButton` only handles button activation logic
- `Door` only handles door sliding logic
- `MovingPlatform` only handles platform movement
- `CheckpointManager` only handles player respawning
- `AudioManager` only handles sound playback

**O — Open/Closed**
The puzzle system is open for extension, closed for modification. Adding a new puzzle object (e.g. a SpikeTrap) requires zero changes to existing scripts — it simply implements `IActivatable` and `IButtonReaction`.

**L — Liskov Substitution**
Every `IButtonReaction` implementor (Door, MovingPlatform, FuseTimer) can be swapped for another without breaking the system. PuzzleButton does not care what is listening — it just fires the event.

**I — Interface Segregation**
Three small focused interfaces instead of one large one:
- `IActivatable` — for anything the dart can hit
- `IButtonReaction` — for anything that reacts to a button
- `IResettable` — for anything that resets on player death

**D — Dependency Inversion**
`Dart` never references `Door`, `MovingPlatform`, or any concrete type. It only calls `IActivatable.Activate()` — it depends on the abstraction, never the implementation. The gun never knows what it hit.

---

## 🧩 Design Patterns

This project consciously applies 4 design patterns, each mapped to a specific system:

### 1. Observer Pattern
**Where:** `PuzzleButton.cs` using `UnityEvent`
**Why:** The button (Subject) notifies subscribers (Observers) when activated. Door, MovingPlatform, and FuseTimer all subscribe via the Inspector. The button never knows what it is notifying — zero coupling between cause and effect. Adding new reactions requires zero changes to PuzzleButton.

### 2. Command Pattern
**Where:** `ShootCommand.cs` implementing `ICommand`
**Why:** Each shot is encapsulated as an object with an `Execute()` method. ToyGun creates the command, the command handles instantiation and launching. This decouples the gun from the dart — ToyGun never touches Dart directly.

### 3. Strategy Pattern
**Where:** `IButtonReaction` implemented by `Door`, `MovingPlatform`, `FuseTimer`
**Why:** Each object defines its own reaction behavior independently. PuzzleButton fires one event — each subscriber responds differently based on its own Strategy. Swapping behaviors requires no changes to the button.

### 4. Factory Pattern
**Where:** `ToyGun.cs` creating `Dart` via `ShootCommand`
**Why:** Dart instantiation is handled through ShootCommand, not directly by ToyGun. The gun depends only on ICommand — it never calls Instantiate directly, keeping creation logic separate from firing logic.

---

## 🏗️ Architecture Overview
