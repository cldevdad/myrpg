# My RPG
A 2D RPG with MonoGame.

## Tools

 - Tiled: https://www.mapeditor.org/
  
## Designing Maps
  
Maps are made of `64px` x `64px` tiles. Tile images should be combined into a single spritesheet for each themed tileset. Import the spritesheet into Tiled to create a tileset resource. Create a Tiled Map and add Tile Layers to design a map.

## Adding Triggers to Maps

Add an Object Layer in Tiled named "Triggers". Add triggers of any shape to this layer that define triggers which the game responds to.

### Current Trigger Types

Spawn Point
- `type`: `"spawn"`
- `name`: spawn point name

Door
- `type`: `"door"`
- `targetMap`: target map name
- `targetSpawnPoint`: target spawn point name


