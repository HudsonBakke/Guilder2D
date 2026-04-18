using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Guilder2D;

public class PlacedMapData
{
    public int TileWidth { get; set; }
    public int TileHeight { get; set; }
    public List<object> Tiles { get; set; } = [];
    public List<MapObjectData> Objects { get; set; } = [];
}

public class MapObjectData
{
    public RectangleData Position { get; set; } = new();
    public string TextureSource { get; set; } = "";
    public bool Collidable { get; set; }
}

public class RectangleData
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

/// <summary>
/// Used to load JSON data into a Map object
/// </summary>
public static class MapLoader
{
    /// <summary>
    /// Loads 
    /// </summary>
    /// <param name="content"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static PlacedMap LoadMap(ContentManager content, string path)
    {
        PlacedMap newMap = new();
        PlacedMapData data = LoadMapData(path);
        foreach(MapObjectData obj in data.Objects)
        {
            newMap.AddObject(
                new MapObject(
                    content,
                    new Rectangle(
                        obj.Position.X,
                        obj.Position.Y,
                        obj.Position.Width,
                        obj.Position.Height
                    ),
                    obj.TextureSource,
                    obj.Collidable
                )
            );
        }
        return newMap;
    }

    private static PlacedMapData LoadMapData(string path)
    {
        string json = File.ReadAllText(path);
        PlacedMapData? data = JsonSerializer.Deserialize<PlacedMapData>(json);
        if (data == null) throw new InvalidOperationException("Failed to deserialize map JSON.");
        return data;
    }
}