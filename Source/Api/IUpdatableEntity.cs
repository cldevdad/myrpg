using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MyRpg.Api;

/// <summary>
/// Shape of an entity with updatable components.
/// </summary>
public interface IUpdatableEntity : IEntity
{
    /// <summary>
    /// Updates all updatable entities attached to the entity.
    /// </summary>
    /// <param name="gameTime">Elapsed time since the last update.</param>
    /// <param name="keyboardState">The current state of keystrokes on the keyboard.</param>
    void Update(GameTime gameTime, KeyboardState? keyboardState = null);
}
