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
    public List<int> Position { get; set; } = [];
    public string Asset { get; set; } = "";
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
                MapObjectFactory.GetMapObject(
                    assets, obj.Asset
                ).UpdatePos(new Vector2(obj.Position[0], obj.Position[1]))
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