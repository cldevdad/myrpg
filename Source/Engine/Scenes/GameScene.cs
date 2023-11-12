using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyRpg.Api;

namespace MyRpg.Engine.Scenes;

/// <summary>
/// Base game scene object.
/// </summary>
internal abstract class GameScene : IScene
{
    /// <inheritdoc />
    public string Id { get; set; } = string.Empty;

    /// <inheritdoc />
    public bool Active { get; set; } = false;

    /// <inheritdoc />
    public List<IEntity> Entities { get; set; } = new List<IEntity>();

    /// <summary>
    /// Gets or sets the game's Microsoft.Xna.Framework.Content.ContentManager dependency.
    /// </summary>
    protected ContentManager ContentManager { get; set; }

    /// <summary>
    /// Gets or sets the game's Microsoft.Xna.Framework.Graphics.GraphicsDevice dependency.
    /// </summary>
    protected GraphicsDevice GraphicsDevice { get; set; }

    /// <summary>
    /// Gets or sets the game's Microsoft.Xna.Framework.Graphics.GraphicsDeviceManager dependency.
    /// </summary>
    protected GraphicsDeviceManager GraphicsDeviceManager { get; set; }

    /// <summary>
    /// Gets or sets the game's Microsoft.Xna.Framework.Graphics.SpriteBatch dependency.
    /// </summary>
    protected SpriteBatch SpriteBatch { get; set; }

    /// <summary>
    /// Gets the width of the scene in pixels.
    /// </summary>
    protected int Width => this.GraphicsDeviceManager.PreferredBackBufferWidth;

    /// <summary>
    /// Gets the height of the scene in pixels.
    /// </summary>
    protected int Height => this.GraphicsDeviceManager.PreferredBackBufferHeight;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameScene"/> class.
    /// </summary>
    /// <param name="id">Unique identifier for the scene.</param>
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
        foreach (var entity in Entities.Where(e => e.GetType().IsAssignableTo(typeof(IContentEntity))))
        {
            (entity as IContentEntity)?.LoadContent(this.ContentManager, this.GraphicsDevice);
        }
    }

    /// <inheritdoc />
    public virtual void Draw(Matrix? transformMatrix = null)
    {
        this.SpriteBatch.Begin(transformMatrix: transformMatrix);
        if (Active)
        {
            var drawableEntities = Entities
                .Where(e => e.GetType().IsAssignableTo(typeof(IDrawableEntity)))
                .Cast<IDrawableEntity>()
                .Where(e => !e.CustomRenderer)
                .OrderBy(e => e.Layer)
                .ToList();

            var orderedDrawableEntities = drawableEntities.OrderBy(e => e.Layer).ToList();
            if (orderedDrawableEntities != null)
            {
                orderedDrawableEntities = orderedDrawableEntities.OrderBy(e => e.Layer).ToList();
            }

            foreach (var entity in orderedDrawableEntities ?? Enumerable.Empty<IDrawableEntity>())
            {
                entity?.Draw(this.SpriteBatch, transformMatrix);
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

            foreach (var entity in Entities.Where(e => e.GetType().IsAssignableTo(typeof(IUpdatableEntity))))
            {
                (entity as IUpdatableEntity)?.Update(gameTime, keyboardState);
            }
        }
    }

    /// <inheritdoc />
    public virtual void UnloadContent()
    {
        this.ContentManager.Unload();
    }

    protected List<IDrawableEntity> GetOrderedDrawableEntities()
    {
        var drawableEntities = Entities
            .Where(e => e.GetType().IsAssignableTo(typeof(IDrawableEntity)))
            .Cast<IDrawableEntity>()
            .OrderBy(e => e.Layer)
            .ToList();

        foreach (var entity in Entities.Where(e => e.GetType().IsAssignableTo(typeof(IDrawableEntity))))
        {
            if (entity != null)
            {
                drawableEntities.Add(entity as IDrawableEntity ?? throw new InvalidCastException());
            }
        }

        var orderedEntities = drawableEntities.OrderBy(e => e.Layer).ToList();
        if (orderedEntities != null)
        {
            orderedEntities = orderedEntities.OrderBy(e => e.Layer).ToList();
        }

        return orderedEntities ?? new List<IDrawableEntity>();
    }
}
