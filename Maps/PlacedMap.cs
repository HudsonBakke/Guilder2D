using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

public class PlacedMap : Map
{
    public PlacedMap(ContentManager content, TilesetData tilesetData, List<List<int>> tiles)
    {
        _objects = [];

        _tileAtlas = content.Load<Texture2D>(tilesetData.TextureAsset);
        _tileWidth = tilesetData.TileWidth;
        _tileHeight = tilesetData.TileHeight;
        foreach (var (strId, pos) in tilesetData.TileMappings)
        {
            _tileAtlasPositions[int.Parse(strId)] = new Rectangle(
                pos[0] * tilesetData.TileWidth, 
                pos[1] * tilesetData.TileHeight,
                tilesetData.TileWidth,
                tilesetData.TileHeight
            );
        }
        _collisionTiles = [.. tilesetData.CollisionTiles];
        _tiles = tiles;
    }

    public override void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        Rectangle visibleWorldRect = new(
            (int)camera.Position.X,
            (int)camera.Position.Y,
            Guilder2D.VirtualWidth,
            Guilder2D.VirtualHeight
        );

        int startCol = Math.Max(0, visibleWorldRect.Left / _tileWidth);
        int endCol = Math.Min(WidthInTiles - 1, visibleWorldRect.Right / _tileWidth);
        int startRow = Math.Max(0, visibleWorldRect.Top / _tileHeight);
        int endRow = Math.Min(HeightInTiles - 1, visibleWorldRect.Bottom / _tileHeight);

        for (int row = startRow; row <= endRow; row++)
        {
            for (int col = startCol; col <= endCol; col++)
            {
                int tileId = _tiles[row][col];

                if (tileId == 0)
                    continue;

                if (!_tileAtlasPositions.TryGetValue(tileId, out Rectangle sourceRect))
                    continue;

                Vector2 worldPos = new(col * _tileWidth, row * _tileHeight);
                Vector2 screenPos = camera.WorldToScreen(worldPos);

                spriteBatch.Draw(
                    texture: _tileAtlas,
                    position: screenPos,
                    sourceRectangle: sourceRect,
                    color: Color.White
                );
            }
        }

        foreach (MapObject obj in _objects)
        {
            obj.Draw(spriteBatch, camera);
        }
    }
}