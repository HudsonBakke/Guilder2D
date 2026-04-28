using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

/// <summary>
/// Holds data for map objects
/// </summary>
public abstract class MapObject : IEntity
{
    protected readonly bool _collidable;
    protected Texture2D _texture;

    public MapObject(Texture2D texture, bool collidable = false)
    {
        _texture = texture;
        Width = _texture.Width;
        Height = _texture.Height;
        Position = Vector2.Zero;
        _collidable = collidable;
    }

    public int Width { get; }
    public int Height { get; }
    public Vector2 Position { get; private set; }
    public Rectangle Hitbox => new(
        (int)Position.X,
        (int)Position.Y,
        Width,
        Height
    );
    public Rectangle InteractRange => new(
        Hitbox.X - 50,
        Hitbox.Y - 50,
        Width + 100,
        Height + 100
    );

    public bool CollidesWith(IEntity other)
        => Hitbox.Intersects(other.Hitbox) && _collidable;

    public bool ContainsMouse(Point mousePos)
        => Hitbox.Contains(mousePos);

    public bool IsInInteractRange(IEntity other)
        => InteractRange.Intersects(other.Hitbox);

    public MapObject UpdatePos(Vector2 newPos)
    {
        Position = newPos;
        return this;
    }

    // "Select" and "Interact" will be the 2 predefined ways for the player to interact with world objects.
    // "Select" is basically left-click, and "Interact" is basically right-click
    public abstract void OnSelect();
    public abstract void OnInteract();

    public void Update(GameTime gameTime, Map? map = null)
    {
        
    }
    
    public void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        Vector2 drawPos = camera.WorldToScreen(new Vector2(Hitbox.X, Hitbox.Y));

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

public static class MapObjectFactory
{
    public static MapObject GetMapObject(AssetManager assets, string id)
    {
        return id switch
        {
            "Pot" => new Pot(assets),
            _ => null,
        };
    }
}