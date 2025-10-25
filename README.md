# LunarCodex
Cryptic game with some features

## Features

Following are the features included in the game.

### Dynamic Level System:

* Uses ScriptableObjects to define number of levels.

* Each level can have a unique grid size (e.g., 2x3, 4x4, 5x6).

* The level menu is procedurally populated from the LevelData asset.

### Responsive Grid Layout:

* The GridManager automatically calculates card positions and scales them to fit any screen resolution or aspect ratio, complete with customizable padding.

### Scoring & Combo System:

* Players earn a base score for each match.

* Earn bonus points by making consecutive matches (combos).

### Save & Load Progress:

* The game automatically saves the player's highest score, star rating, and stats for each completed level.

* All progress is saved to a local saveData.json file on a button click and retrieved back when loading the game.

* Players can load their progress from the main menu or clear their save data at runtime.

### Performance-Optimized:

* Uses an Object Pooling system for the cards. Instead of destroying and instantiating new cards (which causes garbage collection spikes), cards are recycled, ensuring smooth performance.

### Animations:

* Uses procedural, smooth animations for all card interactions:

* Smooth 180-degree flip on-tap.

* "Scale up and shrink" effect on a successful match.

* "Shake" effect on a mismatch.

### Audio Management:

* A central AudioManager handles all sound effects, such as card taps, matches, mismatches, and level completion.

### Star Rating System:

* At the end of each level, players are awarded a star rating (0-3 stars) based on their score.

* Star ratings are saved and displayed in the level selection menu.

### Android and Windows Support:

* Build supports and optimized for both Windows and Android platforms. Both the build files are included in this repo.

## Core Technical Architecture

Singleton Managers: The project is built around a series of singleton managers (LevelManager, CardManager,GridManager, AudioManager, SaveLoadManager, etc.) to handle distinct parts of the game logic.

ScriptableObject Data: Level data is decoupled from the scene logic using ScriptableObjects, making it easy for designers to add or edit levels without touching the code.
