using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

/// <summary>
/// Holds data for map objects
/// </summary>
public class MapObject
{
    private readonly bool _collidable;
    private readonly Texture2D _texture;

    public MapObject(Texture2D texture, Rectangle position, bool collidable = false)
    {
        Position = position;
        _collidable = collidable;
        _texture = texture;
    }

    public Rectangle Position { get; private set; }

    /// <summary>
    /// Use to detect collision
    /// </summary>
    /// <param name="hitbox">Hitbox for collision</param>
    /// <returns>bool</returns>
    public bool CollidesWith(Rectangle hitbox)
        => _collidable && Position.Intersects(hitbox);
    
    /// <summary>
    /// Call this method to draw the object to the screen.
    /// </summary>
    /// <param name="spriteBatch">MonoGame SpriteBatch object</param>
    public void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        Vector2 drawPos = camera.WorldToScreen(new Vector2(Position.X, Position.Y));

        spriteBatch.Draw(
            texture: _texture,
            position: drawPos,
            sourceRectangle: new Rectangle(
                0,
                0,
                _texture.Width,
                _texture.Height
            ),
            color: Color.White
        );
    }
}

/// <summary>
/// Holds game maps. May be procedural or placed.
/// </summary>
public abstract class Map
{
    protected int _tileWidth;
    protected int _tileHeight;
    protected List<List<int>> _tiles;
    protected Dictionary<int, Rectangle> _tileAtlasPositions = [];
    protected HashSet<int> _collisionTiles;
    protected Texture2D _tileAtlas;
    protected List<MapObject> _objects;

    public int WidthInTiles => _tiles.Count == 0 ? 0 : _tiles[0].Count;
    public int HeightInTiles => _tiles.Count;
    public int WidthInPixels => WidthInTiles * _tileWidth;
    public int HeightInPixels => HeightInTiles * _tileHeight;

    /// <summary>
    /// Used to add objects to the map
    /// </summary>
    /// <param name="mapObject">The object to be added</param>
    public void AddObject(MapObject mapObject)
    {
        _objects.Add(mapObject);
    }

    /// <summary>
    /// Loops through all map objects and tiles to see if there is a collision
    /// </summary>
    /// <param name="hitbox">Hitbox for collision</param>
    /// <returns>True if collision is detected, false if not</returns>
    public bool CollidesWith(Rectangle hitbox)
    {
        // First check objects
        if (_objects.Any(obj => obj.CollidesWith(hitbox)))
            return true;

        // Convert hitbox to tile coordinates
        int startCol = Math.Max(0, hitbox.Left / _tileWidth);
        int endCol = Math.Min(WidthInTiles - 1, (hitbox.Right - 1) / _tileWidth);
        int startRow = Math.Max(0, hitbox.Top / _tileHeight);
        int endRow = Math.Min(HeightInTiles - 1, (hitbox.Bottom - 1) / _tileHeight);

        for (int row = startRow; row <= endRow; row++)
        {
            for (int col = startCol; col <= endCol; col++)
            {
                int tileId = _tiles[row][col];

                if (_collisionTiles.Contains(tileId))
                    return true; // early exit immediately
            }
        }

        return false;
    }

    /// <summary>
    /// Call this method to draw the map and its objects to the screen.
    /// </summary>
    /// <param name="spriteBatch">MonoGame SpriteBatch object</param>
    public abstract void Draw(SpriteBatch spriteBatch, Camera camera);
}