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

    public MapObject(ContentManager content, Rectangle position, string textureSource, bool collidable = false)
    {
        Position = position;
        _collidable = collidable;
        _texture = content.Load<Texture2D>(textureSource);
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
    protected readonly int _tileWidth;
    protected readonly int _tileHeight;
    protected readonly Rectangle[,] _tiles;
    protected List<MapObject> _objects;

    /// <summary>
    /// Used to add objects to the map
    /// </summary>
    /// <param name="mapObject">The object to be added</param>
    public void AddObject(MapObject mapObject)
    {
        _objects.Add(mapObject);
    }

    /// <summary>
    /// Loops through all map objects to see if there is a collision
    /// </summary>
    /// <param name="hitbox">Hitbox for collision</param>
    /// <returns></returns>
    public bool CollidesWith(Rectangle hitbox)
        => _objects.Any(obj => obj.CollidesWith(hitbox));

    /// <summary>
    /// Call this method to draw the map and its objects to the screen.
    /// </summary>
    /// <param name="spriteBatch">MonoGame SpriteBatch object</param>
    public abstract void Draw(SpriteBatch spriteBatch, Camera camera);
}