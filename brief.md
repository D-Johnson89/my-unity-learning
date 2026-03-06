# Project Brief: Dungeon of Echoes — 2D Top-Down Dungeon Crawler
**Category:** Game Development  
**Stack Freedom:** ✅ Open (architecture and design choices are yours)  
**Language Constraint:** C# (Unity)  
**Engine:** Unity 6 LTS  
**Difficulty:** Beginner → Novice  
**Estimated Scope:** 8–16 weeks (part-time, building on existing foundation)  
**Note:** *This is your project. You already have the player movement foundation, CharacterBase inheritance, and the EnemyController structure in place. This brief scopes it into a completable, shippable game.*

---

## Overview

Dungeon of Echoes is a 2D top-down dungeon crawler where the player navigates hand-designed rooms, fights enemies with melee combat, finds basic loot, and tries to survive to a final encounter. The scope is intentionally tight — one dungeon, one player class, a handful of enemy types, a boss. The goal is a **finished, playable game** rather than an ever-expanding prototype.

This brief picks up where your existing codebase left off. The architecture decisions you've already made (CharacterBase inheritance, separation of concerns between base class and controllers, pre-placed enemies) are validated and built upon here.

---

## Problem Statement

Most solo game dev projects never ship because scope creep kills them. This project is scoped to finish. One dungeon. One boss. Core loop only. Polish what you have. Ship it.

---

## Existing Foundation (What You Already Have)

Your current codebase includes:

- Player movement system (MovePosition + `Time.deltaTime`, WASD input)
- `CharacterBase` (abstract) with shared `Move()` logic
- `PlayerController` extending CharacterBase
- `EnemyController` extending CharacterBase (partially implemented — patrol/chase state machine stub)
- Slime and Rat prefabs created with colliders and Rigidbody2D
- Kenney asset packs organized in the project
- Git repo with `main` and `fundamentals` branches

Everything below builds directly on this.

---

## Core Requirements (MVP — Shippable Game)

A game is only done when a person who didn't make it can pick it up and have a complete experience. These features define that threshold:

### Player System
- **Melee Attack** — A single attack animation with a hitbox (trigger collider) that deals damage to enemies in range. Attack should have a brief cooldown to prevent spam.
- **Health System** — Player has a defined max health. Taking damage reduces it. Reaching zero triggers a game over.
- **Invincibility Frames** — Brief period of invulnerability after taking damage. Prevents enemy contact from instantly draining health.

### Enemy System
- **Patrol Behavior** — Enemies follow waypoints in their patrol path when the player is out of detection range.
- **Chase Behavior** — Enemy transitions to chase state when the player enters detection radius. Uses steering toward player's position.
- **Leash / Return Behavior** — If the player exits a maximum range from the enemy's spawn point, the enemy stops chasing and returns to patrol.
- **Contact Damage** — Enemies deal damage when colliding with the player. Respects player's invincibility frames.
- **Enemy Health & Death** — Enemies have health. Dying from the player's attack removes the enemy from the scene and optionally drops a pickup.

### Room / Level System
- **Minimum Two Rooms** — The player begins in a starting room and progresses through at least two distinct areas before the boss.
- **Room Transitions** — Doors or passage colliders that move the player to the next room. You can use scene loading or camera bounds — your call.
- **Room Locking** — A room locks (doors close) when enemies are present and unlocks on enemy clear. This is optional but highly recommended for feel.

### Loot System (Minimal)
- **Health Pickup** — A collectible that restores a set amount of health.
- **Optional: Key Item** — An item the player must find before the boss door opens.

### Boss Encounter
- **Boss Enemy** — One enemy with significantly more health, a distinct attack pattern (different from standard enemies — e.g., ranged projectile, charge attack, or spawning minions), and a health bar UI element.
- **Win Condition** — Defeating the boss triggers a "You Win" screen or message.

### UI / HUD
- **Player Health Bar** — On-screen health representation (bar or hearts — your preference).
- **Game Over Screen** — Triggered on player death. Includes a "Try Again" button that restarts from the beginning.
- **Win Screen** — Triggered on boss defeat.

---

## Architecture Guidance (Building on What You Have)

These are not requirements, but recommendations based on your existing codebase and the decisions you've already validated:

### Attack System
- Consider a separate `AttackController` component on the player, rather than adding attack logic directly to `PlayerController`. This keeps CharacterBase clean and follows the separation of concerns you've already established.
- Use a child GameObject with a `Collider2D` (trigger) as the attack hitbox. Enable it briefly on attack input, disable after the swing.

### Enemy AI (Completing EnemyController)
- Your existing state machine stub (`enum State { Patrol, Chase, Return }`) is the right approach. Implement `PatrolBehavior()`, `ChaseBehavior()`, and `ReturnBehavior()` as distinct private methods.
- Use `Physics2D.OverlapCircle()` for detection radius checks — it's cleaner than trigger colliders for AI awareness.
- `Vector2.MoveTowards()` is a good simple option for chase movement.

### Health & Damage
- Consider a `HealthComponent` (or just add health fields to `CharacterBase`) so both players and enemies share the same "take damage / die" interface.
- Use a `DamageDealerComponent` or an interface (`IDamageable`) so any object can deal or receive damage without needing to know what it's hitting.

### ScriptableObjects (Optional but Powerful)
- Enemy stats (speed, health, damage, detection radius) can be defined in ScriptableObjects. This lets you create different enemy "types" by changing a data asset, not the code. If you want to learn one new concept, this is the highest-value one for game dev.

---

## Stretch Goals

- **Procedural Room Layouts** — Instead of hand-placed rooms, use a simple grid-based room generator.
- **Enemy Variants** — A ranged enemy that fires projectiles toward the player.
- **Inventory System** — Basic inventory (3–5 slots) for holding and using items.
- **Save System** — Checkpoint system using `PlayerPrefs` or JSON serialization that saves room progress.
- **Minimap** — Simple minimap using an overhead camera with a RenderTexture.
- **Sound & Music** — Background music loop, hit SFX, footstep SFX using Unity's AudioSource.
- **Particle Effects** — Hit flash on damage, death particle burst using Unity's Particle System.
- **Controller Support** — Unity's new Input System supports gamepads natively.

---

## Art & Asset Direction

You're already using Kenney assets — stick with them. Consistency matters more than variety.

- Kenney's "Dungeon Pack" or "Roguelike/RPG Pack" are free and comprehensive.
- Build rooms using Unity's Tilemap system for floor and wall tiles.
- Use sprite layers (Sorting Layers) to manage: Floor → Items → Characters → UI.

---

## Professional Standards Expected

- **README.md** — Description, how to run in Unity Editor, controls, known issues.
- **Git workflow** — Continue using your existing branch strategy: `feature/*`, `fix/*`, `refactor/*` branches merged into `main`.
- **Prefab discipline** — All enemies, pickups, and projectiles must be prefabs. Never hard-code scene-specific references in scripts.
- **No magic numbers** — Speed, health, damage, radii should be serialized fields on the Inspector, not raw literals in code.
- **One responsibility per script** — If a script is doing too many things, split it.

---

## Learning Resources

| Topic | Resource |
|---|---|
| Unity 2D Tilemap | [docs.unity3d.com → Tilemap](https://docs.unity3d.com/Manual/class-Tilemap.html) |
| Unity ScriptableObjects | [docs.unity3d.com → ScriptableObject](https://docs.unity3d.com/Manual/class-ScriptableObject.html) |
| Unity new Input System | [docs.unity3d.com → Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/manual/index.html) |
| Physics2D.OverlapCircle | [docs.unity3d.com → Physics2D.OverlapCircle](https://docs.unity3d.com/ScriptReference/Physics2D.OverlapCircle.html) |
| Unity Coroutines (invincibility frames) | [docs.unity3d.com → Coroutines](https://docs.unity3d.com/Manual/Coroutines.html) |
| C# interfaces in Unity | [gamedevbeginner.com/interfaces-in-unity](https://gamedevbeginner.com/interfaces-in-unity/) |
| Kenney free assets | [kenney.nl/assets](https://kenney.nl/assets) |
| Unity Particle System basics | [docs.unity3d.com → Particle System](https://docs.unity3d.com/Manual/PartSysReference.html) |

---

## What This Teaches You (and Validates)

By completing this project, you will have:

- Shipped a complete game with a start, middle, and end — most devs never do this
- Implemented a full game loop: player input → combat → enemy response → win/lose condition
- Practiced OOP inheritance in a real game context (CharacterBase design)
- Applied Unity component architecture correctly (one responsibility per script)
- Built AI behavior with a state machine pattern
- Managed scene/game state (locked rooms, pickups, game over)
- Written production-quality C# that someone else could read and understand

---

*Finish this one. Seriously. A shipped project — even a simple one — is worth ten unfinished ambitious ones on a portfolio.*
