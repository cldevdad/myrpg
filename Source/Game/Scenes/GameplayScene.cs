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
internal class GameplayScene : GameScene
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

        var camera = new Camera(this.GraphicsDevice.Viewport);
        camera.AdjustZoom(2f);
        Entities.Add("camera", camera);

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

        CheckMapTriggers();

        base.Update(gameTime);
    }

    public override void Draw(Matrix? transformMatrix = null)
    {
        base.Draw(Camera?.Transform);
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
    private Camera? Camera => Entities["camera"] as Camera;

    /// <summary>
    /// Spawns the player at a position.
    /// </summary>
    /// <param name="position">Position to spawn at.</param>
    private void SpawnPlayer(Vector2 position)
    {
        if (PlayerCharacter != null)
        {
            PlayerCharacter.Position = position;
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
}
