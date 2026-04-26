using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Guilder2D;

public class PlacedMapData
{
    public string TilesetName { get; set; } = "";
    public List<List<int>> Tiles { get; set; } = [];
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
    public static PlacedMap LoadMap(AssetManager assets, string path)
    {
        PlacedMapData data = LoadMapData(path);
        PlacedMap newMap = new(
            assets,
            TilesetLoader.LoadTilesetData(data.TilesetName),
            data.Tiles
        );
        foreach(MapObjectData obj in data.Objects)
        {
            newMap.AddObject(
                new MapObject(
                    assets.LoadTexture(obj.TextureSource),
                    new Rectangle(
                        obj.Position.X,
                        obj.Position.Y,
                        obj.Position.Width,
                        obj.Position.Height
                    ),
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