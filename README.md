# DeckSaga

A Unity-based turn-based card game inspired by Slay the Spire. Features deck building, combat system, enemy AI, and an event-driven action system architecture.

## Features

- **Turn-Based Combat System** - Strategic card battles with mana management
- **Event-Driven Architecture** - Flexible action system with pre/post reactions
- **Card System** - Draw cards, play cards, discard pile management
- **Enemy AI** - Multiple enemies with attack patterns
- **Status Effects** - Burn mechanic and customizable status effects
- **Target Modes** - Multiple targeting options (all enemies, random, hero)

## Architecture

```
Architecture: Event-Driven + Action System

Core Components:
├── GameAction          - Base class for all game actions
├── ActionSystem        - Manages action flow: Pre → Perform → Post
├── Singleton<T>        - Base singleton for systems
└── Systems            - CardSystem, EnemySystem, ManaSystem, etc.

Patterns Used:
• Event-Driven Architecture
• Singleton Pattern
• Factory Pattern
• Command Pattern
```

## Project Structure

```
Assets/Scripts/
├── General/          # Base components (Singleton, ActionSystem)
├── Models/           # Data models (Card, Effect, TargetMode)
├── Data/             # Data assets (CardData, HeroData, EnemyData)
├── Systems/          # Core game systems
├── GameActions/      # Game actions (PlayCard, DrawCards, etc.)
├── Effects/          # Card effects (DealDamage, DrawCards, etc.)
├── Views/            # UI views (CardView, HandView, HeroView)
├── UI/               # UI components
└── TargetModes/      # Target selection modes
```

## Tech Stack

- **Engine**: Unity 2022+
- **UI**: Unity UGUI + TextMesh Pro
- **Animation**: DOTween
- **Language**: C#

## Getting Started

1. Clone this repository
2. Open the project in Unity (2022 or later)
3. Open `Assets/Scenes/SampleScene.unity`
4. Press Play to run the game

## License

MIT