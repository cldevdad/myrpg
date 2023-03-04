using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MyRpg.Engine.Components;
using MyRpg.Engine.Entities;
using MyRpg.Exceptions;

namespace MyRpg.Entities;

/// <summary>
/// A map entity containing a tiled map.
/// </summary>
public class Map : DisplayEntity
{
    /// <summary>
    /// The tiled map data.
    /// </summary>
    public TiledMap TiledMap
    {
        get => Components.Find(c => c.Id == "tiledMap")?.GetValue<TiledMap>() ?? throw new MapNotFoundException();
    }

    /// <summary>
    /// Initializes and returns a new instance of the Map class.
    /// </summary>
    /// <param name="id">Unique identifier for the entity.</param>
    /// <param name="position">Screen position of the entity.</param>
    public Map(string id, Vector2 position = default(Vector2))
        : base(id, position) { }

    /// <inheritdoc />
    public override void LoadContent(ContentManager contentManager, GraphicsDevice? graphicsDevice = null)
    {
        var tiledMap = contentManager.Load<TiledMap>(Id);
        Components.Add(new Component("tiledMap", typeof(TiledMap), tiledMap));
        Components.Add(
            new Component("renderer", typeof(TiledMapRenderer), new TiledMapRenderer(graphicsDevice, tiledMap))
        );
    }

    /// <inheritdoc />
    public override void Draw(SpriteBatch spriteBatch, Matrix? transformMatrix = null)
    {
        Components.Find(c => c.Id == "renderer")?.GetValue<TiledMapRenderer>().Draw();
    }

    /// <inheritdoc />
    public override void Update(GameTime gameTime, KeyboardState? keyboardState = null)
    {
        var tiledMapRenderer = Components.Find(c => c.Id == "renderer")?.GetValue<TiledMapRenderer>();
        tiledMapRenderer?.Update(gameTime);
    }
}
