using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyRpg.Api;
using MyRpg.Logging;
using MyRpg.Scenes;

namespace MyRpg;

/// <summary>
/// Main RPG game class.
/// </summary>
internal sealed class RpgGame : MyRpg.Engine.Game
{
    /// <summary>
    /// Returns the static instance of the RpgGame class.
    /// </summary>
    public static RpgGame Instance
    {
        get { return _instance.Value; }
    }

    /// <summary>
    /// Add a scene to the game.
    /// </summary>
    /// <param name="scene">The scene to add.</param>
    /// <param name="active">Whether the scene should initially be active.</param>
    internal void AddScene(IScene scene, bool active = false)
    {
        scene.Initialize();
        scene.LoadContent();
        scene.Active = active;
        _scenes.Add(scene.Id, scene);
    }

    /// <summary>
    /// Toggle a scene's active status.
    /// </summary>
    /// <param name="sceneId">Scene ID.</param>
    /// <param name="active">Whether or not to activate the scene.</param>
    internal void ToggleSceneActive(string sceneId, bool active)
    {
        _scenes[sceneId].Active = active;
    }

    /// <summary>
    /// Remove a scene from the game.
    /// </summary>
    /// <param name="sceneId">Scene ID.</param>
    internal void RemoveScene(string sceneId)
    {
        _scenes[sceneId].UnloadContent();
        _scenes.Remove(sceneId);

        if (_scenes.Count == 0)
        {
            Exit();
        }
    }

    /// <inheritdoc />
    protected override void Initialize()
    {
        Dependencies.Register<ILogger>(new ConsoleLogger());
        Dependencies.Resolve<ILogger>().Log("Hello from RpgGame!");

        _scenes = new Dictionary<string, IScene>();

        base.Initialize();
    }

    /// <inheritdoc />
    protected override void LoadContent()
    {
        AddScene(new MainMenuScene("main-menu"), active: true);
        AddScene(new DebugOverlayScene("debug-overlay"), active: false);

        base.LoadContent();
    }

    /// <inheritdoc />
    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.OemTilde))
        {
            _scenes["debug-overlay"].Active = !_scenes["debug-overlay"].Active;
        }

        ActiveScenes().ForEach(scene => scene.Update(gameTime));

        base.Update(gameTime);
    }

    /// <inheritdoc />
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        if (this.SpriteBatch == null)
        {
            base.Draw(gameTime);
            return;
        }

        ActiveScenes().ForEach(scene => scene.Draw());
    }

    /// <inheritdoc />
    protected override void UnloadContent()
    {
        foreach (var scene in _scenes)
        {
            RemoveScene(scene.Key);
        }

        base.UnloadContent();
    }

    /// <summary>
    /// Initializes and returns a new instance of the RpgGame class.
    /// </summary>
    private RpgGame() { }

    /// <summary>
    /// Returns a list of all scenes that are currently active.
    /// </summary>
    /// <returns>A list of scenes that are currently active.</returns>
    private List<IScene> ActiveScenes()
    {
        return _scenes.Where(scene => scene.Value.Active).Select(s => s.Value).ToList();
    }

    /// <summary>
    /// A dictionary of scenes added to the game.
    /// </summary>
    private Dictionary<string, IScene> _scenes = new Dictionary<string, IScene>();

    /// <summary>
    /// The static instance of the RpgGame class.
    /// </summary>
    private static readonly Lazy<RpgGame> _instance = new Lazy<RpgGame>(() => new RpgGame());
}
