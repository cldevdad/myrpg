using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyRpg.Engine.Scenes;
using MyRpg.Entities;

namespace MyRpg.Scenes;

/// <summary>
/// The front end main menu scene.
/// </summary>
internal class MainMenuScene : GameScene
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainMenuScene"/> class.
    /// </summary>
    /// <param name="id">Unique identifier for the scene.</param>
    public MainMenuScene(string? id = null)
        : base(id) { }

    /// <inheritdoc />
    public override void Initialize()
    {
        Entities.Add(new Logo("logo", new Vector2(Width / 2, Height / 2)));
    }

    /// <inheritdoc />
    public override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.X))
        {
            RpgGame.Instance.RemoveScene("main-menu");
        }

        if (keyboardState.IsKeyDown(Keys.Space))
        {
            this.Active = false;
            RpgGame.Instance.AddScene(new GameplayScene("gameplay"));
            RpgGame.Instance.ToggleSceneActive("gameplay", true);
        }

        base.Update(gameTime);
    }
}
