using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyRpg.Api;

namespace MyRpg.Engine.Scenes;

/// <summary>
/// A presentation of a state of game entities. Common uses are game states and menus.
/// </summary>
internal abstract class GameScene : IScene
{
    /// <inheritdoc />
    public string Id { get; set; } = string.Empty;

    /// <inheritdoc />
    public bool Active { get; set; } = false;

    /// <inheritdoc />
    public Dictionary<string, IEntity> Entities { get; set; } = new Dictionary<string, IEntity>();

    /// <summary>
    /// The game's Microsoft.Xna.Framework.Content.ContentManager dependency.
    /// </summary>
    protected ContentManager ContentManager { get; set; }

    /// <summary>
    /// The game's Microsoft.Xna.Framework.Graphics.GraphicsDevice dependency.
    /// </summary>
    protected GraphicsDevice GraphicsDevice { get; set; }

    /// <summary>
    /// The game's Microsoft.Xna.Framework.Graphics.GraphicsDeviceManager dependency.
    /// </summary>
    protected GraphicsDeviceManager GraphicsDeviceManager { get; set; }

    /// <summary>
    /// The game's Microsoft.Xna.Framework.Graphics.SpriteBatch dependency.
    /// </summary>
    protected SpriteBatch SpriteBatch { get; set; }

    /// <summary>
    /// The width of the scene in pixels.
    /// </summary>
    protected int Width => this.GraphicsDeviceManager.PreferredBackBufferWidth;

    /// <summary>
    /// The height of the scene in pixels.
    /// </summary>
    protected int Height => this.GraphicsDeviceManager.PreferredBackBufferHeight;

    /// <summary>
    /// Initializes a new scene and resolves any dependencies the scene needs.
    /// </summary>
    /// <param name="Id">Unique identifier for the scene.</param>
    protected GameScene(string? id = null)
    {
        Id = id ?? string.Empty;
        this.ContentManager = RpgGame.Instance.Dependencies.Resolve<ContentManager>();
        this.GraphicsDevice = RpgGame.Instance.Dependencies.Resolve<GraphicsDevice>();
        this.GraphicsDeviceManager = RpgGame.Instance.Dependencies.Resolve<GraphicsDeviceManager>();
        this.SpriteBatch = RpgGame.Instance.Dependencies.Resolve<SpriteBatch>();
    }

    /// <inheritdoc />
    public abstract void Initialize();

    /// <inheritdoc />
    public virtual void LoadContent()
    {
        foreach (var entity in Entities.Where(e => e.Value.GetType().IsAssignableTo(typeof(IContentEntity))))
        {
            (entity.Value as IContentEntity)?.LoadContent(this.ContentManager, this.GraphicsDevice);
        }
    }

    /// <inheritdoc />
    public virtual void Draw(Matrix? transformMatrix = null)
    {
        this.SpriteBatch.Begin();
        if (Active)
        {
            foreach (var entity in Entities.Where(e => e.Value.GetType().IsAssignableTo(typeof(IDrawableEntity))))
            {
                (entity.Value as IDrawableEntity)?.Draw(this.SpriteBatch, transformMatrix);
            }
        }
        this.SpriteBatch.End();
    }

    /// <inheritdoc />
    public virtual void Update(GameTime gameTime)
    {
        if (Active)
        {
            var keyboardState = Keyboard.GetState();

            foreach (var entity in Entities.Where(e => e.Value.GetType().IsAssignableTo(typeof(IUpdatableEntity))))
            {
                (entity.Value as IUpdatableEntity)?.Update(gameTime, keyboardState);
            }
        }
    }

    /// <inheritdoc />
    public virtual void UnloadContent()
    {
        this.ContentManager.Unload();
    }
}
