using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

public class Hurtbox
{
    private Texture2D _texture;
    private double _timer;

    public Hurtbox(Texture2D texture, double duration)
    {
        _texture = texture;
        Width = _texture.Width;
        Height = _texture.Height;

        _timer = duration;
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

    public bool CollidesWith(IEntity other)
        => Hitbox.Intersects(other.CollisionBox);

    public void Update(GameTime gameTime, Map map)
    {
        _timer -= gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        throw new System.NotImplementedException();
    }
}