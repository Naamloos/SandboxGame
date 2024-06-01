# SandboxGame
Simple-ish sandbox game I am developing. Currently in very early experimental pre-alpha whatever

<img src="https://github.com/Naamloos/SandboxGame/assets/12187179/e2af17b8-a0e2-4499-ae5e-601752801b0a" width="400"/>
<img src="https://github.com/Naamloos/SandboxGame/assets/12187179/015470c6-9fc3-419d-b46b-8c6b417d6f86" width="400"/>


<sup><sub>oh no makriplier do'tn drown!!1</sub></sup>

## Milestones
- [ ] Properly finish up World Gen, and make the world feel alive
- [ ] Prettier visuals / Pixel art
- [ ] NPCs that actually interact with the world and can move around (aka AI)
- [ ] Inventory system, crafting, building
- [ ] Proper progression system
- [ ] Combat against NPCs
- [ ] Multiplayer (with all aforementioned features included)
- [ ] Proper modding support
- [ ] Lore!!!!! :D
Feel free to open an issue if you have cool ideas for this, I would love to hear them!

## Contributing
Contributions are welcome, but do be wary that this repository is licensed under the [AGPL](https://github.com/Naamloos/SandboxGame/blob/master/LICENSE) license.

Contribution guidelines are not available yet, but they will be at some point. Though if you have any cool ideas and can't wait for me to implement them, or want to fix a bug or issue, go ahead and open a PR!

## Some basics about the game's code
Most of this game is built with dependency injection in mind. In the future, this may be helpful for when modding support gets added. Mods will be able to interact with other parts of the game without clashing too much with the base game. I am not sure yet how mods will be able to change _the base game itself_ but I'll find a way, lol

As of right now, the codebase is quite extendable and I intend to keep it so. Here's some examples that are either already implemented or are planned to be:

- Scenes and Entities can gather all they need without having to modify the game's base code.
- The game contains abstractions for FileSystem handlers and Serializers.
- The game's many helpers and managers are only loosely dependent of each other through dependency injection.
- I intend to add interactable objects in the world as "entities", which are managed by the world they are spawned in.
- Key binds are configurable via a config file, and at some point the ability to add modded keybinds is to be added.
- Assets are loaded from assembly, meaning mods can easily embed their own resources if needed.

## Attributions
Uses font m6x11 by Daniel Linssen [ [Link](https://managore.itch.io/m6x11) ]

Uses sprite sheet by Stealthix [ [Link](https://stealthix.itch.io/rpg-nature-tileset) ]

Uses ambient music by prismlightdesigns [ [Link](https://prismlightdesigns.itch.io/syntheticambiencevol1) ]
