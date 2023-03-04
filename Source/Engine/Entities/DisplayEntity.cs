using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyRpg.Api;
using MyRpg.Engine.Components;
using MyRpg.Engine.Exceptions;

namespace MyRpg.Engine.Entities;

/// <summary>
/// An entity that can be displayed on the screen.
/// </summary>
public abstract class DisplayEntity : BaseEntity, IDrawableEntity, IUpdatableEntity, IContentEntity
{
    /// <inheritdoc />
    public override EntityType Type
    {
        get => EntityType.DISPLAY;
    }

    /// <summary>
    /// The value of the entity's position component.
    /// </summary>
    public Vector2 Position
    {
        get => Components.Find(c => c.Id == "position")?.GetValue<Vector2>() ?? Vector2.Zero;
        set => Components.Find(c => c.Id == "position")?.SetValue<Vector2>(value);
    }

    /// <summary>
    /// The value of the entity's texture component.
    /// </summary>
    public Texture2D Texture
    {
        get =>
            Components.Find(c => c.Id == CurrentTextureId)?.GetValue<Texture2D>()
            ?? throw new ComponentNotFoundException($"Texture component not found with id: {CurrentTextureId}");
    }

    /// <summary>
    /// The ID of the currently displayed texture.
    /// </summary>
    public string CurrentTextureId { get; private set; } = string.Empty;

    /// <summary>
    /// Add a texture to the entity.
    /// </summary>
    /// <param name="textureId">Texture ID.</param>
    public void AddTexture(string textureId)
    {
        _textureIds.Add(textureId);
    }

    /// <summary>
    /// Add a texture to the entity and set it as the currently displayed texture.
    /// </summary>
    /// <param name="textureId">Texture ID.</param>
    public void AddTextureAndSet(string textureId)
    {
        AddTexture(textureId);
        SetTexture(textureId);
    }

    /// <summary>
    /// Add a group of textures to the entity.
    /// </summary>
    /// <param name="textureIds">Texture IDs.</param>
    public void AddTextures(List<string> textureIds)
    {
        _textureIds.AddRange(textureIds);
    }

    /// <summary>
    /// Add a group of textures to the entity and set one as the currently displayed texture.
    /// </summary>
    /// <param name="textureIds">Texture IDs.</param>
    public void AddTexturesAndSet(List<string> textureIds, string idToSet)
    {
        AddTextures(textureIds);
        SetTexture(idToSet);
    }

    /// <summary>
    /// Set the currently displayed texture.
    /// </summary>
    /// <param name="textureId">Texture ID.</param>
    public void SetTexture(string textureId)
    {
        CurrentTextureId = textureId;
    }

    /// <summary>
    /// Initialize and return a new Display Entity.
    /// </summary>
    /// <param name="position">Optional position of the entity.</param>
    protected DisplayEntity(string id, Vector2 position = default(Vector2))
        : base(id)
    {
        this.Components.Add(new Component("position", typeof(Vector2), position));
    }

    /// <inheritdoc />
    public Rectangle GetBoundingRect()
    {
        var position = Position;
        var texture = Texture;
        return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
    }

    /// <inheritdoc />
    public virtual void LoadContent(ContentManager contentManager, GraphicsDevice? graphicsDevice = null)
    {
        foreach (var textureId in _textureIds)
        {
            Components.Add(new Component(textureId, typeof(Texture2D), contentManager.Load<Texture2D>(textureId)));
        }
    }

    /// <inheritdoc />
    public virtual void Draw(SpriteBatch spriteBatch, Matrix? transformMatrix = null)
    {
        var texture = Texture;

        spriteBatch.Draw(
            texture,
            Position,
            null,
            Color.White,
            0f,
            new Vector2((texture?.Width / 2) ?? 0, (texture?.Height / 2) ?? 0),
            Vector2.One,
            SpriteEffects.None,
            0f
        );
    }

    /// <inheritdoc />
    public virtual void Update(GameTime gameTime, KeyboardState? keyboardState = null) { }

    /// <summary>
    /// IDs of textures added to the entity.
    /// </summary>
    private readonly List<string> _textureIds = new List<string>();
}
