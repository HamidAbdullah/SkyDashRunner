# 🏃 Sky Dash Runner - 3D Endless Runner Game

A highly addictive and visually appealing 3D endless-runner platformer game built in Unity. Run across procedurally generated floating islands in the sky, avoid obstacles, collect power-ups, and achieve the highest score!

## 🎮 Game Features

### Core Gameplay
- **Automatic Forward Movement**: Player runs forward automatically
- **Smooth Controls**: Swipe/keyboard/mouse controls for left-right movement, jump, slide, and dash
- **Procedural Level Generation**: Endless path with randomly generated floating islands
- **Progressive Difficulty**: Game speed and obstacle density increase over time

### Player Mechanics
- **Movement**: Left/Right lane switching
- **Jump**: Single and double jump
- **Slide**: Slide under obstacles
- **Dash**: Mid-air dash ability
- **Animations**: Idle, run, jump, slide, dash, fall animations

### Obstacles & Challenges
- **Moving Platforms**: Platforms that move horizontally
- **Breaking Tiles**: Tiles that break when stepped on
- **Rotating Blades**: Deadly rotating obstacles
- **Jump Pads**: Boost platforms for extra height
- **Wind Zones**: Areas that push the player
- **Gaps**: Jump across gaps between islands

### Power-Ups (5 Types)
1. **Magnet**: Pulls coins and collectibles toward the player
2. **Hyper Dash**: Super speed + invincibility
3. **Double Jump**: Allows an extra jump in mid-air
4. **Slow Motion**: Slows down time for easier navigation
5. **Shield**: Protects from one deadly obstacle hit

### Collectibles
- **Coins**: Basic currency (1 coin each)
- **Orbs**: High-value collectibles (2 coins, 20 points each)

### Progression Systems
- **Scoring System**: Points for distance, collectibles, and survival
- **Daily Missions**: 3 daily missions with rewards
- **Shop System**: Unlockable skins, particle trails, and power-up upgrades
- **Leaderboard**: Local and online leaderboard support
- **Coin Economy**: Earn and spend coins on upgrades

### Audio & Visuals
- **Background Music**: Fast-paced game music
- **Sound Effects**: Jump, slide, collect, power-up, game over sounds
- **Score Popups**: Floating score animations
- **Camera Shake**: Impact feedback
- **Particle Effects**: Visual feedback for actions

## 📁 Project Structure

```
SkyDashRunner/
├── Assets/
│   └── Scripts/
│       ├── Player/
│       │   └── PlayerController.cs
│       ├── Managers/
│       │   ├── GameManager.cs
│       │   ├── UIManager.cs
│       │   ├── AudioManager.cs
│       │   ├── CameraController.cs
│       │   ├── MissionSystem.cs
│       │   ├── ShopSystem.cs
│       │   └── LeaderboardSystem.cs
│       ├── LevelGeneration/
│       │   ├── ProceduralLevelGenerator.cs
│       │   └── IslandController.cs
│       ├── PowerUps/
│       │   ├── PowerUp.cs (Base class)
│       │   ├── MagnetPowerUp.cs
│       │   ├── HyperDashPowerUp.cs
│       │   ├── DoubleJumpPowerUp.cs
│       │   ├── SlowMotionPowerUp.cs
│       │   └── ShieldPowerUp.cs
│       ├── Obstacles/
│       │   ├── Obstacle.cs (Base class)
│       │   ├── MovingPlatform.cs
│       │   ├── BreakingTile.cs
│       │   ├── RotatingBlade.cs
│       │   ├── JumpPad.cs
│       │   └── WindZone.cs
│       └── Collectibles/
│           ├── Collectible.cs (Base class)
│           ├── Coin.cs
│           └── Orb.cs
└── README.md
```

## 🚀 Setup Instructions

### Prerequisites
- Unity 2021.3 LTS or later
- TextMeshPro (included in Unity)
- Input System package (Window > Package Manager > Input System)

### Step 1: Create Unity Project
1. Open Unity Hub
2. Create a new 3D project
3. Name it "SkyDashRunner"
4. Set the project location

### Step 2: Import Required Packages
1. Open **Window > Package Manager**
2. Install **TextMeshPro** (if not already installed)
3. Install **Input System** package:
   - Click **+** > **Add package by name**
   - Enter: `com.unity.inputsystem`
   - Click **Add**

### Step 3: Setup Project Structure
1. Copy all scripts from `Assets/Scripts/` to your Unity project
2. Create the folder structure as shown above if it doesn't exist

### Step 4: Create Game Scene
1. Create a new scene: **File > New Scene > 3D**
2. Save as "GameScene"

### Step 5: Setup Player
1. Create an empty GameObject named "Player"
2. Add **CharacterController** component
3. Add **Animator** component
4. Add **PlayerController** script
5. Create a child GameObject for the player model (or use a capsule as placeholder)
6. Set up ground check:
   - Create empty child GameObject "GroundCheck"
   - Position it at player's feet
   - Assign to PlayerController's GroundCheck field

### Step 6: Setup Input System
1. Create Input Actions: **Window > Analysis > Input Debugger**
2. Or create an Input Action Asset:
   - Right-click in Project > **Create > Input Actions**
   - Name it "PlayerInput"
   - Set up actions:
     - **Move** (Vector2) - for left/right movement
     - **Jump** (Button) - for jumping
     - **Slide** (Button) - for sliding
     - **Dash** (Button) - for dashing
3. Add **Player Input** component to Player
4. Assign the Input Action Asset

### Step 7: Setup Camera
1. Create empty GameObject "CameraController"
2. Add **CameraController** script
3. Assign Player transform to Target field
4. Adjust Offset and Smooth Speed as needed

### Step 8: Setup Managers
1. Create empty GameObject "GameManager"
   - Add **GameManager** script
   - Assign Player reference
2. Create empty GameObject "UIManager"
   - Add **UIManager** script
   - Create UI Canvas (Right-click > UI > Canvas)
   - Setup UI panels (Main Menu, Game UI, Game Over, etc.)
3. Create empty GameObject "AudioManager"
   - Add **AudioManager** script
   - Assign music and SFX clips
4. Create empty GameObject "MissionSystem"
   - Add **MissionSystem** script
5. Create empty GameObject "ShopSystem"
   - Add **ShopSystem** script
6. Create empty GameObject "LeaderboardSystem"
   - Add **LeaderboardSystem** script

### Step 9: Setup Level Generation
1. Create empty GameObject "LevelGenerator"
   - Add **ProceduralLevelGenerator** script
   - Assign Player transform
   - Create Island prefabs (3D models or simple cubes)
   - Assign island prefabs array

### Step 10: Create Island Prefabs
1. Create a basic island:
   - Create a Cube or use a 3D model
   - Scale to desired island size
   - Add **IslandController** script
   - Create child GameObjects for spawn points:
     - "ObstacleSpawnPoints" (empty parent)
     - "CollectibleSpawnPoints" (empty parent)
     - "PowerUpSpawnPoints" (empty parent)
   - Create child transforms for each spawn point
2. Make it a prefab: Drag to Project window

### Step 11: Create Obstacle Prefabs
1. For each obstacle type:
   - Create GameObject (Cube, Sphere, etc.)
   - Add appropriate obstacle script
   - Add Collider (set as Trigger)
   - Add Tag "Obstacle"
   - Make prefab
2. Obstacle types:
   - **RotatingBlade**: Add RotatingBlade script
   - **BreakingTile**: Add BreakingTile script
   - **MovingPlatform**: Add MovingPlatform script
   - **JumpPad**: Add JumpPad script
   - **WindZone**: Add WindZone script

### Step 12: Create Collectible Prefabs
1. Create Coin:
   - Create Sphere or coin model
   - Add **Coin** script
   - Add Collider (Trigger)
   - Add Tag "Collectible"
   - Make prefab
2. Create Orb:
   - Similar to Coin but add **Orb** script

### Step 13: Create Power-Up Prefabs
1. For each power-up:
   - Create GameObject (Sphere, etc.)
   - Add appropriate power-up script
   - Add Collider (Trigger)
   - Add Tag "PowerUp"
   - Add Particle System for visual effect
   - Make prefab

### Step 14: Setup UI
1. Create Canvas (if not created)
2. Create panels:
   - MainMenuPanel
   - GamePanel (with Score, Coins, Distance text)
   - PausePanel
   - GameOverPanel
   - ShopPanel
   - MissionsPanel
3. Assign panels to UIManager script
4. Setup buttons and text references

### Step 15: Setup Tags and Layers
1. Go to **Edit > Project Settings > Tags and Layers**
2. Create tags:
   - "Player"
   - "Obstacle"
   - "Collectible"
   - "PowerUp"
   - "Ground"
3. Create layer "Ground" for ground detection

### Step 16: Setup Ground
1. Create a large plane or cube as ground
2. Assign "Ground" layer
3. Add to Ground Mask in PlayerController

### Step 17: Configure Settings
1. **PlayerController**:
   - Set Forward Speed, Side Speed, Jump Force
   - Configure Ground Check
   - Assign particle effects
2. **GameManager**:
   - Set Game Speed Increase Rate
   - Assign references
3. **ProceduralLevelGenerator**:
   - Set Island Spacing
   - Assign Island Prefabs
   - Set Spawn Distance

### Step 18: Add Animations (Optional)
1. Create Animator Controller for Player
2. Add states: Idle, Run, Jump, Slide, Dash, Fall, Die
3. Create transitions
4. Assign to Player's Animator component

### Step 19: Add Audio
1. Import or create audio files:
   - Background music (MP3/WAV)
   - Jump, slide, dash, collect, game over sounds
2. Assign to AudioManager script

### Step 20: Test and Build
1. Press Play to test
2. Adjust settings as needed
3. Build for target platform:
   - **File > Build Settings**
   - Select platform (Android, iOS, PC)
   - Click **Build**

## 🎯 Controls

### PC (Keyboard/Mouse)
- **A/D** or **Left/Right Arrow**: Move left/right
- **Space**: Jump
- **S** or **Down Arrow**: Slide
- **Shift**: Dash
- **Mouse Movement**: Move left/right (alternative)

### Mobile (Touch)
- **Swipe Left/Right**: Move left/right
- **Tap**: Jump
- **Swipe Down**: Slide
- **Swipe Up**: Dash

## 🔧 Customization

### Adjust Difficulty
- Edit `GameManager.cs`: `gameSpeedIncreaseRate`, `maxGameSpeed`
- Edit `ProceduralLevelGenerator.cs`: `difficultyIncreaseRate`

### Add New Obstacles
1. Create new script inheriting from `Obstacle.cs`
2. Implement custom behavior
3. Create prefab and add to IslandController's obstacle prefabs

### Add New Power-Ups
1. Create new script inheriting from `PowerUp.cs`
2. Implement `ApplyPowerUp()` method
3. Create prefab and add to IslandController's power-up prefabs

### Customize Visuals
- Replace placeholder models with your 3D assets
- Adjust materials and colors
- Add particle effects
- Customize UI design

## 📱 Platform-Specific Notes

### Android
- Minimum API Level: 21 (Android 5.0)
- Target API Level: 30+
- Enable "Internet" permission for online leaderboard

### iOS
- Minimum iOS Version: 11.0
- Configure App Store settings
- Enable "Internet" capability for online leaderboard

### PC
- Supports keyboard and mouse
- Can use gamepad with Input System

## 🐛 Troubleshooting

### Player Not Moving
- Check CharacterController is attached
- Verify Input System is set up correctly
- Check PlayerController script is enabled

### Islands Not Spawning
- Verify ProceduralLevelGenerator has island prefabs assigned
- Check Player reference is set
- Ensure spawn distance is appropriate

### UI Not Showing
- Check Canvas is set to Screen Space - Overlay
- Verify UIManager has panel references
- Check GameManager instance exists

### Audio Not Playing
- Verify AudioManager has audio clips assigned
- Check AudioSource components exist
- Ensure volume is not muted

## 📝 Notes

- All scripts use namespaces for organization
- Singleton pattern used for managers
- PlayerPrefs used for local data storage
- Online leaderboard requires backend setup
- LeanTween recommended for UI animations (optional)

## 🎨 Asset Recommendations

### Free Assets
- **Synty Studios Polygon** series (low-poly models)
- **Kenney Assets** (game assets)
- **Unity Asset Store** free section

### Paid Assets
- **POLYGON** series for stylized graphics
- **Nature Starter Kit 2** for environment
- **Particle Effects** packs

## 📄 License

This project is provided as-is for educational and development purposes.

## 🤝 Contributing

Feel free to extend and modify this game for your needs!

---

**Enjoy creating your Sky Dash Runner game! 🚀**

