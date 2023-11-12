using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MyRpg.Engine.Scenes;
using MyRpg.Engine.Utilities;
using MyRpg.Entities;
using MyRpg.Exceptions;
using MyRpg.Logging;

namespace MyRpg.Scenes;

/// <summary>
/// The gameplay scene.
/// </summary>
internal class GameplayScene : CameraScene
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameplayScene"/> class.
    /// </summary>
    /// <param name="id">Unique identifier for the scene.</param>
    public GameplayScene(string? id = null)
        : base(id) { }

    /// <inheritdoc />
    public override void Initialize()
    {
        Entities.Add(new Hero("hero", new Size(64, 72)));
    }

    /// <inheritdoc />
    public override void LoadContent()
    {
        base.LoadContent();

        LoadMap("town", "house");
    }

    /// <inheritdoc />
    public override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            RpgGame.Instance.RemoveScene("gameplay");
            RpgGame.Instance.ToggleSceneActive("main-menu", true);
            return;
        }

        UpdateCamera();
        CheckMapTriggers();

        base.Update(gameTime);
    }

    public override void Draw(Matrix? transformMatrix = null)
    {
        // Draw the ground layers
        if (CurrentMap is not null)
        {
            SpriteBatch.Begin(transformMatrix: Camera?.Transform);
            CurrentMap.DrawLayer(0, Camera?.Transform);
            CurrentMap.DrawLayer(1, Camera?.Transform);
            SpriteBatch.End();

            base.Draw(Camera?.Transform);

            SpriteBatch.Begin(transformMatrix: Camera?.Transform);
            PlayerCharacter?.Draw(SpriteBatch, Camera?.Transform);
            SpriteBatch.End();

            SpriteBatch.Begin(transformMatrix: Camera?.Transform);

            // Draw the remaining layers above the hero
            for (int i = 2; i < CurrentMap.TiledMap.Layers.Count; i++)
            {
                CurrentMap.DrawLayer(i, Camera?.Transform);
            }

            SpriteBatch.End();
        }

        
    }

    /// <summary>
    /// Gets the player character entity.
    /// </summary>
    private Hero? PlayerCharacter => Entities.Find(e => e.GetType() == typeof(Hero)) as Hero;

    /// <summary>
    /// Gets the current map.
    /// </summary>
    private Map? CurrentMap => Entities.Find(e => e.GetType() == typeof(Map)) as Map;

    /// <summary>
    /// Gets the player character entity.
    /// </summary>
    private OverheadCamera? Camera => Entities.Find(e => e.GetType() == typeof(OverheadCamera)) as OverheadCamera;

    /// <summary>
    /// Spawns the player at a position.
    /// </summary>
    /// <param name="position">Position to spawn at.</param>
    private void SpawnPlayer(Vector2 position)
    {
        if (PlayerCharacter != null && Camera != null)
        {
            PlayerCharacter.Position = position;
            Camera.Position = position;
        }
    }

    /// <summary>
    /// Load a map as the current map.
    /// </summary>
    /// <param name="mapId">Map ID.</param>
    /// <exception cref="MapNotFoundException">Thrown if the map is not able to be loaded.</exception>
    private void LoadMap(string mapId, string spawnPoint)
    {
        RpgGame.Instance.Dependencies.Resolve<ILogger>().Log($"Loading map '{mapId}' at spawn point '{spawnPoint}'");

        // Load the map
        var existingMap = Entities.FindIndex(e => e.GetType() == typeof(Map));
        Map map = new Map(mapId);
        map.LoadContent(this.ContentManager, this.GraphicsDevice);

        // Remove the existing map, if any
        if (existingMap != -1)
        {
            Entities[existingMap] = map;
        }
        else
        {
            Entities.Add(map);
        }

        LoadMapObjects();

        // Find the spawn trigger
        var spawnTrigger = map.TiledMap
            .GetLayer<TiledMapObjectLayer>("Triggers")
            ?.Objects.First(o => o.Properties["type"] == "spawn" && o.Properties["name"] == spawnPoint);

        if (spawnTrigger == null)
        {
            return;
        }

        // Tile map objects position starts from bottom-right, so we need to spawn the
        // player a bit up and to the right
        var spawnPosition = new Vector2(
            spawnTrigger.Position.X + (PlayerCharacter?.GetBoundingRect().Width / 2) ?? 0,
            spawnTrigger.Position.Y - (PlayerCharacter?.GetBoundingRect().Height / 2) ?? 0
        );

        SpawnPlayer(spawnPosition);
    }

    /// <summary>
    /// Loads objects from the map's object's layer.
    /// </summary>
    private void LoadMapObjects()
    {
        var map = Entities.Find(entity => entity.GetType() == typeof(Map)) as Map ?? throw new MapNotFoundException();
        var mapObjectsLayer = map?.TiledMap.GetLayer<TiledMapObjectLayer>("Objects");

        // Remove existing campfires
        Entities = Entities.FindAll(entity => !entity.Id.StartsWith("campfire"));

        // Add new campfires
        var campfires = mapObjectsLayer?.Objects.Where(obj => obj.Name == "Campfire");
        var campfireIndex = 0;
        foreach (var pos in campfires?.Select(fireObject => fireObject.Position) ?? Enumerable.Empty<Vector2>())
        {
            var campfire = new Campfire(
                $"campfire_{campfireIndex}",
                new Size(64, 64),
                new Vector2(pos.X + 32, pos.Y + 32)
            );
            campfire.LoadContent(this.ContentManager);
            Entities.Add(campfire);
            campfireIndex++;
        }
    }

    /// <summary>
    /// Checks and responds to any triggers on the current map.
    /// </summary>
    /// <exception cref="MapNotFoundException">Thrown if the map is not the current map.</exception>
    private void CheckMapTriggers()
    {
        if (CurrentMap == null || PlayerCharacter == null)
        {
            return;
        }

        var triggers = CurrentMap.TiledMap.GetLayer<TiledMapObjectLayer>("Triggers");
        if (triggers == null)
        {
            return;
        }

        // Doors
        var doors = triggers.Objects.Where(o => o.Properties["type"] == "door");
        foreach (var door in doors)
        {
            var doorBounds = new Rectangle(
                (int)door.Position.X,
                (int)door.Position.Y,
                (int)door.Size.Width,
                (int)door.Size.Height
            );

            if (PlayerCharacter.GetBoundingRect().Intersects(doorBounds))
            {
                LoadMap(door.Properties["targetMap"], door.Properties["targetSpawnPoint"]);
            }
        }
    }

    /// <summary>
    /// Update the camera to focus on the player.
    /// </summary>
    private void UpdateCamera()
    {
        if (Camera == null || this.CurrentMap == null || PlayerCharacter == null)
        {
            return;
        }

        var cameraPosition = Camera.Position;
        var playerPosition = PlayerCharacter.Position;
        var mapSizeInPixels = this.CurrentMap.MapProperties.SizeInPixels;

        var halfScreenWidth = Width / Camera.Zoom / 2;
        var halfScreenHeight = Height / Camera.Zoom / 2;

        var leftEdge = halfScreenWidth;
        var rightEdge = mapSizeInPixels.Width - halfScreenWidth;
        var topEdge = halfScreenHeight;
        var bottomEdge = mapSizeInPixels.Height - halfScreenHeight;

        // X position
        if (playerPosition.X > leftEdge && playerPosition.X < rightEdge)
        {
            cameraPosition.X = playerPosition.X;
        }
        else if (playerPosition.X < leftEdge)
        {
            cameraPosition.X = leftEdge;
        }
        else if (playerPosition.X > rightEdge)
        {
            cameraPosition.X = rightEdge;
        }

        // Y position
        if (playerPosition.Y > topEdge && playerPosition.Y < bottomEdge)
        {
            cameraPosition.Y = playerPosition.Y;
        }
        else if (playerPosition.Y < topEdge)
        {
            cameraPosition.Y = topEdge;
        }
        else if (playerPosition.Y > bottomEdge)
        {
            cameraPosition.Y = bottomEdge;
        }

        // Set the new camera position
        Camera.Position = cameraPosition;
    }
}
