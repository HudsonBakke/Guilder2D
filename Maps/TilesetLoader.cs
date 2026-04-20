using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Guilder2D;

public class TilesetData
{
    public string TextureAsset { get; set; } = "";
    public int TileWidth { get; set; }
    public int TileHeight { get; set; }
    public Dictionary<string, int[]> TileMappings { get; set; } = [];
    public HashSet<int> CollisionTiles { get; set; } = [];
}

public static class TilesetLoader
{
    public static TilesetData LoadTilesetData(string tilesetName)
    {
        string path = Path.Combine("Content", "Tilesets", tilesetName, "tileset.json");
        string json = File.ReadAllText(path);
        TilesetData? data = JsonSerializer.Deserialize<TilesetData>(json);
        if (data == null) throw new InvalidOperationException("Failed to deserialize map JSON.");
        return data;
    }
}