using MyRpg.Engine.Utilities;

namespace MyRpg.Properties;

/// <summary>
/// Properties for a 2D tiled map.
/// </summary>
public class MapProperties
{
    /// <summary>
    /// Size of the map in pixels.
    /// </summary>
    public Size SizeInPixels { get; set; }

    /// <summary>
    /// Size of the map in tiles.
    /// </summary>
    public Size SizeInTiles { get; set; }

    /// <summary>
    /// Size of the map's individual tiles.
    /// </summary>
    public Size TileSize { get; set; }
}
