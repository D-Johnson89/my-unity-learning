# Dungeon of Echoes - My Unity Learning Journey

Backstory and lore in development...

## Fundamentals Branch

Tutorial reference code and C# basics from [Learn Unity - Beginner's Game Development Tutorial](https://www.youtube.com/watch?v=gB1F9G0JXOo&t=10557s).
Covers: Classes, inheritance, MonoBehaviour, basic movement.

## Main Branch - Dungeon of Echoes

My first Unity game built from scratch, applying tutorial concepts.

### Overview

Dungeon of Echoes is a 2D top-down dungeon crawler where the player navigates hand-designed rooms, fights enemies with melee combat, finds basic loot, and tries to survive to a final encounter.

![Dungeons of Echoes Test Scene](/docs/images/doe_test_scene.png)

### Features

- Player movement with newer unity input system package
- Tilemap-based room layout with floors and walls
- Enemy prefabs with defined types (Mouse, Slime, Rat)
- Enemy patrol and combat logic with trigger colliders
- Player combat with trigger colliders and direction based hitbox

### Roadmap

- See full project roadmap here
  [Roadmap](ROADMAP.md)

### Project Structure

- Characters folder: Enemies/ (Bandit, Bat, Crab, Cyclops, Ghost Mouse, Rat, Slime, Spider, Warlock), NPCs/ (Blacksmith, Guard, InnKeeper, Scout, tile_0087), Players/ (Player1, Cleric, Knight, Mage, Rogue)
- Environment folder: Props/ (Decoractive/, Interactive/), Tiles/ (Floors/, TileAssets/, Walls/)
- Items folder: Consumables/, Equipment/ (Shields/), Weapons/ (Magic/, Melee/)
- UI folder: Indicators/
- Palettes: Floor_Tiles, Wall_Tiles
- Prefabs: Mouse, Player, Rat, Slime
- Scripts folder: Characters/ (Enemies/ Player/ CharacterBase.cs), Spawning/ (SpawnConfig.cs, SpawnManager.cs)

### Tech Stack

- Unity 6.3 LTS (6000.3.10f1)
- C#
- 2D sprites
- Kenney.nl Tiny-Dungeon asset pack

![Kenney Tiny-Dungeon Asset Pack](/docs/images/tiny_dungeon_kenney_nl.png)

### Learning Goals

- Apply C# OOP concepts
- Understand Unity's component system
- Implement game mechanics independently
