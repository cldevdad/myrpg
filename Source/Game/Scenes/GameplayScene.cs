using System.Linq;
using Microsoft.Xna.Framework;
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
    /// Constructs and returns a new GameplayScene.
    /// </summary>
    /// <param name="id">Unique identifier for the scene.</param>
    public GameplayScene(string? id = null)
        : base(id) { }

    /// <inheritdoc />
    public override void Initialize()
    {
        Entities.Add("player", new Hero("hero", new Size(64, 72)));
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

    /// <summary>
    /// The player character entity.
    /// </summary>
    private Hero? PlayerCharacter => Entities["player"] as Hero;

    /// <summary>
    /// The current map.
    /// </summary>
    private Map? CurrentMap => Entities["map"] as Map;

    /// <summary>
    /// The player character entity.
    /// </summary>
    private OverheadCamera? Camera => Entities["camera"] as OverheadCamera;

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
        RpgGame.Instance.Dependencies.Resolve<ILogger>().Log($"Loading map {mapId} at spawn point {spawnPoint}");

        // Load the map
        Map map = new Map(mapId);
        map.LoadContent(this.ContentManager, this.GraphicsDevice);
        Entities["map"] = map;
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
        var map = Entities["map"] as Map;
        var mapObjectsLayer = map?.TiledMap.GetLayer<TiledMapObjectLayer>("Objects");

        // Remove existing campfires
        var nonCampfireEntities = from kvp in Entities where !kvp.Key.StartsWith("campfire") select kvp;
        Entities = nonCampfireEntities.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        // Add new campfires
        var campfires = mapObjectsLayer?.Objects.Where(obj => obj.Name == "Campfire");
        var campfireIndex = 0;
        foreach (var pos in campfires?.Select(fireObject => fireObject.Position) ?? Enumerable.Empty<Vector2>())
        {
            var campfire = new Campfire("campfire", new Size(64, 64), new Vector2(pos.X + 32, pos.Y + 32));
            campfire.LoadContent(this.ContentManager);

            Entities.Add($"campfire_{campfireIndex}", campfire);
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
