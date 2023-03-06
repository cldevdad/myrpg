using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MyRpg.Engine.Utilities;

namespace MyRpg.Engine;

/// <summary>
/// The entry point for a game. Handles setting up graphic and content managers and
/// registering dependencies.
/// </summary>
public class Game : Microsoft.Xna.Framework.Game
{
    /// <summary>
    /// The game's dependencies.
    /// </summary>
    internal DependencyContainer Dependencies { get; set; }

    /// <summary>
    /// The games graphics device manager.
    /// </summary>
    private readonly GraphicsDeviceManager _graphicsDeviceManager;

    /// <summary>
    /// The game's sprite batch that is rendered every frame.
    /// </summary>
    protected SpriteBatch? SpriteBatch { get; set; }

    /// <summary>
    /// Initializes and returns a new instance of the Game class.
    /// </summary>
    public Game()
    {
        Dependencies = new DependencyContainer();

        _graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    /// <summary>
    /// Initializes the game, creating and registering all graphic and content dependencies.
    /// </summary>
    protected override void Initialize()
    {
        _graphicsDeviceManager.IsFullScreen = false;
        _graphicsDeviceManager.PreferMultiSampling = true;
        _graphicsDeviceManager.PreferredBackBufferWidth = 1280;
        _graphicsDeviceManager.PreferredBackBufferHeight = 720;
        _graphicsDeviceManager.SynchronizeWithVerticalRetrace = true;
        _graphicsDeviceManager.ApplyChanges();

        Dependencies.Register<GraphicsDevice>(GraphicsDevice);
        Dependencies.Register<GraphicsDeviceManager>(_graphicsDeviceManager);

        this.SpriteBatch = new SpriteBatch(GraphicsDevice);
        Dependencies.Register<SpriteBatch>(this.SpriteBatch);

        Dependencies.Register<ContentManager>(Content);

        base.Initialize();
    }

    /// <summary>
    /// Load content resources required by the game.
    /// </summary>
    protected override void LoadContent()
    {
        Dependencies.Register<ContentManager>(Content);
        base.LoadContent();
    }
}
