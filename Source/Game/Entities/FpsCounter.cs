using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyRpg.Engine.Components;
using MyRpg.Engine.Entities;
using MyRpg.Engine.Exceptions;
using MyRpg.Enums;

namespace MyRpg.Entities;

/// <summary>
/// A simple framerate counter.
/// </summary>
public class FpsCounter : DisplayEntity
{
    /// <summary>
    /// Gets or sets the font used to render the framerate.
    /// </summary>
    public SpriteFont Font
    {
        get =>
            Components.Find(c => c.Id == "font")?.GetValue<SpriteFont>()
            ?? throw new ComponentNotFoundException("SpriteFont component not found with id: font");
        set => Components.Find(c => c.Id == "font")?.SetValue<SpriteFont>(value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FpsCounter"/> class.
    /// </summary>
    /// <param name="id">Unique identifier for the entity.</param>
    /// <param name="position">Screen position of the entity.</param>
    public FpsCounter(string id, Vector2 position = default(Vector2))
        : base(id, position) { }

    /// <inheritdoc />
    public override void LoadContent(ContentManager contentManager, GraphicsDevice? graphicsDevice = null)
    {
        ContentRoot = "Fonts";
        Layer = DrawLayer.UI;

        Components.Add(
            new Component("font", typeof(SpriteFont), contentManager.Load<SpriteFont>($"{ContentRoot}/MyFont"))
        );
    }

    /// <inheritdoc />
    public override void Draw(SpriteBatch spriteBatch, Matrix? transformMatrix = null)
    {
        spriteBatch.DrawString(
            Font,
            $"FPS: {_fps}",
            Position,
            Color.White,
            0,
            Vector2.Zero,
            1.0f,
            SpriteEffects.None,
            0.5f
        );
    }

    /// <inheritdoc />
    public override void Update(GameTime gameTime, KeyboardState? keyboardState = null)
    {
        _fps = 1 / gameTime.ElapsedGameTime.TotalSeconds;
    }

    /// <summary>
    /// The most recent frames-per-second.
    /// </summary>
    private double _fps = 0d;
}
