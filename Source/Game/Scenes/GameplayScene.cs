using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MyRpg.Engine.Scenes;
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
        Entities.Add("player", new Hero("player"));
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

        Map map = new Map(mapId);
        map.LoadContent(this.ContentManager, this.GraphicsDevice);

        var spawnTrigger = map.TiledMap
            .GetLayer<TiledMapObjectLayer>("Triggers")
            ?.Objects.First(o => o.Properties["type"] == "spawn" && o.Properties["name"] == spawnPoint);

        if (spawnTrigger == null)
        {
            return;
        }

        Entities["map"] = map;
        SpawnPlayer(spawnTrigger.Position);
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

        // Update the camera's X position if it's not at the left or right edge of the screen
        if (playerPosition.X > halfScreenWidth && playerPosition.X < mapSizeInPixels.Width - halfScreenWidth)
        {
            cameraPosition.X = playerPosition.X;
        }

        // Update the camera's Y position if it's not at the left or right edge of the screen
        if (playerPosition.Y > halfScreenHeight && playerPosition.Y < mapSizeInPixels.Height - halfScreenHeight)
        {
            cameraPosition.Y = playerPosition.Y;
        }

        // Set the new camera position
        Camera.Position = cameraPosition;
    }
}
