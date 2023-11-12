using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MyRpg.Engine.Components;
using MyRpg.Engine.Entities;
using MyRpg.Engine.Utilities;
using MyRpg.Exceptions;
using MyRpg.Properties;

namespace MyRpg.Entities;

/// <summary>
/// A map entity containing a tiled map.
/// </summary>
public class Map : DisplayEntity
{
    /// <summary>
    /// Gets the tiled map data.
    /// </summary>
    public TiledMap TiledMap
    {
        get => Components.Find(c => c.Id == "tiledMap")?.GetValue<TiledMap>() ?? throw new MapNotFoundException();
    }

    /// <summary>
    /// Gets properties for the map.
    /// </summary>
    public MapProperties MapProperties
    {
        get
        {
            var mapProperties = new MapProperties();
            mapProperties.TileSize = new Size(TiledMap.TileWidth, TiledMap.TileHeight);
            mapProperties.SizeInTiles = new Size(TiledMap.Width, TiledMap.Height);
            mapProperties.SizeInPixels = mapProperties.TileSize * mapProperties.SizeInTiles;
            return mapProperties;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Map"/> class.
    /// </summary>
    /// <param name="id">Unique identifier for the entity.</param>
    /// <param name="position">Screen position of the entity.</param>
    public Map(string id, Vector2 position = default(Vector2))
        : base(id, position)
    {
        ContentRoot = "Maps";
        Layer = Enums.DrawLayer.MAP;
        CustomRenderer = true;
    }

    /// <inheritdoc />
    public override void LoadContent(ContentManager contentManager, GraphicsDevice? graphicsDevice = null)
    {
        var tiledMap = contentManager.Load<TiledMap>($"{ContentRoot}/{Id}");
        Components.Add(new Component("tiledMap", typeof(TiledMap), tiledMap));
        Components.Add(
            new Component("renderer", typeof(TiledMapRenderer), new TiledMapRenderer(graphicsDevice, tiledMap))
        );
    }

    /// <summary>
    /// Draws a specific tile layer.
    /// </summary>
    /// <param name="layerIndex">The index of the layer to draw.</param>
    /// <param name="transformMatrix">The transform matrix to apply to the layer.</param>
    public void DrawLayer(int layerIndex, Matrix? transformMatrix = null)
    {
        var tiledMapRenderer = Components.Find(c => c.Id == "renderer")?.GetValue<TiledMapRenderer>();
        if (tiledMapRenderer == null || layerIndex < 0 || layerIndex >= TiledMap.Layers.Count)
            return;

        // Draw the map with only the selected layer visible
        TiledMap.Layers[layerIndex].IsVisible = true;
        tiledMapRenderer.Draw(transformMatrix, depth: layerIndex);
        TiledMap.Layers[layerIndex].IsVisible = false;
    }

    /// <inheritdoc />
    public override void Update(GameTime gameTime, KeyboardState? keyboardState = null)
    {
        var tiledMapRenderer = Components.Find(c => c.Id == "renderer")?.GetValue<TiledMapRenderer>();
        tiledMapRenderer?.Update(gameTime);
    }
}
