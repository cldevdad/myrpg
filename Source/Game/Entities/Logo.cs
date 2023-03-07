using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MyRpg.Engine.Entities;

namespace MyRpg.Entities;

/// <summary>
/// A logo image entity.
/// </summary>
public class Logo : DisplayEntity
{
    /// <summary>
    /// Initializes and returns a new instance of the Logo class.
    /// </summary>
    /// <param name="id">Unique identifier for the entity.</param>
    /// <param name="position">Screen position of the entity.</param>
    public Logo(string id, Vector2 position = default(Vector2))
        : base(id, position) { }

    /// <inheritdoc />
    public override void LoadContent(ContentManager contentManager, GraphicsDevice? graphicsDevice = null)
    {
        ContentRoot = "UI";

        AddTextureAndSet($"{ContentRoot}/MyRpgLogo");
        base.LoadContent(contentManager, graphicsDevice);
    }

    /// <inheritdoc />
    public override void Draw(SpriteBatch spriteBatch, Matrix? transformMatrix = null)
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
}
