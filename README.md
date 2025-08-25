# Pro_4 - Unity Tower Defense Game

A feature-rich 3D tower defense game built with Unity, featuring multiple tower types, enemy varieties, and strategic gameplay mechanics.

## 🎮 Game Overview

Pro_4 is a tower defense game where players strategically place and upgrade towers to defend against waves of enemies. The game features a modular architecture with extensible systems for towers, enemies, and level progression.

## ✨ Key Features

### Tower System
- **7 Unique Tower Types:**
  - **Crossbow**: Accurate ranged attacks
  - **Canon**: High damage explosive projectiles
  - **Rapid Fire Gun**: Fast attack speed with continuous damage
  - **Hammer**: Melee range with devastating impact
  - **Spider Nest**: Area control with spawning mechanics
  - **AA Harpoon**: Anti-air specialized tower
  - **Only Fan**: Special utility tower

### Enemy Varieties
- **Standard Enemies**: Basic ground units with varying health and speed
- **Stealth Enemies**: Units that can become invisible and bypass detection
- **Boss Units**: Large, powerful enemies with special abilities
- **Swarm Enemies**: Fast-moving units that attack in groups

### Game Systems
- **Dynamic Build System**: Click-to-place towers with real-time preview
- **Wave Management**: Progressive difficulty with timed enemy spawns
- **Level Progression**: Multiple levels with unique layouts and challenges
- **Resource Management**: Currency system for tower purchases and upgrades
- **Camera Effects**: Dynamic camera movements and visual effects
- **Audio System**: Immersive sound effects and background music

## 🏗️ Project Architecture

### Core Systems

#### **NhoxBehaviour Base Class**
All game components inherit from `NhoxBehaviour`, providing:
- Automatic component loading
- Consistent initialization patterns
- Debug logging capabilities

#### **Core Component System**
Modular component architecture for entities:
- **Core**: Central entity management
- **Movement**: Entity movement and pathfinding
- **Stats**: Health, damage, and attribute management
- **Visuals**: Rendering and visual effects
- **Death**: Entity destruction and cleanup

#### **Manager Systems**
- **BuildManager**: Handles tower placement and building mechanics
- **LevelManager**: Scene loading and level progression
- **WaveManager**: Enemy spawning and wave progression
- **AudioManager**: Sound and music management
- **TowerPreviewManager**: Real-time tower placement preview

### Tower System Architecture
```
TowerCtrl (Main Controller)
├── Attack Components (Targeting, Damage)
├── Rotation Components (Auto-aim)
├── Visual Components (Effects, Animation)
└── Specialized Components (per tower type)
```

### Build System
- **BuildSlots**: Predefined positions for tower placement
- **GridBuilder**: Level layout and pathfinding grid
- **TowerPreview**: Real-time visual feedback during placement
- **BuildBtnsUI**: User interface for tower selection

## 🛠️ Technology Stack

- **Engine**: Unity 3D
- **Language**: C# (.NET)
- **Architecture**: Component-based Entity System
- **Input System**: Unity Input System
- **Audio**: Unity Audio System
- **Effects**: Custom particle systems (CFXR)

## 📁 Project Structure

```
Assets/
├── _Data/                    # Core game scripts and data
│   ├── BuildSystem/          # Tower building mechanics
│   ├── Core/                 # Core component system
│   ├── Enemy/                # Enemy types and behaviors
│   ├── Tower/                # Tower implementations
│   ├── UI/                   # User interface components
│   ├── LevelSystem/          # Level management
│   ├── WaveManager/          # Wave spawning system
│   ├── Audio/                # Audio management
│   └── ScriptableObject/     # Game configuration data
├── _Scenes/                  # Game scenes
│   ├── MainScene.unity       # Main gameplay scene
│   ├── GameLevel/            # Level selection
│   └── TestScene/            # Development testing
├── _Prefabs/                 # Reusable game objects
├── _Graphics/                # Visual assets
└── _Audio/                   # Sound and music files
```

## 🚀 Getting Started

### Prerequisites
- Unity 2022.3 LTS or later
- .NET Framework 4.8+
- Git

### Setup Instructions

1. **Clone the Repository**
   ```bash
   git clone https://github.com/Nhoxboon/Pro_4.git
   cd Pro_4
   ```

2. **Open in Unity**
   - Launch Unity Hub
   - Click "Open" and select the project folder
   - Wait for Unity to import all assets

3. **Play the Game**
   - Open `Assets/_Scenes/MainScene.unity`
   - Click the Play button in Unity Editor
   - Use mouse to select and place towers
   - Defend against incoming enemy waves

### Controls
- **Left Click**: Select build slot and place towers
- **Number Keys (1-7)**: Quick select tower types
- **Right Click**: Cancel building action
- **Mouse Wheel**: Rotate tower preview (where applicable)

## 🎯 Gameplay Mechanics

### Tower Placement
1. Click on available build slots (highlighted areas)
2. Select desired tower type from the UI
3. Confirm placement to build the tower
4. Towers automatically target and attack enemies

### Resource Management
- Start each level with initial currency
- Earn money by defeating enemies
- Spend currency on towers and upgrades
- Manage resources strategically for optimal defense

### Enemy Waves
- Enemies spawn in timed waves
- Multiple enemy types with different abilities
- Boss enemies appear in later waves
- Enemies follow predefined paths to the player's base

### Win/Lose Conditions
- **Victory**: Survive all enemy waves
- **Defeat**: Player's castle health reaches zero

## 🔧 Development Features

### Debug Tools
- Comprehensive logging system via `DebugTool`
- Component auto-loading validation
- Real-time performance monitoring

### Extensibility
- Modular component system for easy feature addition
- ScriptableObject-based configuration
- Event-driven architecture for loose coupling

### Editor Tools
- Custom inspectors for game configuration
- Tower unlock configuration editor
- Level setup utilities

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is developed for educational and portfolio purposes.

## 👨‍💻 Developer

**Nhoxboon** - Game Developer

## 🙏 Acknowledgments

- Unity Technologies for the game engine
- CFXR for particle effects system
- Community contributors and testers

---

*Built with Unity and passion for game development* 🎮